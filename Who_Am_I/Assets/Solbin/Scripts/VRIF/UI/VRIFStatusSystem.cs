using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어의 정보를 관리한다. 
/// </summary>
/// 
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
    // 포만감 대분류 리스트 (FullImg, HalfImg)
    private GameObject[] fullnessArray;
    // 배출 대분류 리스트 (FullImg, HalfImg)
    private GameObject[] pooArray;
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

        fullnessArray = new GameObject[fullnessGage.childCount];
        pooArray = new GameObject[pooGage.childCount];

        for (int i = 0; i < gageCount; i++)
        {
            fullnessArray[i] = fullnessGage.GetChild(i).gameObject;
            pooArray[i] = pooGage.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// 소화 시작 (포만감 수치 하락)
    /// </summary>
    private IEnumerator Digestion()
    {
        while (digestion)
        {
            yield return new WaitForSeconds(60); // TODO: 1분에 5% 떨어지는 것으로 설정
            m_Fullness -= 5;

            if (m_Fullness <= 0)
            {
                digestion = false; // 소화기 정지
                DieEvent(); // 사망 이벤트
                break;
            }
        }
    }

    private void Update()
    {
        UIUpdate();
    }

    private void UIUpdate()
    {
        
    }

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
