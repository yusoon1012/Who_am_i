using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// 타이틀 => 새로운 게임, 이어하기, 종료하기 연결 
/// </summary>
public class VRIFTitleManager : MonoBehaviour
{
    public static VRIFTitleManager Instance;

    // 저장 폴더 이름
    private string saveFolderName = "SaveFolder";
    // 저장 폴더 경로
    private string saveFolderPath = default;

    // 실 저장 경로 (저장 폴더 경로 + json 폴더의 이름)
    private string savePath = default;

    [Header("이어하기 버튼")]
    public Button continueButton = default;

    // 봄 씬 이름
    private string springSceneName = "M_Spring_Scene";

    [Header("Black Image")]
    [Tooltip("암전 효과를 위한 Dark Canvas - Black Image")]
    [SerializeField] private Image blackImage = default;

    // 이어하기 선택 여부
    private bool isContinue = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        saveFolderPath = Path.Combine(Application.dataPath, saveFolderName); // 저장 폴더 경로

        if (!Directory.Exists(saveFolderPath)) // 저장 폴더 경로에 저장 폴더가 존재하지 않으면
        {
            Directory.CreateDirectory(saveFolderPath); // 저장 폴더 생성
        }

        savePath = Path.Combine(saveFolderPath, "playerData.json"); // 저장 폴더 안 json 폴더 경로

        if (!File.Exists(savePath))
        {
            continueButton.interactable = false; // 저장 파일이 존재하지 않으면 '이어하기' 비활성화
        }
        else { continueButton.interactable = true; } // 존재한다면 '이어하기' 활성화
    }

    #region 새로운 게임
    /// <summary>
    /// '새로운 게임' 클릭
    /// </summary>
    /// 
    public void ClickNewGame() { StartCoroutine(LoadNewGame()); }

    /// <summary>
    /// 새로운 게임 로딩
    /// </summary>
    private IEnumerator LoadNewGame()
    {
        SaveData saveData = new SaveData();

        if (File.Exists(savePath)) // 기존 세이브 파일이 존재한다면
        {
            File.Delete(savePath); // 삭제 처리 
        }

        AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(springSceneName);
        sceneOperation.allowSceneActivation = false;

        while (!sceneOperation.isDone) // 씬이 완전히 로드되지 않았을때
        {
            if (sceneOperation.progress >= 0.9f) // 0.9 이상 로드에 성공했다면
            {
                break;
            }

            yield return null;
        }

        sceneOperation.allowSceneActivation = true;
    }
    #endregion

    #region 이어하기
    /// <summary>
    /// '이어하기' 클릭
    /// </summary>
    public void ClickContinue()
    {
        isContinue = true;

        SaveData saveData = new SaveData();

        string loadJson = File.ReadAllText(savePath); // 경로의 모든 텍스트를 읽어와 string에 할당
        saveData = JsonUtility.FromJson<SaveData>(loadJson); // 역직렬화

        if (saveData != null) // json을 읽어오는 것에 성공했을때
        {
            StartCoroutine(LoadSaveScene(saveData));
        }
    }

    /// <summary>
    /// 씬 로드를 가리기 위한 이펙트 
    /// </summary>
    private IEnumerator DarkEffect()
    {
        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame(); // 프레임 대기 

        Color blackColor = blackImage.color;

        while (blackColor.a < 1)
        {
            blackColor.a += 0.1f;

            if (blackColor.a >= 1)
            {
                break;
            }

            yield return endOfFrame;
        }
    }

    /// <summary>
    /// 저장된 json 로딩 후 세팅
    /// </summary>
    /// <param name="saveData_">저장된 json에서 읽어온 데이터</param>
    /// <returns></returns>
    private IEnumerator LoadSaveScene(SaveData saveData_)
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

        while (VRIFGameManager.Instance == null) // VRIFGameManager.Instance 할당까지 대기
        {
            if (VRIFGameManager.Instance != null)
            {
                break;
            }

            yield return null;
        }

        // 플레이어 Position 세팅
        VRIFGameManager.Instance.playerPos = saveData_.playerPos;
        // 플레이어 Rotation 세팅
        VRIFGameManager.Instance.playerDir = saveData_.playerDir;

        VRIFGameManager.Instance.PlayerSetting(); // 저장 정보 로드 후 세팅

        // TODO: AddInventory로 인벤토리 세팅, 

        // loadingCanvas.SetActive(false); // 로딩 캔버스 비활성화
    }

    public void QuestSetting()
    {
        if (isContinue)
        {
            SaveData saveData = new SaveData();

            string loadJson = File.ReadAllText(savePath); // 경로의 모든 텍스트를 읽어와 string에 할당
            saveData = JsonUtility.FromJson<SaveData>(loadJson); // 역직렬화

            // 퀘스트 넘버 세팅
            QuestManager_Jun.instance.currentQuest = saveData.currentQuest; 
            // ===

            // 퀘스트 리스트 세팅
            int questCount = QuestManager_Jun.instance.questList.Count; // 퀘스트 개수

            for (int i = 0; i < questCount; i++)
            {
                QuestManager_Jun.instance.questList[i].currentProgress = saveData.questList[i].currentProgress;
                QuestManager_Jun.instance.questList[i].questTitle = saveData.questList[i].questTitle;
                QuestManager_Jun.instance.questList[i].questGoal = saveData.questList[i].questGoal;
                QuestManager_Jun.instance.questList[i].targetValues = saveData.questList[i].targetValues;
                QuestManager_Jun.instance.questList[i].currentValues = saveData.questList[i].currentValues;
                QuestManager_Jun.instance.questList[i].isMBTIConditions = saveData.questList[i].isMBTIConditions;
                QuestManager_Jun.instance.questList[i].compensationItem = saveData.questList[i].compensationItem;
                QuestManager_Jun.instance.questList[i].trueMBTI = saveData.questList[i].trueMBTI;
                QuestManager_Jun.instance.questList[i].falseMBTI = saveData.questList[i].falseMBTI;
                QuestManager_Jun.instance.questList[i].npc = saveData.questList[i].npc;
            }
            // ===
        }
    }
    #endregion

    /// <summary>
    /// '종료' 클릭
    /// </summary>
    public void ClickExit()
    {
        Application.Quit(); // 애플리케이션 종료 
    }
}
