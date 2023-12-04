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
    [Header("Gage Transform")]
    [SerializeField] Transform fullnessGage = default;
    [SerializeField] Transform pooGage = default;
    
    [Header("GameObject Poo")]
    [Tooltip("똥")]
    [SerializeField] GameObject poo = default; // 프리팹
    [SerializeField] GameObject playerPoo = default;

    // 게이지 총 수는 5로 정해졌다.
    private int gageCount = 5;
    // 현 포만감, 배출도 수치 
    private int m_Fullness = default;
    private int m_Poo = default;
    // 소화기 작동 여부
    private bool digestion = true;
    // 포만감 게이지 배열
    private GameObject[] halfFullnessArray;
    private GameObject[] fullFullnessArray;
    // 배출도 게이지 배열 
    private GameObject[] halfPooArray;
    private GameObject[] fullPooArray;
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

        Transform halfFullness = fullnessGage.GetChild(0); // 반개짜리 배열의 부모 오브젝트
        Transform fullFullness = fullnessGage.GetChild(1); // 한개짜리 배열의 부모 오브젝트
        Transform halfPoo = pooGage.GetChild(0);
        Transform fullPoo = pooGage.GetChild(1);

        halfFullnessArray = new GameObject[halfFullness.childCount]; // 반개짜리 배열
        fullFullnessArray = new GameObject[fullFullness.childCount]; // 한개짜리 배열
        halfPooArray = new GameObject[halfPoo.childCount];
        fullPooArray = new GameObject[fullPoo.childCount];

        for (int i = 0; i < gageCount; i++)
        {
            halfFullnessArray[i] = halfFullness.GetChild(i).gameObject; // 반개짜리 배열 할당
            fullFullnessArray[i] = fullFullness.GetChild(i).gameObject; // 한개짜리 배열 할당

            halfPooArray[i] = halfPoo.GetChild(i).gameObject; // 반개짜리 배열 할당
            fullPooArray[i] = fullPoo.GetChild(i).gameObject; // 한개짜리 배열 할당 
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
        PooCheck();

        if (Input.GetKeyDown(KeyCode.G))
        {
            GetFood(15, 20);
        }
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

    #region 배출도 게이지 업데이트
    private void PooCheck()
    {
        if (1 <= m_Poo && m_Poo <= 20)
        {
            if (m_Poo <= 10) { PooUpdate(1, "half"); }
            else { PooUpdate(1, "full"); }
        }
        else if (21 <= m_Poo && m_Poo <= 40)
        {
            if (m_Poo <= 30) { PooUpdate(2, "half"); }
            else { PooUpdate(2, "full"); }
        }
        else if (41 <= m_Poo && m_Poo <= 60)
        {
            if (m_Poo <= 50) { PooUpdate(3, "half"); }
            else { PooUpdate(3, "full"); }
        }
        else if (61 <= m_Poo && m_Poo <= 80)
        {
            if (m_Poo <= 70) { PooUpdate(4, "half"); }
            else { PooUpdate(4, "full"); }
        }
        else if (81 <= m_Poo && m_Poo <= 100)
        {
            if (m_Fullness <= 90) { PooUpdate(5, "half"); }
            else { PooUpdate(5, "full"); }
        }

        if (m_Poo >= 100) { PooEvent(); } // 배출도 초과시 배출 이벤트 발생 
    }

    private void PooUpdate(int _num, string _percent)
    {
        if (_percent == "half")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i <= _num - 1) { halfPooArray[i].SetActive(true); }
                else { halfPooArray[i].SetActive(false); }
            }
        }
        else if (_percent == "full")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i <= _num - 1) { fullPooArray[i].SetActive(true); }
                else { fullPooArray[i].SetActive(false); }
            }
        }
    }
    #endregion

    /// <summary>
    /// 음식 섭취 메소드
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
        Vector3 pooPos = new Vector3();
        m_Poo = 0; // 배출값 초기화 

        playerPoo = Instantiate(poo); // 생성 
        // TODO: 배출 이벤트 구현 
    }   
    
    /// <summary>
    /// 사망 이벤트
    /// </summary>
    private void DieEvent()
    {
       
    }
}
