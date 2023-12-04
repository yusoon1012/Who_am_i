using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// Apply Gravity to a CharacterController or RigidBody
    /// </summary>
    public class PlayerGravity : MonoBehaviour {

        [Tooltip("If true, will apply gravity to the CharacterController component, or RigidBody if no CC is present.")]

        public bool GravityEnabled = true; // <Solbin> 기본적으로 중력이 적용 되도록 설정 되어있다. 

        [Tooltip("Amount of Gravity to apply to the CharacterController or Rigidbody. Default is 'Physics.gravity'.")]
        public Vector3 Gravity = Physics.gravity; // <Solbin> 기본 중력값 y축 -9.82을 말한다.

        CharacterController characterController;
        SmoothLocomotion smoothLocomotion;

        Rigidbody playerRigidbody;

        private float _movementY;
        private Vector3 _initialGravityModifier; // <Solbin> Start()에서 설정되어 기본 중력값을 담고 있는 필드

        // Save us a null check in FixedUpdate
        private bool _validRigidBody = false;

        void Start() {
            characterController = GetComponent<CharacterController>();
            smoothLocomotion = GetComponentInChildren<SmoothLocomotion>();
            playerRigidbody = GetComponent<Rigidbody>();

            _validRigidBody = playerRigidbody != null;

            _initialGravityModifier = Gravity;
        }

        // Apply Gravity in LateUpdate to ensure it gets applied after any character movement is applied in Update
        void LateUpdate() {

            // Apply Gravity to Character Controller
            if (GravityEnabled && characterController != null && characterController.enabled)
            {
                _movementY += Gravity.y * Time.deltaTime;

                // Default to smooth locomotion
                if (smoothLocomotion)
                {
                    smoothLocomotion.MoveCharacter(new Vector3(0, _movementY, 0) * Time.deltaTime);
                }
                // Fallback to character controller
                else if (characterController)
                {
                    characterController.Move(new Vector3(0, _movementY, 0) * Time.deltaTime);
                }

                // Reset Y movement if we are grounded
                if (characterController.isGrounded)
                {
                    _movementY = 0;

                    // <Solbin> AddForce의 영향으로 무한 점프하는 문제가 있어 추가
                    if (playerRigidbody != null)
                    {
                        playerRigidbody.velocity = Vector3.zero;
                    }
                    // <Solbim> ===
                }
            }
        }

        void FixedUpdate() {
            // Apply Gravity to Rigidbody Controller
            if (_validRigidBody && GravityEnabled) {
                //playerRigidbody.AddRelativeForce(Gravity, ForceMode.VelocityChange);
                //playerRigidbody.AddForce(new Vector3(0, -Gravity.y * playerRigidbody.mass, 0));

                if(smoothLocomotion && smoothLocomotion.ControllerType == PlayerControllerType.Rigidbody && smoothLocomotion.GroundContacts < 1) {
                    
                }

                /// ORIGINAL LEGACY
                //playerRigidbody.AddForce(Gravity * playerRigidbody.mass);
                // <Solbin> Rigidbody를 PlayerController 오브젝트에 추가시 중력이 이중적용 되는 문제가 있어 주석처리. 
            }
        }

        public void ToggleGravity(bool gravityOn) {

            GravityEnabled = gravityOn;

            if (gravityOn)
            {
                Gravity = _initialGravityModifier;
            }
            else
            {
                Gravity = Vector3.zero;
            }
        }
    }
}

