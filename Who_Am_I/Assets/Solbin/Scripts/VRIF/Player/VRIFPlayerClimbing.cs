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

        [Header("플레이어")]
        [SerializeField] private Transform player = default;

        [Header("Player State.cs")]
        [SerializeField] private VRIFStateSystem vrifStateSystem = default;

        [Header("점프값")]
        public float upJumpForce = 3.5f; // 위로 점프하는 힘
        public float sideJumpForce = 1.5f; // 측면으로 점프하는 힘

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
            if (VRIFStateSystem.gameState == VRIFStateSystem.GameState.CLIMBING) { SuperJump(); }

            Debug.LogWarning("Before=" + playerRigid.velocity); // TODO: 밑 After 로그와 비교해 언제 velocity가 0이 되는지
            // 확인할 것 요망 
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

                if (vrifAction.Player.LeftGrip.ReadValue<float>() <= 0.3f &&
                    vrifAction.Player.RightGrip.ReadValue<float>() <= 0.3f) // 손을 놓았다면 (등반 중이 아니라면)
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
            if (VRIFStateSystem.gameState != VRIFStateSystem.GameState.NORMAL && !addForce) // 호출이 수백번 되기 때문에 실행 수 제한
            {
                addForce = true;
                sideJump = true; // 잡고 있는 물체를 놓게 한다

                // TODO: 왜 두번째 측면 점프는 제대로 작동하지 않는가?
                // 두 번째 점프 시 중력도 제대로 적용되고, velocity에 힘도 주어지나 점프가 이루어지지 않고 추락한다.
                // 중력값은 동일한 것을 확인함. 
                // 1. 혹시 Rigidbody의 use gravity가 활성화 되는가?

                if (dir == 0)
                {
                    playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                    playerRigid.AddForce(Vector3.left * sideJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
                }
                else if (dir == 1)
                {
                    playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                    playerRigid.AddForce(Vector3.right * sideJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
                }

                Debug.LogWarning("After=" + playerRigid.velocity);

                Invoke("ClearSideJump", 0.3f);
                Invoke("ClearAddForce", 0.5f);
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
