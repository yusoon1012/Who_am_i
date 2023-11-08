using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어 UI 스크립트
/// </summary>
public class UI_Player : MonoBehaviour
{
    [SerializeField] Transform fullnessGage = default;
    [SerializeField] Transform pooGage = default;
    // 포만감 게이지 구성 수
    int fullnessCount = default;
    // 배출 게이지 구성 수 
    int pooCount = default;
    // 포만감 게이지 배열
    GameObject[] fullnessArray = default;
    // 배출 게이지 배열
    GameObject[] pooArray = default;

    ///<Point> m_fullness와 fullnessGage의 childCount는 나누어 떨어져야 한다.
    // m_fullness / fullnessGage 자식 = 25 / 5 = 5개, 20 / 5 = 4개, 15 / 5 = 3개, 10 / 5 = 2개, 5 / 5 = 1개, 0 / 5 = 0개 

    private void Start()
    {
        fullnessCount = fullnessGage.childCount;
        pooCount = pooGage.childCount;

        SetAray();
    }

    /// <summary>
    /// 게이지 배열 세팅
    /// </summary>
    private void SetAray()
    {
        fullnessArray = new GameObject[fullnessCount];
        for (int i = 0; i < fullnessCount; i++)
        {
            fullnessArray[i] = fullnessGage.GetChild(i).gameObject;
        }

        pooArray = new GameObject[pooCount];
        for (int i = 0; i < pooCount; i++)
        {
            pooArray[i] = pooGage.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// 포만감 게이지 업데이트
    /// </summary>
    private void FollowUp_Fullness()
    {
        int quotient = (int)(Player_Status.m_fullness / fullnessCount); // 몫 (남은 게이지 비례)
        float remainder = Player_Status.m_fullness % fullnessCount; // 나머지 (게이지 업데이트 시점)

        // TODO: 포만감, 배출 수치 UI에 반영 
    }

    /// <summary>
    /// 배출 게이지 업데이트
    /// </summary>
    private void FollowUp_Poo()
    {

    }

    private void Update()
    {
        FollowUp_Fullness();
        FollowUp_Poo();
    }

}
