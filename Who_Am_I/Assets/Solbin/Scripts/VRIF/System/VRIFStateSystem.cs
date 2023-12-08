using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class VRIFStateSystem : MonoBehaviour
    {
        public enum GameState
        {
            NORMAL,
            CLIMBING,
            LADDER,
            UI,
            POO
        }

        // 현재 게임 상태 
        public static GameState gameState = default;

        [Header("플레이어 트랜스폼")]
        [SerializeField] private Transform player = default;

        // 플레이어 움직임 컴포넌트 (아래 둘 스크립트가 쌍으로 작동)
        private LocomotionManager locomotionManager = default;
        private SmoothLocomotion smoothLocomotion = default;
        // 플레이어 등반 컴포넌트 (아래 둘 스크립트가 쌍으로 작동)
        private PlayerClimbing playerClimbing = default;
        private VRIFPlayerClimbing vrifPlayerClimbing = default;
        // 플레이어 회전 컴포넌트 
        private PlayerRotation playerRotation;

        [Header("UI Controller")]
        // UI Controller (UI 입력 막기)
        [SerializeField] private UIController uiController = default;
        // 퀵슬롯
        [SerializeField] private QuickSlot quickSlot = default;

        [Header("양쪽 컨트롤러")]
        // 왼쪽 컨트롤러
        [SerializeField] private Transform leftController;
        // 오른쪽 컨트롤러
        [SerializeField] private Transform rightController;

        // 플레이어의 현 스테이터스 (포만감, 배출도)
        private VRIFStatusSystem vrifStatusSystem = default;

        private void Start()
        {
            gameState = GameState.NORMAL; // 기본 상태로 초기화 

            locomotionManager = player.GetComponent<LocomotionManager>();
            smoothLocomotion = player.GetComponent<SmoothLocomotion>();

            playerClimbing = player.GetComponent<PlayerClimbing>();
            vrifPlayerClimbing = player.GetComponent<VRIFPlayerClimbing>();

            playerRotation = player.GetComponent<PlayerRotation>();

            vrifStatusSystem = transform.GetComponent<VRIFStatusSystem>();
        }

        private void Update()
        {
            ChangeState();
        }

        /// <summary>
        /// 게임 상태 변경 메소드
        /// </summary>
        private void ChangeState()
        {
            switch (gameState)
            {
                case GameState.NORMAL: // 일반 상태
                    NormalState();
                    break;

                case GameState.CLIMBING: // 등반 상태 
                    ClimbingState();
                    break;

                case GameState.UI: // UI 상태 
                    UIState();
                    break;

                case GameState.POO: // POO 상태 (배출 상태)
                    PooState();
                    break;
            }
        }

        #region 상태 변경
        /// <Point> PlayerController의 Locomotion Manager.cs가 플레이어의 이동을 담당한다

        /// <summary>
        /// 기본 상태
        /// </summary>
        private void NormalState()
        {
            locomotionManager.enabled = true; // 이동 활성화
            smoothLocomotion.enabled = true;

            playerRotation.enabled = true; // 회전 활성화

            foreach (Transform child in leftController) { child.gameObject.SetActive(true); } // 왼손 활성화

            foreach (Transform child in rightController) { child.gameObject.SetActive(true); } // 오른손 활성화

            vrifStatusSystem.digestion = true; // 소화기 활성화

            vrifStatusSystem.hungerTimer = vrifStatusSystem.hungerTimer_Origin; // 본래 지정된 값으로 회귀

            quickSlot.enabled = true;
            uiController.enabled = true;
        }

        /// <summary>
        /// UI 상태 
        /// </summary>
        private void UIState()
        {
            if (locomotionManager.enabled) // 이동 활성화 상태면
            {
                locomotionManager.enabled = false; // 이동 비활성화
                smoothLocomotion.enabled = false;
            }

            if (playerRotation.enabled) // 회전 활성화 상태면
            {
                playerRotation.enabled = false; // 회전 비활성화
            }

            foreach (Transform child in leftController) { child.gameObject.SetActive(false); } // 왼손 비활성화

            foreach (Transform child in rightController) { child.gameObject.SetActive(false); } // 오른손 비활성화 

            vrifStatusSystem.digestion = false; // 소화기 잠시 정지 
        }

        /// <summary>
        /// 등반 상태 
        /// </summary>
        private void ClimbingState()
        {
            vrifStatusSystem.hungerTimer = 30;

            quickSlot.enabled = false;
            uiController.enabled = false;
        }

        /// <summary>
        /// POO 상태 (배출 상태)
        /// </summary>
        private void PooState()
        {
            if (locomotionManager.enabled) // 이동 활성화 상태면
            {
                locomotionManager.enabled = false; // 이동 비활성화
                smoothLocomotion.enabled = false;
            }
        }
        #endregion
    }
}
