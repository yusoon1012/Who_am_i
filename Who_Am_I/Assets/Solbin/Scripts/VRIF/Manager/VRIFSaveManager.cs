using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Meta.WitAi;
using static VRIFSceneManager;
using System.Drawing;

[System.Serializable]
public class SaveData
{
    // 마지막으로 열려있던 씬
    public string sceneName;
    // 플레이어 위치
    public Vector3 playerPos;
    // 플레이어 각도
    public Vector3 playerDir;
}

public class VRIFSaveManager : MonoBehaviour
{
    // TODO: 'PC의 위치와 각도'를 저장하고 불러오는 것이 가능한지 테스트
    // 세팅은 다른 기능 스크립트의 Start()가 종료되고 하는 것으로...?

    public static VRIFSaveManager Instance;

    // 저장 폴더 이름
    private string saveFolderName = "SaveGameFile";
    // 저장 폴더 경로
    private string saveFolderPath = default;

    // 실 저장 경로 (저장 폴더 경로 + json 폴더의 이름)
    private string savePath = default;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        saveFolderPath = Path.Combine(Application.dataPath, saveFolderName); // 저장 폴더 경로

        if (!Directory.Exists(saveFolderPath)) // 저장 폴더 경로에 저장 폴더가 존재하지 않으면
        {
            Directory.CreateDirectory(saveFolderPath); // 저장 폴더 생성
        }

        savePath = Path.Combine(saveFolderPath, "playerData.json"); // 저장 폴더 안 json 폴더 경로
        
        SelectLoad(); // TODO: 추후 Start()에서 빠져야 한다. 
    }

    /// <summary>
    /// 파이어베이스가 연결되면 삭제될 테스트 메소드 (json 테스트용) 
    /// </summary>
    private void SelectLoad()
    {
        // TODO: 추후 타이틀 씬 선택값으로 선택
        if (!File.Exists(savePath)) { FirstPlay(); } // 저장된 파일이 존재하지 않으면 초기값 세팅 
        else { RePlay(); } // 저장된 파일이 있으면 
    }

    #region 시작: VRIFGameManager 선순위, 새로 시작할 때 
    /// <summary>
    /// 1. 저장된 데이터가 없거나 2. 새로 게임을 시작했을 때의 세팅값
    /// </summary>
    private void FirstPlay()
    {
        // 플레이어 Position 세팅
        VRIFGameManager.Instance.playerPos = new Vector3 (2.19f, 1.52f, 0f);
        // 플레이어 Rotation 세팅
        VRIFGameManager.Instance.playerDir = Vector3.zero;

        VRIFGameManager.Instance.PlayerSetting(); // 테스트 세팅

        // TODO: 추후 파이어베이스 연결 시 MainTitle에서 넘어오는 것은 VRIFSceneManager.cs가 관리할 것이다. 
        // TODO: 체크포인트 관리는? 
    }
    #endregion

    #region 시작: VRIFGameManager 선순위, 이어할 때
    /// <summary>
    /// 저장된 데이터를 불러왔을때
    /// </summary>
    private void RePlay()
    {
        SaveData saveData = new SaveData();

        string loadJson = File.ReadAllText(savePath); // 경로의 모든 텍스트를 읽어와 string에 할당
        saveData = JsonUtility.FromJson<SaveData>(loadJson); // 역직렬화 (경로의 json에서 class를 읽어와 담음)

        if (saveData != null) // json을 읽어오는 것에 성공했을때
        {
            StartCoroutine(OpenScene(saveData));
        }
    }

    /// <summary>
    /// 기존 Scene을 불러온 후 정보대로 세팅
    /// </summary>
    /// <param name="saveData_">저장된 json에서 읽어온 임시 saveData</param>
    /// <returns></returns>
    private IEnumerator OpenScene(SaveData saveData_)
    {
        //loadingCanvas.SetActive(true); // 로딩캔버스 활성화

        AsyncOperation mainOperation = SceneManager.LoadSceneAsync(saveData_.sceneName); // 저장되어 있던 씬 이름
        mainOperation.allowSceneActivation = false;

        while (!mainOperation.isDone) // 메인씬이 다시 완벽히 로드되기까지 대기 
        {
            if (mainOperation.progress >= 0.9f) // 로딩률 0.9 이상이면
            {
                mainOperation.allowSceneActivation = true; // 씬 오픈

                break;
            }

            yield return null;
        }

        // TODO: 씬이 계속 넘어가지 않으면 언로드 고려 

        // 플레이어 Position 세팅
        VRIFGameManager.Instance.playerPos = saveData_.playerPos;
        // 플레이어 Rotation 세팅
        VRIFGameManager.Instance.playerDir = saveData_.playerDir;

        VRIFGameManager.Instance.PlayerSetting(); // 저장 정보 로드 후 세팅

        // loadingCanvas.SetActive(false); // 로딩 캔버스 비활성화
    }
    #endregion

    #region 저장: VRIFGameManager 후순위
    public void SaveFile()
    {
        SaveData saveData = new SaveData();

        saveData.sceneName = VRIFGameManager.Instance.currentScene; // 씬 저장
        saveData.playerPos = VRIFGameManager.Instance.playerPos; // PC 위치 저장
        saveData.playerDir = VRIFGameManager.Instance.playerDir; // PC 회전값 저장 

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }
    #endregion
}

// TODO: 새로운 게임 => 기존 json 파일을 삭제하고 새로 시작하기
// 이어하기 => 기존 json 파일을 기반으로 한 데이터 로드 
// 종료 => APK 종료 
// 새로운 게임을 시작할 때는 기존 json 데이터를 덮어쓴 후 VRIFSceneManager를 통해 봄 씬을 로드