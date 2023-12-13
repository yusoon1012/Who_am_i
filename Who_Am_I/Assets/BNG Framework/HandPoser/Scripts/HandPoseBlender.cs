using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG {
    public class HandPoseBlender : MonoBehaviour {

        // <Solbin> 아랫값이 참이면 입력값을 읽는다. 
        [Header("Run in Update")]
        [Tooltip("If true the HandPoser will be updated in Update by reading ThumbValue, IndexValue, and GripValue")]
        public bool UpdatePose = true;

        /// <Point> 다른 아바타와 Rig가 적용된 3D 손모델을 적용했을때 움직임이 적용되지 않는 문제가 있는데, 아래 Pose1과 Pose2의 세팅이 잘못되었기 때문이다.
        /// 새로 세팅한 Rig를 적용한 HandPose를 지정해주어야 한다.

        // <Solbin> 아래 Pose1에서 Pose2로 혼합된다. 
        [Header("Blend From / To")]
        [Tooltip("(Required) Blend from this hand pose to the Pose2 hand pose.")]
        public HandPose Pose1; 

        // <Solbin> 아래 Pose2에 Pose1을 혼합한다. 
        [Tooltip("(Required) Blend from the Pose1 hand pose to this hand pose.")]
        public HandPose Pose2;

        [Header("Inputs")] // <Solbin> Rig만 짜여진 새로운 3D 손모델 삽입시에도 입력은 받는 것을 확인했다. (문제: 새로 Rig를 배치해도 적용되지 않는다.)
        [Range(0, 1)]
        public float ThumbValue = 0f;

        [Range(0, 1)]
        public float IndexValue = 0f;

        [Range(0, 1)]
        public float MiddleValue = 0f;

        [Range(0, 1)]
        public float RingValue = 0f;

        [Range(0, 1)]
        public float PinkyValue = 0f;

        [Range(0, 1)]
        public float GripValue = 0f;
        private float _lastGripValue;

        protected HandPoser handPoser;

        void Start() {
            handPoser = GetComponent<HandPoser>();
        }

        void Update() {
            if (UpdatePose) {
                UpdatePoseFromInputs();
            }
        }

        /// <summary>
        /// Update the hand pose based on ThumbValue, IndexValue, and GripValue
        /// </summary>
        public virtual void UpdatePoseFromInputs() {
            DoIdleBlendPose();
        }

        public void UpdateThumb(float amount) {
            handPoser.UpdateJoints(Pose2.Joints.ThumbJoints, handPoser.ThumbJoints, amount);
        }

        public void UpdateIndex(float amount) {
            handPoser.UpdateJoints(Pose2.Joints.IndexJoints, handPoser.IndexJoints, amount);
        }

        public void UpdateMiddle(float amount) {
            handPoser.UpdateJoints(Pose2.Joints.MiddleJoints, handPoser.MiddleJoints, MiddleValue);
        }

        public void UpdateRing(float amount) {
            handPoser.UpdateJoints(Pose2.Joints.RingJoints, handPoser.RingJoints, amount);
        }

        public void UpdatePinky(float amount) {
            handPoser.UpdateJoints(Pose2.Joints.PinkyJoints, handPoser.PinkyJoints, amount);
        }

        /// <summary>
        /// Shortcut for updating the middle, ring, and pinky fingers together
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateGrip(float amount) {
            // Then lerp the pinky, ring, and middle finger joints to the Fist position based on grip amount
            MiddleValue = amount;
            RingValue = amount;
            PinkyValue = amount;

            UpdateMiddle(amount);
            UpdateRing(amount);
            UpdatePinky(amount);

            _lastGripValue = amount;
        }

        // <Solbin> 입력값에 따라 손 포즈를 바꾼다. 
        public virtual void DoIdleBlendPose() {
            if (Pose1) {
                // Start at idle
                handPoser.UpdateHandPose(Pose1, false);

                // Then lerp each finger to fist pose depending on input
                UpdateThumb(ThumbValue);
                UpdateIndex(IndexValue);


                // Set Grip Amount only if it changed. This will override Middle, Ring, and Pinky
                if (GripValue != _lastGripValue) {
                    UpdateGrip(GripValue);
                }
                // Otherwise update the remaining fingers independently
                else {
                    UpdateMiddle(MiddleValue);
                    UpdateRing(RingValue);
                    UpdatePinky(PinkyValue);
                }
            }
        }
    }
}

