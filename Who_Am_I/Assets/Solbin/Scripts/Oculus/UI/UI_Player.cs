using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어 UI 스크립트
/// </summary>
public class UI_Player : MonoBehaviour
{
    #region 필드
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
    #endregion

    private void Start()
    {
        fullnessCount = fullnessGage.childCount;
        //pooCount = pooGage.childCount;

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

    #region 게이지 업데이트
    /// <summary>
    /// 포만감 게이지 업데이트
    /// </summary>
    private void FollowUp_Fullness()
    {
        float maxFullness = Player_Status.playerStat.fullness; // 매직넘버

        for (int i = 0; i < fullnessCount; i++)
        {
            if (i < Player_Status.m_fullness / ( maxFullness / fullnessCount))
            {
                fullnessArray[i].SetActive(true);
            }
            else
            {
                fullnessArray[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 배출 게이지 업데이트
    /// </summary>
    private void FollowUp_Poo()
    {
        float maxPoo = Player_Status.playerStat.poo; // 매직넘버

        for (int i = 0; i < pooCount; i++)
        {
            if (i < Player_Status.m_poo / (maxPoo / pooCount))
            {
                pooArray[i].SetActive(true);
            }
            else
            {
                pooArray[i].SetActive(false);
            }
        }
    }
    #endregion

    private void Update()
    {
        FollowUp_Fullness();
        FollowUp_Poo();
    }

}
