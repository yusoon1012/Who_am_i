using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
    public static VRIFSaveManager Instance;

    // 저장 폴더 이름
    private string saveFolderName = "SaveFolder";
    // 저장 폴더 경로
    private string saveFolderPath = default;

    // 실 저장 경로 (저장 폴더 경로 + json 폴더의 이름)
    private string savePath = default;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        saveFolderPath = Path.Combine(Application.dataPath, saveFolderName); // 저장 폴더 경로
        savePath = Path.Combine(saveFolderPath, "playerData.json"); // 저장 폴더 안 json 폴더 경로
    }

    #region 저장: VRIFGameManager 후순위 실행
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