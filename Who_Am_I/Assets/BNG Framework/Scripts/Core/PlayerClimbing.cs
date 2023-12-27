using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class PlayerClimbing : MonoBehaviour { 

        [Header("Climbing Transforms")]
        public Transform LeftControllerTransform; // <Solbin> 왼쪽 컨트롤러 Transform
        public Transform RightControllerTransform; // <Solbin> 오른쪽 컨트롤러 Transform 

        [Header("Capsule Settings")]
        [Tooltip("Set the player's capsule collider height to this amount while climbing. This can allow you to shorten the capsule collider a bit, making it easier to navigate over ledges.")]
        public float ClimbingCapsuleHeight = 0.5f; // <Solbin> 등반 캡슐 콜라이더 크기

        [Tooltip("Set the player's capsule collider capsule center to this amount while climbing.")]
        public float ClimbingCapsuleCenter = -0.25f; // <Solbin> 등반 캡슐 콜라이더 센터 

        [Header("Haptics")]
        public bool ApplyHapticsOnGrab = true;

        [Tooltip("Frequency of haptics to play on grab if 'ApplyHapticsOnGrab' is true")]
        public float VibrateFrequency = 0.3f;

        [Tooltip("Amplitute of haptics to play on grab if 'ApplyHapticsOnGrab' is true")]
        public float VibrateAmplitude = 0.1f;

        [Tooltip("Duration of haptics to play on grab if 'ApplyHapticsOnGrab' is true")]
        public float VibrateDuration = 0.1f;

        // Any climber grabbers in use
        List<Grabber> climbers; // <Solbin> 사용 중인 Grabber의 리스트 (양 손을 다 그랩에 쓰고 있다면 climbers의 Count는 둘?)

        bool wasGrippingClimbable;

        CharacterController characterController; // <Solbin> Character Controller
        SmoothLocomotion smoothLocomotion;
        PlayerGravity playerGravity; // <Solbin> Player Gravity
        Rigidbody playerRigid; // <Solbin> Player Rigidbody

        // <Solbin> Player가 Rigidbody를 가지고 있는지 판단
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

        Vector3 previousLeftControllerPosition; // <Solbin> 왼쪽 컨트롤러 position
        Vector3 previousRightControllerPosition; // <Solbin> 오른쪽 컨트롤러 position

        Vector3 controllerMoveAmount;

        // <Solbin> 한 손 이상 그랩 중인지 체크하는 이벤트
        public event EventHandler climbingEvent;
        // <Solbin> 왼손 그랩 이벤트
        public event EventHandler leftClimbingEvent;
        // <Solbin> 오른손 그랩 이벤트
        public event EventHandler rightClimbingEvent;
        // <Solbin> VRIFStateSystem
        [SerializeField] private VRIFStateSystem vrifStateSystem = default;
        // <Solbin> ===

        // Start is called before the first frame update
        public void Start() {
            climbers = new List<Grabber>();
            characterController = GetComponentInChildren<CharacterController>();
            smoothLocomotion = GetComponentInChildren<SmoothLocomotion>();
            playerGravity = GetComponentInChildren<PlayerGravity>();
            playerRigid = GetComponent<Rigidbody>(); // <Solbin> 등반 중 점프를 위해 Rigidbody를 추가했으므로 null이 아니다
        }

        public void LateUpdate() {
            checkClimbing();

            // <Solbin> 양쪽 컨트롤러 position을 업데이트
            // Update Controller Positions
            if (LeftControllerTransform != null) {
                previousLeftControllerPosition = LeftControllerTransform.position;
            }
            if (RightControllerTransform != null) {
                previousRightControllerPosition = RightControllerTransform.position;
            }
        }

        /// <Problem> 왜 DummyTransform과 Grabber의 좌표에 차이가 나는가? (DummyTransform이 올바른 좌표.)
        // <Solbin> Climbable 컴포넌트가 달린 물체를 그랩 할 때 작동한다
        public virtual void AddClimber(Climbable climbable, Grabber grab) { // <Solbin> climbable은 잡힌 물체, grab은 양쪽 손 

            if (climbers == null) {
                climbers = new List<Grabber>();
            }

            if (!climbers.Contains(grab)) { // <Solbin> 그랩 중인 손이 아니라면 

                if (grab.DummyTransform == null) { // <Solbin> grab.DummyTransform의 기본 세팅
                    GameObject go = new GameObject(); // <Solbin> go는 임시 컨테이너 오브젝트일 뿐이다
                    go.transform.name = "DummyTransform";
                    go.transform.parent = grab.transform; // <Solbin> 왼쪽 오른쪽 Grabber?
                    go.transform.position = grab.transform.position;
                    go.transform.localEulerAngles = Vector3.zero;

                    grab.DummyTransform = go.transform; // <Solbin> GameObject go를 조정 후 grab.DummyTransform에 할당
                }

                // <Solbin> 등반 중인 손 DummyTransform의 부모를 등반 물체로 설정
                // Set parent to whatever we grabbed. This way we can follow the object around if it moves
                // <Solbin> DummyTransform의 부모를 바꾸니 이후 grab.DummyTransform은 null로 처리된다. => 다음 그랩 처리 가능
                grab.DummyTransform.parent = climbable.transform;
                // <Solbin> 직전 position으로 설정한다. 
                grab.PreviousPosition = grab.DummyTransform.position;

                // Play haptics
                if(ApplyHapticsOnGrab) {
                    InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, grab.HandSide);
                }

                // <Solbin> 그리고 그 정보(grab)을 climbers 리스트에 저장 
                climbers.Add(grab);
            }
        }

        public virtual void RemoveClimber(Grabber grab) { // <Solbin> 등반 물체를 놓을 경우 climbers 리스트에서 제거 
            if (climbers.Contains(grab)) {
                // Reset grabbable parent
                grab.DummyTransform.parent = grab.transform; // <Solbin> DummtTransform의 부모는 다시 등반 물체에서 다시 손이 된다. 
                grab.DummyTransform.localPosition = Vector3.zero; // <Solbin> 위치 재정비

                climbers.Remove(grab); // <Solbin> climbers 리스트에서 제거 
            }
        }

        // <Solbin> 하나 이상의 등반 물체를 잡고 있는지 체크하는 메소드
        public virtual bool GrippingAtLeastOneClimbable() {

            if (climbers != null && climbers.Count > 0) {

                for (int x = 0; x < climbers.Count; x++) { // <Solbin> 그랩 중인 손 리스트를 검사
                    // Climbable is still being held
                    if (climbers[x] != null && climbers[x].HoldingItem) { // <Solbin> 개별 손이 null이 아니며 그랩 중임이 확인되면

                        // <Solbin> 등반 이벤트 발생
                        climbingEvent?.Invoke(this, EventArgs.Empty);
                        // <Solbin> ===

                        return true;
                    }
                }

                // If we made it through every climber and none were valid, reset the climbers
                climbers = new List<Grabber>(); // <Solbin> 유효치 않다면 재설정
            }

            return false;
        }

        protected virtual void checkClimbing() {
            GrippingClimbable = GrippingAtLeastOneClimbable(); // <Solbin> 하나 이상의 물체를 잡고 있을 때 true

            // Check events
            if (GrippingClimbable && !wasGrippingClimbable) // <Solbin> 하나 이상의 물체를 잡고 있을때
            {
                onGrabbedClimbable();
            }

            if (wasGrippingClimbable && !GrippingClimbable) // <Solbin> 하나 이상의 물체를 잡고 있지 않을때
            {
                onReleasedClimbable();
            }

            if (GrippingClimbable) { // <Solbin> 한 손 이상 그랩 중임이 확인될 때 

                moveDirection = Vector3.zero;

                int count = 0;
                float length = climbers.Count; // <Solbin> 그랩 중인 손의 수
                for (int i = 0; i < length; i++) {
                    Grabber climber = climbers[i];
                    if (climber != null && climber.HoldingItem) { // <Solbin> 각 손이 그랩 중임이 확인될 때 

                        //<Solbin> 왼손/오른손을 구분해 등반, 그랩을 체크
                        // Add hand offsets
                        if (climber.HandSide == ControllerHand.Left) { // <Solbin> 왼손이면
                            // <Solbin> 직전 손의 위치와 현재 손의 위치를 이용해 방향 구하기
                            controllerMoveAmount = previousLeftControllerPosition - LeftControllerTransform.position;

                            // <Solbin> 
                            leftClimbingEvent?.Invoke(this, EventArgs.Empty); // 왼손 등반 이벤트 발생
                            // <Solbin> ===
                        }
                        else { 
                            controllerMoveAmount = previousRightControllerPosition - RightControllerTransform.position;
                            // <Solbin>
                            rightClimbingEvent?.Invoke(this, EventArgs.Empty); // 오른손 등반 이벤트 발생
                            // <Solbin> ===
                        }

                        // Always use last grabbed hand
                        if (count == length - 1) {  // <Solbin> 그랩 중인 손이 하나라면?
                            moveDirection = controllerMoveAmount; // <Solbin> moveDirection 재설정

                            // Check if Climbable object moved position
                            moveDirection -= climber.PreviousPosition - climber.DummyTransform.position;
                        }

                        count++;
                    }
                }

                // Apply movement to player
                if (smoothLocomotion) { // <Solbin> Rigidbody의 활성 여부에 따른 이동 방식 변경 
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

        void DoPhysicalClimbing() { // <Solbin> Rigidbody 컴포넌트 확인 시 
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

        // <Solbin> 등반 물체를 잡았을 때 한 번만 실행 
        void onGrabbedClimbable() 
        {
            if (VRIFStateSystem.Instance.gameState != VRIFStateSystem.GameState.LADDER) // 사다리를 타고 있는 중이 아닐 때만 CLIMBING
            {
                VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.CLIMBING);
            }

            // <Solbin> 등반 중 그랩은 Velocity에 힘이 가해지고 있는 중이면 제대로 작동하지 않는다. 
            // TODO: 추후 상승 점프 후 첫번째 그립 보정 필요. 
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Don't allow player movement while climbing
            if (smoothLocomotion) { // <Solbin> 기본 움직임 비활성화
                smoothLocomotion.DisableMovement();
            }

            // No gravity on the player while climbing
            if (playerGravity) {
                playerGravity.ToggleGravity(false); // <Solbin> 중력 비활성화
            }
        }

        // <Solbin> 등반 중이 아닐때 (public으로 교체함)
        public void onReleasedClimbable() 
        {
            VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.NORMAL);

            // Reset back to our original values
            if (smoothLocomotion) {
                smoothLocomotion.EnableMovement(); // <Solbin> 기본 움직임 활성화
            }

            // Gravity back to normal
            if (playerGravity) {
                playerGravity.ToggleGravity(true); // <Solbin> 중력 활성화
            }
        }
    }
}

