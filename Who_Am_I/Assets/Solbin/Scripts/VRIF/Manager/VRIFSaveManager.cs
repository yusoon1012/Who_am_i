using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Meta.WitAi;

[System.Serializable]
public class SaveData
{
    // 플레이어 위치
    public Vector3 playerPos;
    // 플레이어 각도
    public Vector3 playerDir;
}

public class VRIFSaveManager : MonoBehaviour
{
    // TODO: 'PC의 위치와 각도'를 저장하고 불러오는 것이 가능한지 테스트
    // 세팅은 다른 기능 스크립트의 Start()가 종료되고 하는 것으로...?

    private string savePath = default;

    private string saveFolderName = "SaveGameFile";
    private string saveFolderPath = default;


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

    #region 불러오기
    /// <summary>
    /// 1. 저장된 데이터가 없거나 2. 새로 게임을 시작했을 때의 세팅값
    /// </summary>
    private void FirstPlay()
    {
        Debug.Log("새로 시작");

        // 플레이어 Position
        VRIFGameManager.Instance.playerPos = new Vector3 (2.19f, 1.52f, 0f);
        // 플레이어 Rotation
        VRIFGameManager.Instance.playerDir = Vector3.zero;

        VRIFGameManager.Instance.TestSetting(); // 세팅

        // TODO: 추후 파이어베이스 연결 시 MainTitle에서 넘어오는 것은 VRIFSceneManager.cs가 관리할 것이다. 
        // TODO: 체크포인트 관리는? 
    }

    /// <summary>
    /// 저장된 데이터를 불러왔을때
    /// </summary>
    private void RePlay()
    {
        Debug.Log("저장된 파일 있음");

        SaveData saveData = new SaveData();

        string loadJson = File.ReadAllText(savePath); // 경로의 모든 텍스트를 읽어와 string에 할당
        saveData = JsonUtility.FromJson<SaveData>(loadJson); // 역직렬화 (경로의 json에서 class를 읽어와 담음)

        if (saveData != null) // json을 읽어오는 것에 성공했을때
        {
            VRIFGameManager.Instance.playerPos = saveData.playerPos;
            VRIFGameManager.Instance.playerDir = saveData.playerDir;
        }
    }
    #endregion

    #region 저장
    public void SaveFile()
    {
        SaveData saveData = new SaveData();

        // TODO: 저장전 정보 최신화 필요

        saveData.playerPos = VRIFGameManager.Instance.playerPos;
        saveData.playerDir = VRIFGameManager.Instance.playerDir;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }
    #endregion
}

// TODO: 새로운 게임 => 기존 json 파일을 삭제하고 새로 시작하기
// 이어하기 => 기존 json 파일을 기반으로 한 데이터 로드 
// 종료 => APK 종료 
// 새로운 게임을 시작할 때는 기존 json 데이터를 덮어쓴 후 VRIFSceneManager를 통해 봄 씬을 로드