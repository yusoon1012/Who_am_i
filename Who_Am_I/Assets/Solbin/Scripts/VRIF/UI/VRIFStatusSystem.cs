using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어의 정보를 관리한다. 
/// </summary>

public class VRIFStatusSystem : MonoBehaviour
{
    #region 필드
    [SerializeField] Transform fullnessGage = default;
    [SerializeField] Transform pooGage = default;

    // 게이지 총 수는 5로 정해졌다.
    private int gageCount = 5;

    private int m_Fullness = default;
    private int m_Poo = default;

    // 소화기 작동 여부
    private bool digestion = true;

    private GameObject[] halfFullnessArray;
    private GameObject[] fullFullnessArray;
    #endregion

    private void Start()
    {
        Setting();

        StartCoroutine(Digestion());
    }

    private void Setting()
    {
        m_Fullness = 100; // 포만감 초기값
        m_Poo = 0; // 배출 초기값

        Transform halfFullness = fullnessGage.GetChild(0);
        Transform fullFullness = fullnessGage.GetChild(1);

        halfFullnessArray = new GameObject[halfFullness.childCount];
        fullFullnessArray = new GameObject[fullFullness.childCount];

        for (int i = 0; i < gageCount; i++)
        {
            halfFullnessArray[i] = halfFullness.GetChild(i).gameObject; // 반개짜리 배열
            fullFullnessArray[i] = fullFullness.GetChild(i).gameObject; // 한개짜리 배열
        }
    }

    /// <summary>
    /// 소화 시작 (포만감 수치 하락)
    /// </summary>
    private IEnumerator Digestion()
    {
        while (digestion)
        {
            yield return new WaitForSeconds(3); // TODO: 1분에 5% 떨어지는 것으로 설정
            m_Fullness -= 5;

            if (m_Fullness <= 0) // 사망 조건 
            {
                digestion = false; // 소화기 정지
                DieEvent(); // 사망 이벤트
                break;
            }
        }
    }

    private void Update()
    {
        FullnessCheck();
    }

    #region 포만감 게이지 업데이트
    private void FullnessCheck()
    {
        if (1 <= m_Fullness && m_Fullness <= 20)
        {
            if (m_Fullness <= 10) { FullnessUpdate(1, "half"); }
            else { FullnessUpdate(1, "full"); }
        }
        else if (21 <= m_Fullness && m_Fullness <= 40)
        {
            if (m_Fullness <= 30) { FullnessUpdate(2, "half"); }
            else { FullnessUpdate(2, "full"); }
        }
        else if (41 <= m_Fullness && m_Fullness <= 60)
        {
            if (m_Fullness <= 50) { FullnessUpdate(3, "half"); }
            else { FullnessUpdate(3, "full"); }
        }
        else if (61 <= m_Fullness && m_Fullness <= 80)
        {
            if (m_Fullness <= 70) { FullnessUpdate(4, "half"); }
            else { FullnessUpdate(4, "full"); }
        }
        else if (81 <= m_Fullness && m_Fullness <= 100)
        {
            if (m_Fullness <= 90) { FullnessUpdate(5, "half"); }
            else { FullnessUpdate(5, "full"); }
        }

        if (m_Fullness >= 100) { m_Fullness = 100; } // 포만감 초과시 100으로 보정
    }

    private void FullnessUpdate(int _num, string _percent)
    {
        if (_percent == "half")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i <= _num - 1) { halfFullnessArray[i].SetActive(true); }
                else { halfFullnessArray[i].SetActive(false); }
            }
        }
        else if (_percent == "full")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i <= _num - 1) { fullFullnessArray[i].SetActive(true); }
                else { fullFullnessArray[i].SetActive(false); }
            }
        }
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_satiety">얻는 포만도</param>
    /// <param name="_poo">얻는 배출도</param>
    public void GetFood(int _satiety, int _poo)
    {
        m_Fullness += _satiety; // 포만감 더하기 
        m_Poo += _poo; // 배출값 더하기 

        if (m_Poo >= 100) { PooEvent(); }
    }

    /// <summary>
    /// 배출 이벤트
    /// </summary>
    private void PooEvent()
    {
        m_Poo = 0; // 배출값 초기화 
        // TODO: 배출 이벤트 구현 
    }   
    
    /// <summary>
    /// 사망 이벤트
    /// </summary>
    private void DieEvent()
    {
       
    }

    // 20 40 60 80 100
}
