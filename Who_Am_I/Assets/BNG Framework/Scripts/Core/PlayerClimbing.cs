using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class PlayerClimbing : MonoBehaviour { 

        [Header("Climbing Transforms")]
        public Transform LeftControllerTransform;
        public Transform RightControllerTransform;

        [Header("Capsule Settings")]
        [Tooltip("Set the player's capsule collider height to this amount while climbing. This can allow you to shorten the capsule collider a bit, making it easier to navigate over ledges.")]
        public float ClimbingCapsuleHeight = 0.5f;

        [Tooltip("Set the player's capsule collider capsule center to this amount while climbing.")]
        public float ClimbingCapsuleCenter = -0.25f;

        [Header("Haptics")]
        public bool ApplyHapticsOnGrab = true;

        [Tooltip("Frequency of haptics to play on grab if 'ApplyHapticsOnGrab' is true")]
        public float VibrateFrequency = 0.3f;

        [Tooltip("Amplitute of haptics to play on grab if 'ApplyHapticsOnGrab' is true")]
        public float VibrateAmplitude = 0.1f;

        [Tooltip("Duration of haptics to play on grab if 'ApplyHapticsOnGrab' is true")]
        public float VibrateDuration = 0.1f;

        // <Solbin> 사용 중인 Grabber의 리스트
        // Any climber grabbers in use
        List<Grabber> climbers;

        bool wasGrippingClimbable;

        CharacterController characterController;
        SmoothLocomotion smoothLocomotion;
        PlayerGravity playerGravity;
        Rigidbody playerRigid;

        public bool IsRigidbodyPlayer {
            get {
                if (_checkedRigidPlayer) 
                {
                    return _isRigidPlayer;
                }
                else 
                {
                    _isRigidPlayer = smoothLocomotion != null && smoothLocomotion.ControllerType == PlayerControllerType.Rigidbody;
                    _checkedRigidPlayer = true;
                    return _isRigidPlayer;
                }
            }
        }

        bool _checkedRigidPlayer = false;
        bool _isRigidPlayer = false;

        // <Solbin> 하나 혹은 이상의 grabber를 홀드하고 있는지 여부
        [Header("Shown for Debug : ")]
        /// <summary>
        /// Whether or not we are currently holding on to something climbable with 1 or more grabbers
        /// </summary>
        public bool GrippingClimbable = false;

        private Vector3 moveDirection = Vector3.zero;

        Vector3 previousLeftControllerPosition;
        Vector3 previousRightControllerPosition;

        Vector3 controllerMoveAmount;

        // <Solbin> 한 손 이상 그랩 중인지 체크하는 이벤트
        public event EventHandler climbingEvent;
        // <Solbin> 왼손 그랩 이벤트
        public event EventHandler leftClimbingEvent;
        // <Solbin> 오른손 그랩 이벤트
        public event EventHandler rightClimbingEvent;
        // <Solbin> ===

        // Start is called before the first frame update
        public void Start() {
            climbers = new List<Grabber>();
            characterController = GetComponentInChildren<CharacterController>();
            smoothLocomotion = GetComponentInChildren<SmoothLocomotion>();
            playerGravity = GetComponentInChildren<PlayerGravity>();
            playerRigid = GetComponent<Rigidbody>();
        }

        public void LateUpdate() {
            checkClimbing();

            // Update Controller Positions
            if (LeftControllerTransform != null) {
                previousLeftControllerPosition = LeftControllerTransform.position;
            }
            if (RightControllerTransform != null) {
                previousRightControllerPosition = RightControllerTransform.position;
            }
        }

        public virtual void AddClimber(Climbable climbable, Grabber grab) {

            if (climbers == null) {
                climbers = new List<Grabber>();
            }

            if (!climbers.Contains(grab)) {

                if (grab.DummyTransform == null) {
                    GameObject go = new GameObject();
                    go.transform.name = "DummyTransform";
                    go.transform.parent = grab.transform;
                    go.transform.position = grab.transform.position;
                    go.transform.localEulerAngles = Vector3.zero;

                    grab.DummyTransform = go.transform;
                }

                // Set parent to whatever we grabbed. This way we can follow the object around if it moves
                grab.DummyTransform.parent = climbable.transform;
                
                grab.PreviousPosition = grab.DummyTransform.position;

                // Play haptics
                if(ApplyHapticsOnGrab) {
                    InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, grab.HandSide);
                }

                climbers.Add(grab);
            }
        }

        public virtual void RemoveClimber(Grabber grab) {
            if (climbers.Contains(grab)) {
                // Reset grabbable parent
                grab.DummyTransform.parent = grab.transform;
                grab.DummyTransform.localPosition = Vector3.zero;

                climbers.Remove(grab);
            }
        }

        // <Solbin> 하나 이상의 등반 물체를 잡고 있는지 체크하는 메소드
        public virtual bool GrippingAtLeastOneClimbable() {

            if (climbers != null && climbers.Count > 0) {

                for (int x = 0; x < climbers.Count; x++) {
                    // Climbable is still being held
                    if (climbers[x] != null && climbers[x].HoldingItem) {

                        // <Solbin> 등반 이벤트 발생
                        climbingEvent?.Invoke(this, EventArgs.Empty);
                        // <Solbin> ===

                        return true;
                    }
                }

                // If we made it through every climber and none were valid, reset the climbers
                climbers = new List<Grabber>();
            }

            return false;
        }

        protected virtual void checkClimbing() {
            GrippingClimbable = GrippingAtLeastOneClimbable();

            // TODO: 아래 부분을 체크하면 측면 점프 때 손을 놓게 하는 동작이 가능할 것 같다. 
            // Check events
            if (GrippingClimbable && !wasGrippingClimbable) 
            {
                onGrabbedClimbable();
            }

            if (wasGrippingClimbable && !GrippingClimbable) 
            {
                onReleasedClimbable();
            }

            if (GrippingClimbable) {

                moveDirection = Vector3.zero;

                int count = 0;
                float length = climbers.Count;
                for (int i = 0; i < length; i++) {
                    Grabber climber = climbers[i];
                    if (climber != null && climber.HoldingItem) {

                        //<Solbin> 왼손/오른손을 구분해 등반, 그랩을 체크
                        // Add hand offsets
                        if (climber.HandSide == ControllerHand.Left) {
                            controllerMoveAmount = previousLeftControllerPosition - LeftControllerTransform.position;

                            // <Solbin> 왼손 등반 이벤트 발생
                            leftClimbingEvent?.Invoke(this, EventArgs.Empty);
                            // <Solbin> ===
                        }
                        else {
                            controllerMoveAmount = previousRightControllerPosition - RightControllerTransform.position;
                            // <Solbin> 오른손 등반 이벤트 발생
                            rightClimbingEvent?.Invoke(this, EventArgs.Empty);
                            // <Solbin> ===
                        }

                        // Always use last grabbed hand
                        if (count == length - 1) {
                            moveDirection = controllerMoveAmount;

                            // Check if Climbable object moved position
                            moveDirection -= climber.PreviousPosition - climber.DummyTransform.position;
                        }

                        count++;
                    }
                }

                // Apply movement to player
                if (smoothLocomotion) {
                    if(smoothLocomotion.ControllerType == PlayerControllerType.CharacterController) {
                        smoothLocomotion.MoveCharacter(moveDirection);
                    }
                    else if(smoothLocomotion.ControllerType == PlayerControllerType.Rigidbody) {
                        DoPhysicalClimbing();
                    }
                }
                else if(characterController) {
                    characterController.Move(moveDirection);
                }
            }

            // Update any climber previous position
            for (int x = 0; x < climbers.Count; x++) {
                Grabber climber = climbers[x];
                if (climber != null && climber.HoldingItem) {
                    if (climber.DummyTransform != null) {
                        // Use climber position if possible
                        climber.PreviousPosition = climber.DummyTransform.position;
                    }
                    else {
                        climber.PreviousPosition = climber.transform.position;
                    }
                }
            }

            wasGrippingClimbable = GrippingClimbable;
        }

        void DoPhysicalClimbing() {
            int count = 0;
            float length = climbers.Count;

            Vector3 movementVelocity = Vector3.zero;

            for (int i = 0; i < length; i++) {
                Grabber climber = climbers[i];
                if (climber != null && climber.HoldingItem) {

                    Vector3 positionDelta = climber.transform.position - climber.DummyTransform.position;

                    // Always use last grabbed hand
                    if (count == length - 1) {
                        movementVelocity = positionDelta;

                        // Check if Climbable object moved position
                        movementVelocity -= climber.PreviousPosition - climber.DummyTransform.position;
                    }

                    count++;
                }
            }

            if(movementVelocity.magnitude > 0) {
                playerRigid.velocity = Vector3.MoveTowards(playerRigid.velocity, (-movementVelocity * 2000f) * Time.fixedDeltaTime, 1f);
            }
        }

        // <Solbin> public으로 교체
        void onGrabbedClimbable() {
            
            // Don't allow player movement while climbing
            if (smoothLocomotion) {
                smoothLocomotion.DisableMovement();
            }

            // No gravity on the player while climbing
            if (playerGravity) {
                playerGravity.ToggleGravity(false);
            }
        }

        // <Solbin> public으로 교체 
        void onReleasedClimbable() {
            // Reset back to our original values
            if (smoothLocomotion) {
                smoothLocomotion.EnableMovement();
            }

            // Gravity back to normal
            if (playerGravity) {
                playerGravity.ToggleGravity(true);
            }
        }

        // <Solbin> Test: 손을 놓는 메소드 (아래 Test 메소드는 테스트용 )
        void Test()
        {
            if ( Input.GetKeyDown(KeyCode.K))
            {
                foreach (Grabber climber in climbers)
                {
                    
                }
            }
        }
    }
}

