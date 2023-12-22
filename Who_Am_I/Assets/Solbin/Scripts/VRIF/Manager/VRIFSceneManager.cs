using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRIFSceneManager : MonoBehaviour
{
    public static VRIFSceneManager Instance;

    [Header("XR Rig Advanced")]
    [SerializeField] private GameObject xrRigPlayer = default;

    // 다음 씬에서의 원활한 작동을 위함 
    private Transform playerController;
    private bool setting = false;

    public class SeasonName
    {
        public static string spring { get; private set; } = "Solbin_Scene"; // TODO: 이후 수정 필요 
        public static string summer { get; private set; } = "Summer_Scene";
        public static string fall { get; private set; } = "Fall_Scene";
        public static string winter { get; private set; } = "Winter_Scene";
    }

    private void Awake()
    {
        DontDestroyOnLoad(xrRigPlayer); // 플레이어 파괴 금지 

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerController = xrRigPlayer.transform.GetChild(1);
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R)) { LoadCheckPoint("Summer", 2); } // 체크포인트를 이용한 이동 예시 

        // if (Input.GetKeyDown(KeyCode.L)) { StartCoroutine(LoadHallScene("Summer")); } // 로딩통로를 이용한 이동 예시 

        if (setting)
        {
            setting = false;

            Transform birthPoint = GameObject.Find("Birth Point").transform;

            playerController.position = birthPoint.position;
            playerController.rotation = birthPoint.rotation;
        }
    }

    #region 로딩 통로를 통한 씬 이동
    public IEnumerator LoadHallScene(string _season)
    {
        Debug.Log("0");

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("Loading_Scene");
        loadingOperation.allowSceneActivation = false;

        Debug.Log("1");

        while (!loadingOperation.isDone) // 씬이 완전히 로드되지 않았을때
        {
            if (loadingOperation.progress >= 0.9f) // 0.9 이상 로드에 성공했다면
            {
                loadingOperation.allowSceneActivation = true; // 씬 활성화
                break;
            }

            yield return null;
        }

        setting = true;

        StartCoroutine(OpenMainScene(_season));
    }

    private void SettingClear() { setting = false; }

    private IEnumerator OpenMainScene(string _season)
    {
        string sceneName = default;

        switch(_season) // 전달받은 계절을 씬 이름으로 변환
        {
            case "Spring":
                sceneName = SeasonName.spring;
                break;

            case "Summer":
                sceneName = SeasonName.summer;
                break;

            case "Fall":
                sceneName = SeasonName.fall;
                break;

            case "Winter":
                sceneName = SeasonName.winter;
                break;
        }

        AsyncOperation mainOperation = SceneManager.LoadSceneAsync(sceneName);
        mainOperation.allowSceneActivation = false;

        // <Solbin> 삽입 예정 코드 
        // TODO: 이후 로딩 통로에 맞춰 수정 

        //while (mainOperation.progress < 0.9f) // 메인씬이 로드되지 않았다면 대기 
        //{
        //    yield return null;
        //}
        // <Solbin> ===

        // <Solbin> 임시 삽입 코드 
        float time = 0f;

        while (time < 10)
        {
            time += Time.deltaTime;
            yield return null;
        }
        // <Solbin> ===

        mainOperation.allowSceneActivation = true; // 로드 

        while (!mainOperation.isDone) // 메인씬이 다시 완벽히 로드되기까지 대기 
        {
            yield return null;
        }

        setting = true;
    }
    #endregion

    #region 체크포인트를 통한 씬 이동
    public void LoadCheckPoint(string _region, int _number)
    {
        string inRegion = SceneManager.GetActiveScene().name;

        if (inRegion.Contains("VRIF")) { inRegion = "Spring"; } // TODO: 이후 Spring으로 수정 필요
        else if (inRegion.Contains("Summer")) { inRegion = "Summer"; }
        else if (inRegion.Contains("Fall")) { inRegion = "Fall"; }
        else if (inRegion.Contains("Winter")) { inRegion = "Winter"; }

        if (inRegion == _region) // 현 지역과 체크포인트 지역이 같으면
        {
            SameRegion(_number); // TODO: 암전효과를 얹어야 한다. 
        }
        else if (inRegion != _region) // 현 지역과 체크포인트 지역이 다르면
        {
            StartCoroutine(DifferentRegion(_region, _number)); // TODO: 로딩씬을 얹어야 한다. 
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
                playerController.position = checkPoint.teleportPosition;
            }
        }
    }

    /// <summary>
    /// 다른 지역의 체크포인트로 이동 (_region => 이동할 지역/씬, _number => 이동 지역 내 체크포인트의 소속 번호)
    /// </summary>
    private IEnumerator DifferentRegion(string _region, int _number)
    {
        string regionName = default;

        switch (_region) // 전달받은 계절을 씬 이름으로 변환
        {
            case "Spring":
                regionName = SeasonName.spring;
                break;

            case "Summer":
                regionName = SeasonName.summer;
                break;

            case "Fall":
                regionName = SeasonName.fall;
                break;

            case "Winter":
                regionName = SeasonName.winter;
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
                playerController.position = checkPoint.teleportPosition;
            }
        }
    }
    #endregion
}
