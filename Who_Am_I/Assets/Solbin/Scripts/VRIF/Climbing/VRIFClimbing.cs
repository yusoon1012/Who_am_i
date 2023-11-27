using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class VRIFClimbing : MonoBehaviour
    {
        // (Asset) PlayerClimbing
        [SerializeField] private PlayerClimbing asset_playerClimbing;
        // Player Rigidbody 
        private Rigidbody playerRigid;
        // VRIF Action
        VRIFAction vrifAction;

        private void Start()
        {
            Setting();
        }

        #region 초기 세팅
        private void Setting()
        {
            asset_playerClimbing.climbingEvent += ActivateSideJump; // 등반 중 이벤트 구독
            playerRigid = GetComponent<Rigidbody>(); // 점프에 사용할 Rigidbody
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

        /// <summary>
        /// 측면 점프 - 한 손 이상 그랩 상태일때 활성화
        /// </summary>
        private void ActivateSideJump(object sender, EventArgs e)
        {
            Debug.LogWarning("Grab Check");
            // TODO: 매핑 후 좌측, 우측 점프 연결하기 
        }

        /// <summary>
        /// 측면 점프 - 좌측
        /// </summary>
        private void LeftJump()
        {

        }

        /// <summary>
        /// 측면 점프 - 우측
        /// </summary>
        private void RightJump()
        {

        }

        #region 상승 점프
        /// <summary>
        /// 상승 점프 - 양 손 전부 그랩 상태일때 활성화
        /// </summary>
        private void ActivateSuperJump(object sender, EventArgs e)
        {
            float jump = -1f; // 상승 점프 조건 

            // 두 손을 아래로 휘두른다면
            if (VRIFControllerSystem.lVelocity.y <= jump && VRIFControllerSystem.rVelocity.y <= jump) { SuperJump(); }
        }

        /// <summary>
        /// 상승 점프
        /// </summary>
        private void SuperJump()
        {
            float jumpForce = 6f; // 점프력

            Debug.LogWarning("Jump!");

            playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        #endregion
    }
}
