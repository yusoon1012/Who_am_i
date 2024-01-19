using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System;
using System.Xml;

[System.Serializable]
public class SaveData
{
    // 마지막으로 열려있던 씬
    public string sceneName;
    // 플레이어 위치
    public Vector3 playerPos;
    // 플레이어 각도
    public Vector3 playerDir;

    // (QuestManager_Jun) 현재 진행 퀘스트 번호
    public int currentQuest;
    public List<SerializableQuestJun> questList;
}

/// <summary>
/// Quest_Jun이 Scriptable Object를 상속받아 Serializable을 위해 Quest_Jun과 동일하되 직렬화 된 클래스를 하나 생성함.
/// </summary>
[System.Serializable]
public class SerializableQuestJun
{
    public QuestState_Jun currentProgress;
    public string questTitle;             
    public string questGoal;              
    public List<int> targetValues;        
    public List<int> currentValues;       
    public bool? isMBTIConditions;         
    public string compensationItem;       
    public string trueMBTI;               
    public string falseMBTI;              
    public List<string> npc;                
}


// TODO: 인벤토리에 아이템이 저장되는 형태

public class VRIFSaveManager : MonoBehaviour
{
    public static VRIFSaveManager Instance;

    // 저장 폴더 이름
    private string saveFolderName = "SaveFolder";
    // 저장 폴더 경로
    private string saveFolderPath = default;

    // 정보 실 저장 경로 (저장 폴더 경로 + json 폴더의 이름)
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
        saveData.questList = new List<SerializableQuestJun>();

        // 플레이어 기본 정보 저장
        saveData.sceneName = VRIFGameManager.Instance.currentScene; // 씬 저장
        saveData.playerPos = VRIFGameManager.Instance.playerPos; // PC 위치 저장
        saveData.playerDir = VRIFGameManager.Instance.playerDir; // PC 회전값 저장 
        saveData.currentQuest = VRIFGameManager.Instance.currentQuest; // 현 퀘스트 진행 번호 저장
        // ===

        // 퀘스트 진행 정보 저장
        int questCount = QuestManager_Jun.instance.questList.Count; // 퀘스트 개수

        for (int i = 0; i < questCount; i++)
        {
            var questJun = new SerializableQuestJun();

            questJun.currentProgress = QuestManager_Jun.instance.questList[i].currentProgress;
            questJun.questTitle = QuestManager_Jun.instance.questList[i].questTitle;
            questJun.questGoal = QuestManager_Jun.instance.questList[i].questGoal;
            questJun.targetValues = QuestManager_Jun.instance.questList[i].targetValues;
            questJun.currentValues = QuestManager_Jun.instance.questList[i].currentValues;
            questJun.isMBTIConditions = QuestManager_Jun.instance.questList[i].isMBTIConditions;
            questJun.compensationItem = QuestManager_Jun.instance.questList[i].compensationItem;
            questJun.trueMBTI = QuestManager_Jun.instance.questList[i].trueMBTI;
            questJun.falseMBTI = QuestManager_Jun.instance.questList[i].falseMBTI;
            questJun.npc = QuestManager_Jun.instance.questList[i].npc;

            saveData.questList.Add(questJun); // 퀘스트 입력 내용 추가 
        }
        // ===

        string json = JsonUtility.ToJson(saveData, true); // SaveData 클래스를 통째로 json으로 변환

        File.WriteAllText(savePath, json); // 지정 폴더에 입력
    }
    #endregion
}