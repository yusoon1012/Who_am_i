using BNG;
using Febucci.UI.Core.Parsing;
using Oculus.Interaction.DistanceReticles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VRIFSceneManager : MonoBehaviour
{
    public static VRIFSceneManager Instance;

    [Header("XR Rig Advanced")]
    [SerializeField] private GameObject xrRigPlayer = default;

    [Header("Player Controller")]
    [SerializeField] private Transform playerController = default;

    [Header("Loading Canvas")]
    [Tooltip("로딩창(UI)")]
    [SerializeField] private GameObject loadingCanvas = default;

    [Header("Character Controller")]
    [Tooltip("CC가 position값을 덮어쓰기 때문에 비활성화 필요")]
    [SerializeField] private CharacterController characterController = default;

    // 플레이어 이동/회전 컴포넌트
    private LocomotionManager locomotionManager = default;
    private SmoothLocomotion smoothLocomotion = default;
    private PlayerRotation playerRotation = default;

    // 잠겨 있는 문을 여는 이벤트
    public event EventHandler openDoorEvent;
    // 메인씬 오픈을 위한 임시 Operation
    private AsyncOperation tempOperation = default;

    [Header("오디오")]
    public AudioClip mapTeleportClip = default;
    private AudioSource audioSource = default;

    public class SeasonName
    {
        public static string spring { get; private set; } = "Spring";
        public static string summer { get; private set; } = "Summer";
        public static string fall { get; private set; } = "Demo_ExteriorOnly_Optimized";
        public static string winter { get; private set; } = "Winter";
        public static string passage { get; private set; } = "M_Passage_Scene"; // 로딩씬
    }

    protected void Awake()
    {
        DontDestroyOnLoad(xrRigPlayer); // 플레이어 파괴 금지 

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        locomotionManager = playerController.GetComponent<LocomotionManager>();
        smoothLocomotion = playerController.GetComponent<SmoothLocomotion>();
        playerRotation = playerController.GetComponent<PlayerRotation>();

        // <Meen Change>
        FirstSetting();

        /// <Point> SceneManager를 통한 씬 오픈이라면 일단 작동한다. 
        SceneManager.sceneLoaded += PlayerSetting; // 씬 전환이 완벽히 이뤄지면 해당 이벤트가 발생한다. 
    }

    // <Meen Change>
    /// <summary>
    /// 타이틀씬 연결 전에 쓸 임시 세팅 메서드
    /// </summary>
    private void FirstSetting()
    {
        if (GameObject.FindGameObjectWithTag("BirthPos") != null)
        {
            Transform birthPos = GameObject.FindGameObjectWithTag("BirthPos").transform;

            characterController.enabled = false;
            playerController.position = birthPos.position;
            playerController.rotation = birthPos.rotation;
            characterController.enabled = true;
        }
    }

    // if (Input.GetKeyDown(KeyCode.R)) { LoadCheckPoint("Summer", 2); } // 체크포인트를 이용한 이동 예시 
    // if (Input.GetKeyDown(KeyCode.L)) { StartCoroutine(LoadHallScene("Summer")); } // 로딩통로를 이용한 이동 예시 

    #region 로딩 통로를 통한 씬 이동
    public IEnumerator LoadHallScene(string _season)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SeasonName.passage);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone) // 씬이 완전히 로드되지 않았을때
        {
            if (loadingOperation.progress >= 0.9f) // 0.9 이상 로드에 성공했다면
            {
                locomotionManager.enabled = false; // 이동 비활성화
                smoothLocomotion.enabled = false;

                playerRotation.enabled = false; // 회전 비활성화

                loadingOperation.allowSceneActivation = true; // 씬 활성화
                break;
            }

            yield return null;
        }

        StartCoroutine(OpenMainScene(_season));
    }

    private IEnumerator OpenMainScene(string _season)
    {
        string sceneName = default;

        switch(_season) // 전달받은 계절을 씬 이름으로 변환
        {
            case "Spring":
                sceneName = SeasonName.spring;
                ClearWinterEffect();
                break;

            case "Summer":
                sceneName = SeasonName.summer;
                ClearWinterEffect();
                break;

            case "Fall":
                sceneName = SeasonName.fall;
                ClearWinterEffect();
                break;

            case "Winter":
                sceneName = SeasonName.winter;
                WinterEffect(); // 겨울 패널티 발생 
                break;
        }

        AsyncOperation mainOperation = SceneManager.LoadSceneAsync(sceneName);
        mainOperation.allowSceneActivation = false;

        tempOperation = mainOperation;

        while (!mainOperation.isDone) // 씬이 완전히 로드되지 않았을때
        {
            if (mainOperation.progress >= 0.9f) // 0.9 이상 로드에 성공했다면
            {
                openDoorEvent?.Invoke(this, EventArgs.Empty); // 잠겨있는 로딩씬의 문을 여는 이벤트
                break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// (계절) 로딩씬 문을 열고 진입 시 메인씬 활성화
    /// </summary>
    public void OpenMainScene() 
    {
        locomotionManager.enabled = false; // 씬 활성화 직전 이동 비활성화
        smoothLocomotion.enabled = false;
        playerRotation.enabled = false; // 회전 비활성화

        tempOperation.allowSceneActivation = true; // 메인씬 활성화

        tempOperation = default; // 비우기
    }
    #endregion

    #region 체크포인트를 통한 씬 이동
    public void LoadCheckPoint(string _region, int _number)
    {
        audioSource.Stop();
        audioSource.clip = mapTeleportClip; // 맵 이동 오디오 재생 
        audioSource.Play();

        string inRegion = SceneManager.GetActiveScene().name;

        if (inRegion.Contains("Spring")) { inRegion = "Spring"; }
        else if (inRegion.Contains("Summer")) { inRegion = "Summer"; }
        else if (inRegion.Contains("Fall")) { inRegion = "Fall"; }
        else if (inRegion.Contains("Winter")) { inRegion = "Winter"; }

        if (inRegion == _region) // 현 지역과 체크포인트 지역이 같으면
        {
            SameRegion(_number); // TODO: 암전효과를 얹어야 한다. 
        }
        else if (inRegion != _region) // 현 지역과 이동을 원하는 체크포인트 지역이 다르면
        {
            StartCoroutine(DifferentRegion(_region, _number));
        }    
    }

    /// <summary>
    /// 같은 지역 내 체크포인트 이동 (_number => 체크포인트 소속 번호)
    /// </summary>
    private void SameRegion(int _number)
    {
        VRIFMap_CheckPoint[] checkPoints = FindObjectsOfType<VRIFMap_CheckPoint>();

        foreach(var checkPoint in checkPoints)
        {
            if (checkPoint.number == _number)
            {
                characterController.enabled = false;
                playerController.position = checkPoint.teleportPosition;
                characterController.enabled = true;
            }
        }
    }

    /// <summary>
    /// 다른 지역의 체크포인트로 이동 (_region => 이동할 지역/씬, _number => 이동 지역 내 체크포인트의 소속 번호)
    /// </summary>
    private IEnumerator DifferentRegion(string _region, int _number)
    {
        loadingCanvas.SetActive(true); // 로딩캔버스 활성화

        string regionName = default;

        switch (_region) // 전달받은 계절을 씬 이름으로 변환
        {
            case "Spring":
                regionName = SeasonName.spring;
                ClearWinterEffect();
                break;

            case "Summer":
                regionName = SeasonName.summer;
                ClearWinterEffect();
                break;

            case "Fall":
                regionName = SeasonName.fall;
                ClearWinterEffect();
                break;

            case "Winter":
                regionName = SeasonName.winter;
                WinterEffect(); // 겨울 패널티 발생
                break;
        }

        AsyncOperation mainOperation = SceneManager.LoadSceneAsync(regionName);
        mainOperation.allowSceneActivation = false;

        while (mainOperation.progress < 0.9f) // 메인씬이 로드되지 않았다면 대기 
        {
            yield return null;
        }

        mainOperation.allowSceneActivation = true; // 로드 

        while (!mainOperation.isDone) // 메인씬이 다시 완벽히 로드되기까지 대기 
        {
            yield return null;
        }

        // 지역 이동을 완료했다면 체크포인트를 찾는다
        VRIFMap_CheckPoint[] checkPoints = FindObjectsOfType<VRIFMap_CheckPoint>();

        foreach (var checkPoint in checkPoints)
        {
            if (checkPoint.number == _number)
            {
                characterController.enabled = false;
                playerController.position = checkPoint.teleportPosition;
                characterController.enabled = true;
            }
        }

        loadingCanvas.SetActive(false); // 로딩 캔버스 비활성화
    }
    #endregion

    /// <summary>
    /// 씬 완료 이벤트를 구독하고 있다. (플레이어 위치 세팅)
    /// </summary>
    private void PlayerSetting(Scene scene, LoadSceneMode mode)
    {
        Transform birthPos = GameObject.FindGameObjectWithTag("BirthPos").transform;

        characterController.enabled = false;
        playerController.position = birthPos.position;
        playerController.rotation = birthPos.rotation;
        characterController.enabled = true;

        locomotionManager.enabled = true; // 플레이어 위치시킨 후 이동 재활성화
        smoothLocomotion.enabled = true;
        playerRotation.enabled = true; // 회전 재활성화
    }

    /// <summary>
    /// 겨울이 아닌 다른 맵에 진입시 패널티 제거
    /// </summary>
    private void ClearWinterEffect()
    {
        VRIFStatusSystem.Instance.hungerTimer = VRIFStatusSystem.Instance.hungerTimer_Origin;
    }

    /// <summary>
    /// 겨울 맵에 진입시 패널티 발생
    /// </summary>
    private void WinterEffect()
    {
        VRIFStatusSystem.Instance.hungerTimer = VRIFStatusSystem.Instance.winterHungerTimer;
    }
}