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
            UI,
            POO
        }

        // 현재 게임 상태 
        public GameState gameState { get; private set; }

        [Header("Need Restrict")]
        // 플레이어 움직임 컴포넌트 (아래 둘 스크립트가 쌍으로 작동)
        [SerializeField] private LocomotionManager locomotionManager;
        [SerializeField] private SmoothLocomotion smoothLocomotion;
        // 플레이어 회전 컴포넌트 
        [SerializeField] private PlayerRotation playerRotation;
        // 왼쪽 컨트롤러
        [SerializeField] private Transform leftController;
        // 오른쪽 컨트롤러
        [SerializeField] private Transform rightController;

        private void Start()
        {
            gameState = GameState.NORMAL; // 기본 상태로 초기화 
        }

        /// <summary>
        /// 게임 상태 변경 메소드
        /// </summary>
        public void ChangeState(GameState _gameState)
        {
            gameState = _gameState;

            switch (gameState)
            {
                case GameState.NORMAL: // 일반 상태
                    NormalState();
                    break;

                case GameState.CLIMBING: // 등반 상태 
                    // TODO: 등반 상태 추가 
                    break;

                case GameState.UI: // UI 상태 
                    UIState();
                    break;

                case GameState.POO: // POO 상태 (배출 상태)
                    PooState();
                    break;
            }
        }

        /// <Point> PlayerController의 Locomotion Manager.cs가 플레이어의 이동을 담당한다

        /// <summary>
        /// 기본 상태
        /// </summary>
        private void NormalState()
        {
            if (!locomotionManager.enabled) // 이동 비활성화 상태면
            {
                locomotionManager.enabled = true; // 이동 활성화
                smoothLocomotion.enabled = true;
            }

            foreach (Transform child in leftController) { child.gameObject.SetActive(true); } // 왼손 활성화

            foreach (Transform child in rightController) { child.gameObject.SetActive(true); } // 오른손 활성화
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

            foreach (Transform child in leftController) { child.gameObject.SetActive(false); } // 왼손 비활성화

            foreach (Transform child in rightController) { child.gameObject.SetActive(false); } // 오른손 비활성화 
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
    }
}
