using BNG;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어의 정보를 관리한다. 
/// </summary>

public class VRIFStatusSystem : MonoBehaviour
{
    #region 필드
    [Header("게이지 트랜스폼")]
    [SerializeField] Transform fullnessGage = default;
    [SerializeField] Transform pooGage = default;

    [Header("똥")]
    [SerializeField] GameObject poo = default; // 프리팹
    private GameObject playerPoo = default;

    [Header("플레이어 트랜스폼")]
    [SerializeField] private Transform player = default; // 플레이어 트랜스폼 

    [Header("포만감 하락 타이머")]
    public int hungerTimer = 60; // 소화기 타이머
    public int hungerTimer_Origin { get; private set; } // 타이머의 원래 값

    [Header("시간당 수치 조절값")]
    [SerializeField] private int getHunger = 5; // 분당 떨어지는 포만감
    [SerializeField] private int getPoo = 1; // 분당 얻는 배출값

    [Header("손")]
    [SerializeField] private Grabber leftGrabber = default; // 왼쪽 손 
    [SerializeField] private Grabber rightGrabber = default; // 오른쪽 손 

    [Header("플레이어 사망")]
    [SerializeField] private GameObject dieCanvas = default;

    // 게이지 총 수는 5로 정해졌다. TODO: 후에 수정 필요 
    private int gageCount = 5;
    // 현 포만감, 배출도 수치 
    public int m_Fullness = default;
    public int m_Poo = default;
    // 소화기 작동 여부
    public bool digestion = true;
    // 포만감 게이지 배열
    private GameObject[] halfFullnessArray;
    private GameObject[] fullFullnessArray;
    // 배출도 게이지 배열 
    private GameObject[] halfPooArray;
    private GameObject[] fullPooArray;
    #endregion

    private void Start()
    {
        Setting(); // 초기 세팅

        FullnessCheck(); // 게이지 체크
        PooCheck();

        StartCoroutine(Digestion()); // 소화기 작동 시작
    }

    private void Setting()
    {
        m_Fullness = 100; // 포만감 초기값
        m_Poo = 0; // 배출 초기값

        hungerTimer_Origin = hungerTimer; // 본래 타이머 값 저장

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
            yield return new WaitForSeconds(hungerTimer);
            m_Fullness -= getHunger;
            m_Poo += getPoo;

            FullnessCheck();
            PooCheck();

            if (m_Fullness <= 0) // 사망 조건 
            {
                digestion = false; // 소화기 정지
                DieEvent(); // 사망 이벤트
                break;
            }
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
        else if (81 <= m_Fullness)
        {
            if (m_Fullness <= 90) { FullnessUpdate(5, "half"); }
            else { FullnessUpdate(5, "full"); }
        }

        if (m_Fullness >= 100) { m_Fullness = 100; } // 포만감 초과시 100으로 보정
    }

    private void FullnessUpdate(int _num, string _percent) // 48이면 4, half
    {
        if (_percent == "half")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i < _num) { halfFullnessArray[i].SetActive(true); }
                else { halfFullnessArray[i].SetActive(false); }

                if (i >= _num - 1) { fullFullnessArray[i].SetActive(false); } // Full 비활성화.
                else { fullFullnessArray[i].SetActive(true); }
            }
        }
        else if (_percent == "full")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i < _num) { fullFullnessArray[i].SetActive(true); } // 0, 1을 활성화
                else { fullFullnessArray[i].SetActive(false); }
            }
        }
    }
    #endregion

    #region 배출도 게이지 업데이트
    
    /// <summary>
    /// 똥 쌈
    /// </summary>
    public void GetPoo() { Debug.Log("플레이어는 용변을 해결했다."); m_Poo = 0; PooCheck(); }

    private void PooCheck()
    {
        if (m_Poo <= 10)
        {
            PooUpdate(0, "zero");
        }
        else if (11 <= m_Poo && m_Poo <= 30)
        {
            if (m_Poo <= 20) { PooUpdate(1, "half"); } 
            else { PooUpdate(1, "full"); } 
        }
        else if (31 <= m_Poo && m_Poo <= 50)
        {
            if (m_Poo <= 40) { PooUpdate(2, "half"); } 
            else { PooUpdate(2, "full"); }
        }
        else if (51 <= m_Poo && m_Poo <= 70)
        {
            if (m_Poo <= 60) { PooUpdate(3, "half"); }
            else { PooUpdate(3, "full"); } 
        }
        else if (71 <= m_Poo && m_Poo <= 90)
        {
            if (m_Poo <= 80) { PooUpdate(4, "half"); }
            else { PooUpdate(4, "full"); }
        }
        else if (91 <= m_Poo && m_Poo < 100)
        {
            PooUpdate(5, "half");
        }

        if (m_Poo >= 100) { PooUpdate(5, "full"); Invoke("PooEvent", 3); } // 5개, 배출도 초과시 배출 이벤트 발생 
    }

    private void PooUpdate(int _num, string _percent)
    {
        if (_percent == "zero")
        {
            for (int i = 0; i < gageCount; i++)
            {
                halfPooArray[i].SetActive(false);
                fullPooArray[i].SetActive(false);
            }
        }
        else if (_percent == "half")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i < _num) { halfPooArray[i].SetActive(true); }
                else { halfPooArray[i].SetActive(false); }

                if (i < _num - 1) { fullPooArray[i].SetActive(true); }
                else { fullPooArray[i].SetActive(false); }
            }
        }
        else if (_percent == "full")
        {
            for (int i = 0; i < gageCount; i++)
            {
                if (i < _num) { fullPooArray[i].SetActive(true); }
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

        FullnessCheck();
        PooCheck();
    }

    /// <summary>
    /// 배출 이벤트
    /// </summary>
    private void PooEvent()
    {
        Vector3 pooPos = -player.forward * 0.2f; // 플레이어의 2만큼 뒤 
        Quaternion playerRotation = Quaternion.Euler(player.eulerAngles); // 플레이어의 Euler

        if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.CLIMBING) // 등반 중이었다면 
        {
            // TODO: UI 출력이 필요하다. 

            // 손을 놓게 한다. 
            leftGrabber.ReleaseGrab();
            rightGrabber.ReleaseGrab();

            leftGrabber.enabled = false;
            rightGrabber.enabled = false;

            Invoke("ClearGrabbers", 3); // N초 후 다시 그랩이 가능하도록 한다. 
        }

        m_Poo = 0; // 배출값 초기화 

        Instantiate(poo, pooPos, playerRotation);

        // TODO: 후에 NPC에 영향이 가도록 구현, 사운드 출력 
    }

    /// <summary>
    /// 등반 중 배출 이벤트로 인해 손을 놓게 되었을 때 Grabber를 다시 ON
    /// </summary>
    private void ClearGrabbers() { leftGrabber.enabled = true; rightGrabber.enabled = true; }
    
    /// <summary>
    /// 사망 이벤트
    /// </summary>
    private void DieEvent()
    {
        dieCanvas.SetActive(true);
    }
}
