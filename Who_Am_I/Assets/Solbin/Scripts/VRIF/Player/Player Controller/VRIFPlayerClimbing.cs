using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace BNG
{
    public class VRIFPlayerClimbing : MonoBehaviour
    {
        // Player Rigidbody 
        private Rigidbody playerRigid;

        // 호출에 대한 AddForce 실행 수 제한 
        private bool addForce = false;

        [Header("플레이어")]
        [SerializeField] private Transform player = default;

        [Header("Player State.cs")]
        [SerializeField] private VRIFStateSystem vrifStateSystem = default;

        [Header("Grabbers")]
        [SerializeField] private Grabber leftGrabber = default;
        [SerializeField] private Grabber rightGrabber = default;

        [Header("측면 점프값")]
        public float upJumpForce = 3.5f; // 위로 점프하는 힘
        public float sideJumpForce = 1.5f; // 측면으로 점프하는 힘

        [Header("상승 점프값")]
        public float superupJumpForce = 4f;

        [Header("Climbing Anchor")]
        [SerializeField] private Transform climbingAnchor = default;

        private void Start()
        {
            playerRigid = GetComponent<Rigidbody>(); // 점프에 사용할 Rigidbody
        }

        /// <summary>
        /// 등반 점프
        /// </summary>
        public void DoJump(VRIFMap_ClimberJump.Direction dir_, Transform anchor_)
        {
            leftGrabber.ReleaseGrab(); // 양쪽 손을 놓게 한다. 
            rightGrabber.ReleaseGrab();

            if ((VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.CLIMBING) && !addForce)
            {
                addForce = true;

                switch(dir_)
                {
                    case VRIFMap_ClimberJump.Direction.Left:
                        playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                        playerRigid.AddForce(-anchor_.right * sideJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
                        break;

                    case VRIFMap_ClimberJump.Direction.Right:
                        playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                        playerRigid.AddForce(anchor_.right * sideJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
                        break;

                    case VRIFMap_ClimberJump.Direction.Up:
                        playerRigid.AddForce(Vector3.up * superupJumpForce, ForceMode.Impulse); // 상승 점프
                        break;
                }

                Invoke("ClearAddForce", 0.5f); // 호출 정도 제한
            }
        }

        /// <summary>
        /// 재작동을 위해 bool 값 초기화 
        /// </summary>
        private void ClearAddForce() { addForce = false; }
    }
}
