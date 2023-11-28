using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BNG
{
    public class VRIFPlayerClimbing : MonoBehaviour
    {
        // (Asset) PlayerClimbing
        [SerializeField] private PlayerClimbing asset_playerClimbing;
        // Player Rigidbody 
        private Rigidbody playerRigid;
        // VRIF Action
        VRIFAction vrifAction;
        // (Asset) PlayerClimbing
        PlayerClimbing playerClimbing;

        private bool leftGrab = false;
        private bool rightGrab = false;
        private bool readySuperJump = false;

        private void Start()
        {
            Setting();
        }

        #region 초기 세팅
        private void Setting()
        {
            asset_playerClimbing.climbingEvent += ActivateSideJump; // 등반 중 이벤트 구독
            playerRigid = GetComponent<Rigidbody>(); // 점프에 사용할 Rigidbody

            asset_playerClimbing.leftClimbingEvent += LeftGrabCheck;
            asset_playerClimbing.rightClimbingEvent += RightGrabCheck;

            playerClimbing = GetComponent<PlayerClimbing>();
        }

        private void OnEnable()
        {
            vrifAction = new VRIFAction();
            vrifAction.Enable();
        }

        private void OnDisable()
        {
            vrifAction?.Disable();
        }
        #endregion

        #region 그랩 체크
        private void LeftGrabCheck(object sender, EventArgs e) { leftGrab = true; }

        private void RightGrabCheck(object sender, EventArgs e) { rightGrab = true; }

        private void Update()
        {
           SuperJump();
        }
        #endregion

        #region 상승 점프
        private void SuperJump()
        {
            if (leftGrab && rightGrab) // 양손 다 그랩 상태일때
            {
                float jump = -1f; // 상승 점프 조건 

                if (VRIFControllerSystem.lVelocity.y <= jump && VRIFControllerSystem.rVelocity.y <= jump) // 아래로 휘두르기 
                {
                    readySuperJump = true; // 상승 점프 가능 상태 
                }

                leftGrab = false;
                rightGrab = false;
            }

            if (readySuperJump)
            {
                float jumpForce = 5f;

                if (vrifAction.Player.LeftGrip.ReadValue<float>() <= 0.45f &&
                vrifAction.Player.RightGrip.ReadValue<float>() <= 0.45f) // 아래로 휘두르며 손을 놓았다면
                {
                    Debug.LogWarning("Super Jump!");

                    playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 상승 점프
                    readySuperJump = false;
                }

                Invoke("ClearSuperJump", 0.7f); // 시간차 상승 점프 상태 해제 (그랩을 놓는 동작이 늦을 경우를 대비)
            }
        }

        private void ClearSuperJump() { readySuperJump = false; }
        #endregion

        #region 측면 점프
        /// <summary>
        /// 측면 점프 - 한 손 이상 그랩 상태일때 활성화
        /// </summary>
        private void ActivateSideJump(object sender, EventArgs e)
        {
            if (vrifAction.Player.ClimbingLeftJump.triggered) { LeftJump(); }
            else if (vrifAction.Player.ClimbingRightJump.triggered) { RightJump(); }
        }

        /// <summary>
        /// 측면 점프 - 좌측
        /// </summary>
        private void LeftJump()
        {
            float upJumpForce = 1.5f; // 위로 점프하는 힘
            float leftJumpForce = 1f; // 좌측으로 점프하는 힘

            playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
            playerRigid.AddForce(Vector3.left * leftJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
        }

        /// <summary>
        /// 측면 점프 - 우측
        /// </summary>
        private void RightJump()
        {
            float upJumpForce = 1.5f; // 위로 점프하는 힘
            float leftJumpForce = 1f; // 좌측으로 점프하는 힘

            playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
            playerRigid.AddForce(Vector3.right * leftJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
        }
        #endregion
    }
}
