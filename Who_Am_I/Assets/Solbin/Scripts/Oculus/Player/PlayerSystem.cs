using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Meta.WitAi;

/// <summary>
/// 플레이어 시스템
/// </summary>
public class PlayerSystem : MonoBehaviour
{
    #region 필드
    // OVRCameraRig
    //[SerializeField] private Transform player = default;
    // 오른손
    [SerializeField] private GameObject rHand = default;
    // 우측 컨트롤러
    [SerializeField] private GameObject rController = default;
    // 오른손 활성화 여부
    public bool handActivate = false;
    // 전체 풀포지션
    public static Vector3 poolPos = new Vector3(0, -10, 0);
    #endregion

    private void Start()
    {
        ManageEvent(); // 이벤트 구독
    }

    #region 초기 세팅
    /// <summary>
    /// (초기 세팅)각 스크립트에서 이벤트를 받아 기능 구현
    /// </summary>
    private void ManageEvent()
    {
        // 플레이어 쓰러짐 이벤트
        Player_Status.playerDefeat += PlayerDefeat;
        // 플레이어 배출 이벤트
        Player_Status.playerPoo += PlayerPoo;
    }
    #endregion

    private void Update()
    {
        if (Player_State.playerState == Player_State.PlayerState.IDLE) // 평시에만 컨트롤러 교체 가능
        {
            ChangeController();
            CheckController();
        }
    }

    /// <summary>
    /// (구현)컨트롤러 형식 교체
    /// </summary>
    private void ChangeController()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two)) // 우측 컨트롤러 B버튼 클릭
        {
            if (rHand.activeSelf) // 손이 활성화되어 있을 때
            {
                rHand.SetActive(false);
                rController.SetActive(true);
            }
            else if (!rHand.activeSelf)// 손이 비활성화 되어 있을 때 
            {
                rHand.SetActive(true);
                rController.SetActive(false);
            }
        }
    }

    /// <summary>
    /// (구현)컨트롤러 모델 체크
    /// </summary>
    private void CheckController()
    {
        if (rHand.activeSelf) { handActivate = true; }
        else if (!rHand.activeSelf) { handActivate = false; }
    }

    /// <summary>
    /// (구현)플레이어 배출
    /// </summary>
    private void PlayerPoo(object sender, EventArgs e)
    {
        // TODO: 플레이어 배출 이벤트 구현하기
        Debug.Log("플레이어가 배출!");
    }

    /// <summary>
    /// (구현)플레이어 사망 
    /// </summary>
    private void PlayerDefeat(object sender, EventArgs e)
    {
        // TODO: 플레이어 사망 이벤트 구현하기
        Debug.Log("플레이어 사망!");
    }
}
