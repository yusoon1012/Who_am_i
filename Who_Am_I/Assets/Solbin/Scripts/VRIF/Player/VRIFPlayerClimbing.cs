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
        // (Asset) PlayerClimbing
        private PlayerClimbing asset_playerClimbing;
        // Player Rigidbody 
        private Rigidbody playerRigid;
        // VRIF Action
        VRIFAction vrifAction;
        // 왼손 그랩 여부
        private bool leftGrab = false;
        // 오른손 그랩 여부
        private bool rightGrab = false;
        // 상승 점프 대기 상태 
        private bool readySuperJump = false;
        // 잡고 있는 물체를 놓게 하는 bool 값
        public bool sideJump = false;
        // 호출에 대한 AddForce 실행 수 제한 
        private bool addForce = false;

        private void Start()
        {
            Setting();
        }

        #region 초기 세팅
        private void Setting()
        {
            asset_playerClimbing = GetComponent<PlayerClimbing>();
            asset_playerClimbing.climbingEvent += ActivateSideJump; // 등반 중 이벤트 구독
            playerRigid = GetComponent<Rigidbody>(); // 점프에 사용할 Rigidbody

            asset_playerClimbing.leftClimbingEvent += LeftGrabCheck;
            asset_playerClimbing.rightClimbingEvent += RightGrabCheck;
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

                if (!asset_playerClimbing.GrippingClimbable) // 손을 놓았다면 (등반 중이 아니라면)
                {
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
            int left = 0;
            int right = 1;

            vrifAction.Player.ClimbingLeftJump.started += context => SideJump(left);
            vrifAction.Player.ClimbingRightJump.started += context => SideJump(right);
        }

        /// <summary>
        /// 측면 점프
        /// </summary>
        /// <param name="dir"> 방향 값</param>
        private void SideJump(int dir)
        {
            if (!addForce) // 호출이 수백번 되기 때문에 실행 수 제한
            {
                addForce = true;
                sideJump = true; // 잡고 있는 물체를 놓게 한다

                float upJumpForce = 3.5f; // 위로 점프하는 힘
                float leftJumpForce = 1.5f; // 좌측으로 점프하는 힘

                // TODO: 왜 두번째 측면 점프는 제대로 작동하지 않는가?

                if (dir == 0)
                {
                    playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                    playerRigid.AddForce(Vector3.left * leftJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
                }
                else if (dir == 1)
                {
                    playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                    playerRigid.AddForce(Vector3.right * leftJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
                }

                Invoke("ClearSideJump", 0.3f);
                Invoke("ClearAddForce", 1f);
            }
        }

        /// <summary>
        /// 재작동을 위해 bool 값 초기화 
        /// </summary>
        private void ClearSideJump() { sideJump = false; }
        private void ClearAddForce() { addForce = false; }
        #endregion
    }
}
