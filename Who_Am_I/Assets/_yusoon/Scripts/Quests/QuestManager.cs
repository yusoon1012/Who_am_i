using Firebase.Database;
using Firebase.Extensions;
using Oculus.Interaction.DebugTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestData testData;
    private Dictionary<string, Quest> questMap;
   [SerializeField] private int currentQuestIndex;
    private int saveQuestIndex = 0;
    DatabaseReference m_Reference;
    private bool isLoadDone = false;
    private int questmapcount = 0;
    private void Awake()
    {
       
        questMap = CreateQuestMap();
        //LoadQuest(testData);
        //Quest quest = GetQuestById("MeetNPC");
        //Debug.Log(quest.info.name);
        //Debug.Log(quest.info.id);
        //Debug.Log(quest.state);
        //Debug.Log(quest.CurrentStepExists());
    }
    private void OnEnable()
    {
        GameEventManager.instance.questEvent.onStartQuest += StartQuest;        //퀘스트시작 이벤트
        GameEventManager.instance.questEvent.onAdvanceQuest += AdvanceQuest;    //퀘스트 완료상태 이벤트
        GameEventManager.instance.questEvent.onFinishQuest += FinishQuest;      //퀘스트 완료 이벤트
        GameEventManager.instance.questEvent.onQuestIndexChange += QuestIndexChange;// 다음퀘스트 진행하기위한 인덱스 증가 이벤트
        GameEventManager.instance.questEvent.onQuestStepStateChange += QuestStepStateChange;// 퀘스트 조건 진행도 변경 이벤트
       


    }
    private void OnDisable()
    {
        GameEventManager.instance.questEvent.onStartQuest -= StartQuest;
        GameEventManager.instance.questEvent.onAdvanceQuest -= AdvanceQuest;
        GameEventManager.instance.questEvent.onFinishQuest -= FinishQuest;
        GameEventManager.instance.questEvent.onQuestIndexChange -= QuestIndexChange;
        GameEventManager.instance.questEvent.onQuestStepStateChange -= QuestStepStateChange;
    }
    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state==QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            GameEventManager.instance.questEvent.QuestStateChange(quest);
        }
        //퀘스트 목록 초기화 및 진행중이던 퀘스트 상태 불러오기

    }
    private void Update()
    {
          
        foreach(Quest quest in questMap.Values)
        {
            
            if(quest.state==QuestState.NOT_MET&&CheckRequirementsMet(quest))
            {

                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }// 이전 퀘스트를 클리어해서 시작가능한상태로 변경해주는 루프
        
    }

    private void ChangeQuestState(string id,QuestState state_)
    {
        Quest quest = GetQuestById(id); //퀘스트 id값으로 찾아온값을 저장
        quest.state = state_;
        GameEventManager.instance.questEvent.QuestStateChange(quest);//퀘스트 상태 변경 이벤트
    }       //ChangeQuestState() 퀘스트 상태 변경 이벤트 구독받기위한 함수
    private void QuestIndexChange(int index_)
    {
        currentQuestIndex = index_;
    }       //QuestIndexChange() 퀘스트 인덱스 변경 이벤트 구독받기위한 함수
    private void QuestStepStateChange(string id,int stepIdx,QuestStepState queststepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(queststepState, stepIdx);
        ChangeQuestState(id, quest.state);
    }       //QuestStepStateChange() 퀘스트 조건 진행도 변경 이벤트 구독받기위한 함수
    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetRequirements = true;
        if(currentQuestIndex!=quest.info.questIndex)
        {
            meetRequirements = false;
        }
        foreach(QuestData questinfo in quest.info.questPrerequisites)
        {
            if(GetQuestById(questinfo.id).state!=QuestState.FINISHED)
            {
                meetRequirements = false;
            }
        }
        return meetRequirements;
    }       //CheckRequirementsMet() 선행퀘스트 클리어했는지 반환하는 함수
    private void StartQuest(string id)
    {
        Debug.Log("Start Quest : "+id);
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }       //StartQuest() 퀘스트 시작 이벤트 구독해서 상태 변경해주는 함수
    private void AdvanceQuest(string id)
    {
        Debug.Log("Advanced Quest : " + id);
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();
        if(quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }

    }       //AdvanceQuest() 퀘스트 진행하여서 클리어 가능한 상태로 변경해주는 함수

    private void FinishQuest(string id)
    {
        Debug.Log("Finish Quest : " + id);
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
       ChangeQuestState(quest.info.id, QuestState.FINISHED);

    }       //FinishQuest() 퀘스트 클리어하여서 상태를 변경해줄때 사용하는 함수
    private void ClaimRewards(Quest quest)
    {
        GameEventManager.instance.questEvent.QuestIndexChange(quest.info.questIndex + 1);
    }       //ClaimRewards() 퀘스트 보상 등을 획득하는 함수 현재는 인덱스 증가해서 다음 퀘스트 진행할 수 있도록 한 상황
    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestData[] allQuests = Resources.LoadAll<QuestData>("Quests");
        Dictionary<string,Quest> idToQuestMap=new Dictionary<string, Quest>();
        foreach(QuestData questInfo in allQuests)
        {
            if(idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map : " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id,LoadQuest(questInfo));
           // Debug.Log(idToQuestMap.Values);
           // Debug.Log("All Quest ID: " + questInfo.id);
        }
       // Debug.Log("CreateQuestMap Done");
        return idToQuestMap;
    }       //CreateQuestMap() Resources폴더 안에 있는 QuestData 스크립트들을 불러와서 전체 퀘스트 목록 만들어주는 함수
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if(quest == null)
        {
            Debug.LogError("ID not found");
        }
        return quest;
    }       //GetQuestById() 퀘스트 id로 찾아와 리턴하는 함수
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        foreach(Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }       // loop: 퀘스트 목록에 있는 퀘스트들을 저장하기위한 루프
    }       //OnApplicationQuit() 퀘스트 저장하는 함수를 호출
    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestInfo info = quest.GetQuestInfo();
            string serializedData = JsonUtility.ToJson(info);
            PlayerPrefs.SetString(quest.info.id,serializedData);
            Debug.Log(serializedData + "전송완료");
            
        }
        catch(System.Exception e)
        {
            Debug.LogWarning("Failed to save quest with id  " + quest.info.id + ":" + e);
        }
    }       //SaveQuest() quest 클래스에있는 값들을 json파일로 직렬화시켜서 playerprefs로 저장하는 함수
    public Quest LoadQuest(QuestData qData)
    {
        Quest quest = null;
        try
        {
           if(PlayerPrefs.HasKey(qData.id))
            {
                string serializedData = PlayerPrefs.GetString(qData.id);
                QuestInfo questInfo = JsonUtility.FromJson<QuestInfo>(serializedData);
                quest = new Quest(qData, questInfo.state, questInfo.questStepIndex, questInfo.queststepStates);
            }
           else
            {
                quest = new Quest(qData);
            }
            
        }
        catch(System.Exception e)
        {
            Debug.LogError("Failed to load quest with id: "+quest.info.id+e);  
        }
        
        return quest;
    }       //LoadQuest() PlayerPrefs에 저장된 퀘스트 id를 찾아서 진행도 등을 불러오는 함수

    #region legacy
    //private static QuestManager instance;
    //public static QuestManager Instance { get { return instance; } }
    //int appleCount = 0;
    //private int npcCount = 0;
    //private bool[] talkedNpcs = new bool[3];
    //private bool isClear = false;
    //private void Awake()
    //{
    //    if(instance == null)
    //    {
    //        instance = this;
    //    }
    //}
    //// Start is called before the first frame update
    //void Start()
    //{
    //    GameEventManager.instance.miscEvent.onNpcTalked += CheckNpcTalked;
    //    GameEventManager.instance.miscEvent.onClearbleQuest += ClearQuest;
    //}

    //    // Update is called once per frame
    //    void Update()
    //{

    //}
    //public void CheckNpcTalked(string npcName)
    //{
    //    Debug.Log("CheckNpcTalked");

    //    // 특정 NPC와의 대화가 이미 기여한 경우 무시
    //    int npcIndex = GetNpcIndex(npcName);
    //    if (npcIndex != -1 && talkedNpcs[npcIndex])
    //    {
    //        Debug.Log("Already talked to this NPC in this quest.");
    //        return;
    //    }

    //    // 여기서 특정 NPC 이름을 확인하여 퀘스트 진행 상황을 업데이트
    //    if (IsTargetNpc(npcName))
    //    {
    //        npcCount++;
    //        Debug.Log("npc와 대화로인한 카운트 증가: " + npcCount);

    //        // 대화한 NPC 기록에 추가
    //        if (npcIndex != -1)
    //            talkedNpcs[npcIndex] = true;

    //        // 특정 NPC 3명을 만나면 퀘스트 클리어
    //        if (npcCount >= 3)
    //        {
    //            Debug.Log("Quest Cleared!");
    //            // 클리어 퀘스트 관련 처리 추가
    //            if(!isClear)
    //            {
    //             GameEventManager.instance.miscEvent.QuestClearble("Tutorial");
    //            isClear = true;
    //            }
    //            // 클리어 후에 카운트 초기화 및 대화한 NPC 기록 초기화
    //            npcCount = 0;
    //            talkedNpcs = new bool[3];
    //        }
    //    }
    //}
    //public void ClearQuest(string questName)
    //{
    //    Debug.Log(questName + "clear");
    //}
    //public bool IsTargetNpc(string npcName)
    //{

    //    // 특정 NPC인지 확인하는 로직을 추가 (예: 미리 지정된 NPC 이름들과 비교)
    //    // 예시로 "NPC1", "NPC2", "NPC3"이라는 NPC를 타겟으로 설정
    //    return npcName.Equals("NPC1") || npcName.Equals("NPC2") || npcName.Equals("NPC3");
    //}
    //private int GetNpcIndex(string npcName)
    //{
    //    switch(npcName)
    //    {
    //        case "NPC1":
    //            return 0;
    //        case "NPC2":
    //            return 1;
    //        case "NPC3":
    //            return 2;
    //        default:
    //            return -1;
    //    }
    //}
    #endregion
}
