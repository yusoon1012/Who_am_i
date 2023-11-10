using Oculus.Interaction;
using OVRSimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 플레이어 스탯 설정, 관리
/// </summary>

[Serializable]
public struct PlayerStat
{
    public float fullness; // 포만감 최대치
    public float poo; // 배출 최대치
    public float speed; // 플레이어 이동 스피드
    public float teleportDistance; // 텔레포트 가능 거리(레이저 거리)
    public float jumpForce; // 점프력
    public float hungerTime; // 배고픔 타이머
    public float getHunger; // 배고픔 수치
}

public class Player_Status : MonoBehaviour
{
    #region 필드
    public static PlayerStat playerStat;

    public static float m_fullness = default; // 현재 포만감
    public static float m_poo = default; // 현재 배출도

    public static float m_speed = default;
    public static float m_teleportDistance = default;
    public static float m_jumpForce = default;

    private bool metabolism = false; // 소화기 활성화

    public static event EventHandler playerPoo; // 배출 이벤트
    public static event EventHandler playerDefeat; // 플레이어 사망 이벤트
    #endregion

    private void Awake()
    {
        var playerStatJson = Resources.Load<TextAsset>("Json/PlayerStat");
        playerStat = JsonUtility.FromJson<PlayerStat>(playerStatJson.ToString());

        // 초기 스탯 설정
        SetStartStat();
        // 소화 시작
        StartCoroutine(GetHunger());
    }

    /// <summary>
    /// static 필드 초기화
    /// </summary>
    private void SetStartStat() 
    {
        m_fullness = playerStat.fullness; // 포만도 가득 찬 상태로 시작
        m_poo = 0; // 배출 0부터 시작
        m_speed = playerStat.speed; // 이동 속도
        m_jumpForce = playerStat.jumpForce; // 점프력
    }


    #region 배출, 포만감
    /// <summary>
    /// 시간이 지남에 따라 포만감을 깎는다
    /// </summary>
    private IEnumerator GetHunger()
    {
        metabolism = true;

        float _getHunger = playerStat.getHunger; // 배고픔 타이머
        float _hungerTime = playerStat.hungerTime; // _getHunger초당 포만감 하락

        while(metabolism)
        {
            m_fullness -= _getHunger;
            yield return new WaitForSeconds(_hungerTime);
        }
    } 

    /// <summary>
    /// 특정 이벤트 발생시 포만감 하락
    /// </summary>
    /// <param name="getHunger">얻는 배고픔 수치</param>
    public void HungerEvent(float getHunger) { m_fullness -= getHunger; }

    /// <summary>
    /// 음식을 얻으면 포만감, 배출 수치가 높아진다
    /// </summary>
    /// <param name="getFull">얻는 포만감 수치</param>
    /// <param name="getPoo">얻는 배출 수치</param>
    public void GetFood(float getFull, float getPoo) { m_fullness += getFull; m_poo += getPoo; }

    /// <summary>
    /// 플레이어의 현재 상태를 체크
    /// </summary>
    private void StatCheck()
    {
        if (m_fullness <= 0) // 포만감 0 이하
        {
            playerDefeat?.Invoke(this, EventArgs.Empty); // 플레이어 쓰러짐 이벤트
            metabolism = false;
        }
        else if (m_fullness >= playerStat.fullness) // 포만감 한계치보다 많이 섭취
        {
            m_fullness = playerStat.fullness; // 포만감 수치 고정
        }

        if (m_poo >= playerStat.poo) // 배출 한계치보다 많이 섭취
        {
            playerPoo?.Invoke(this, EventArgs.Empty); // 배출 이벤트
        }
    }
    #endregion

    private void Update()
    {
        StatCheck();
    }
}
