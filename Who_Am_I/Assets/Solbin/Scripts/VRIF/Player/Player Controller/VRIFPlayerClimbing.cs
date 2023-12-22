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

        // 왼손 그랩 여부
        private bool leftGrab = false;
        // 오른손 그랩 여부
        private bool rightGrab = false;

        // 왼쪽 점프 시도
        private bool leftJump = false;
        // 오른쪽 점프 시도
        private bool rightJump = false;

        // 호출에 대한 AddForce 실행 수 제한 
        private bool addForce = false;

        [Header("플레이어")]
        [SerializeField] private Transform player = default;

        [Header("Player State.cs")]
        [SerializeField] private VRIFStateSystem vrifStateSystem = default;

        [Header("Grabbers")]
        [SerializeField] private Grabber leftGrabber = default;
        [SerializeField] private Grabber rightGrabber = default;

        [Header("점프값")]
        public float upJumpForce = 3.5f; // 위로 점프하는 힘
        public float sideJumpForce = 1.5f; // 측면으로 점프하는 힘

        [Header("Climbing Anchor")]
        [SerializeField] private Transform climbingAnchor = default;

        private void Start()
        {
            asset_playerClimbing = GetComponent<PlayerClimbing>();
            playerRigid = GetComponent<Rigidbody>(); // 점프에 사용할 Rigidbody

            asset_playerClimbing.leftClimbingEvent += LeftGrabCheck; // 왼손 그랩 확인
            asset_playerClimbing.rightClimbingEvent += RightGrabCheck; // 오른손 그랩 확인

            VRIFInputSystem.Instance.lClimbingJump += ClickLeftJump; // 왼손 점프 클릭 확인
            VRIFInputSystem.Instance.rClimbingJump += ClickRightJump; // 오른손 점프 클릭 확인
        }

        #region 그랩 체크
        private void LeftGrabCheck(object sender, EventArgs e) { leftGrab = true; }

        private void RightGrabCheck(object sender, EventArgs e) { rightGrab = true; }

        private void Update()
        {
            if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.CLIMBING) { SuperJump(); SideJump(); }

            if (Input.GetKeyDown(KeyCode.K)) // TODO: 밑의 코드가 작동하는 것을 봐선 그랩과 상관이 있다.
            {
                playerRigid.AddForce(Vector3.up * 5f, ForceMode.Impulse); // 상승 점프 테스트
            }
        }
        #endregion

        #region 상승 점프
        private void SuperJump()
        {
            if (leftGrab && rightGrab) // 양손 다 그랩 상태일때
            {
                float jump = -0.4f; // 상승 점프 조건 

                if (VRIFInputSystem.Instance.lVelocity.y <= jump && VRIFInputSystem.Instance.rVelocity.y <= jump) // 아래로 휘두르기 
                {
                    leftGrabber.ReleaseGrab(); // 양쪽 손을 놓게 한다. 
                    rightGrabber.ReleaseGrab();

                    float jumpForce = 5f;
                    playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 상승 점프
                }
            }

            leftGrab = false;
            rightGrab = false;
        }
        #endregion

        #region 측면 점프

        // 왼쪽 점프 버튼 클릭 이벤트를 받아온다
        private void ClickLeftJump(object sender, EventArgs e) { leftJump = true; }
        // 오른쪽 점프 버튼 클릭 이벤트를 받아온다. 
        private void ClickRightJump(object sender, EventArgs e) { rightJump = true; }

        /// <summary>
        /// 측면 점프
        /// </summary>
        private void SideJump()
        {
            if (leftJump || rightJump)
            {
                leftGrabber.ReleaseGrab(); // 양쪽 손을 놓게 한다. 
                rightGrabber.ReleaseGrab();

                if (leftJump && !addForce)
                {
                    addForce = true;

                    playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                    playerRigid.AddForce(-climbingAnchor.right * sideJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 

                    Invoke("ClearAddForce", 0.5f);
                }
                else if (rightJump && !addForce) // 호출이 수백번 되기 때문에 실행 수 제한
                {
                    addForce = true;

                    playerRigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
                    playerRigid.AddForce(climbingAnchor.right * sideJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 

                    Invoke("ClearAddForce", 0.5f);
                }
            }

            leftJump = false;
            rightJump = false;
        }

        /// <summary>
        /// 재작동을 위해 bool 값 초기화 
        /// </summary>
        private void ClearAddForce() { addForce = false; }
        #endregion
    }
}
