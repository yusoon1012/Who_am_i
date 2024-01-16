using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager_Jun : MonoBehaviour
{
    #region Singleton
    public static QuestManager_Jun instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        InitializationTable();
        InitializeQuests();
    }
    #endregion

    #region public members
    public static event Action<string> setEvent;
    public List<Quest_Jun> questList;
    public int currentQuest = 0;
    #endregion

    #region event Call
    public void CallEvent()
    {
        if (setEvent != null)
        {
            setEvent.Invoke((10000 + 1 + currentQuest).ToString());
        }
    }
    #endregion

    #region Initialization and Setup
    public Dictionary<string, string> stringTable;
    public Dictionary<string, Dictionary<string, string>> npcTable;
    public Dictionary<string, Dictionary<string, string>> questTable;
    public Dictionary<string, Dictionary<string, string>> questProgressTable;

    private void InitializationTable()
    {
        stringTable = new Dictionary<string, string>();
        npcTable = new Dictionary<string, Dictionary<string, string>>();
        questTable = new Dictionary<string, Dictionary<string, string>>();
        questProgressTable = new Dictionary<string, Dictionary<string, string>>();

        stringTable = CSVReader.ReadCSVKeyString("StringTable");
        npcTable = CSVReader.ReadCSVKeyDictionary("NpcTable");
        questTable = CSVReader.ReadCSVKeyDictionary("QuestTable");
        questProgressTable = CSVReader.ReadCSVKeyDictionary("QuestProgressTable");
    }

    private void InitializeQuests()
    {
        /*  isItemConditions             1명 이상과 인사
        *   isMBTIConditions      I / E  1명과 인사
        */
        Quest_Jun quest_10001 = CreateQuest
            (
            QuestState_Jun.ACCEPTED,
            stringTable["Title_Main_Quest_0001"],
            stringTable["Goal_Main_Quest_0001"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10001"]["REWARD"],
            questTable["10001"]["MBTIVALUE1"],
            questTable["10001"]["MBTIVALUE2"],
            new List<string> { "NPC_001", "NPC_002", "NPC_003", "NPC_004" }
            );
        /*  isItemConditions             딸기 5개 채집
        *   isMBTIConditions      N / S  설명 듣기 O / X
        */
        Quest_Jun quest_10002 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
           stringTable["Title_Main_Quest_0002"],
            stringTable["Goal_Main_Quest_0002"],
            new List<int> { 5 },
            new List<int> { 0 },
            false,
            questTable["10002"]["REWARD"],
            questTable["10002"]["MBTIVALUE1"],
            questTable["10002"]["MBTIVALUE2"],
            new List<string> { "NPC_002", "NPC_021" }
            );
        /*  isItemConditions             오리 5마리 잡아오기
        *   isMBTIConditions      F / T  이웃에게 오리 기부 O / X
        */
        Quest_Jun quest_10003 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0003"],
            stringTable["Goal_Main_Quest_0003"],
            new List<int> { 5 },
            new List<int> { 0 },
            false,
            questTable["10003"]["REWARD"],
            questTable["10003"]["MBTIVALUE1"],
            questTable["10003"]["MBTIVALUE2"],
            new List<string> { "NPC_003", "NPC_022" }
            );
        /*  isItemConditions             송이 불고기 제작
        *   isMBTIConditions      J / P  설명 듣기 O / X
        */
        Quest_Jun quest_10004 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0004"],
            stringTable["Goal_Main_Quest_0004"],
            new List<int> { 1 },
            new List<int> { 0 },
            false,
            questTable["10004"]["REWARD"],
            questTable["10004"]["MBTIVALUE1"],
            questTable["10004"]["MBTIVALUE2"],
            new List<string> { "NPC_004", "NPC_023" }
            );
        /*  isItemConditions             촌장 티타무와 대화
        *   isMBTIConditions             default
        */
        Quest_Jun quest_10005 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0005"],
            stringTable["Goal_Main_Quest_0005"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10005"]["REWARD"],
            null,
            null,
            new List<string> { "NPC_005" }
            );
        /*  isItemConditions             물고기 5마리 잡기
        *   isMBTIConditions      J / P  설명 듣기 O / X
        */
        Quest_Jun quest_10006 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0006"],
            stringTable["Goal_Main_Quest_0006"],
            new List<int> { 5 },
            new List<int> { 0 },
            false,
            questTable["10006"]["REWARD"],
            questTable["10006"]["MBTIVALUE1"],
            questTable["10006"]["MBTIVALUE2"],
            new List<string> { "NPC_006", "NPC_024", "NPC_025" }
            );
        /*  isItemConditions             삐리뽀 구해주기
        *   isMBTIConditions      E / I  질문하기 O / X
        */
        Quest_Jun quest_10007 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0007"],
            stringTable["Goal_Main_Quest_0007"],
            new List<int> { 1 },
            new List<int> { 0 },
            false,
            questTable["10007"]["REWARD"],
            questTable["10007"]["MBTIVALUE1"],
            questTable["10007"]["MBTIVALUE2"],
            new List<string> { "NPC_007", "NPC_026", "NPC_027", "NPC_028" }
            );
        /*  isItemConditions             피쉬앤칩스 전달
        *   isMBTIConditions      F / T  환자에게 전달
        */
        Quest_Jun quest_10008 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0008"],
            stringTable["Goal_Main_Quest_0008"],
            new List<int> { 1 },
            new List<int> { 0 },
            false,
            questTable["10008"]["REWARD"],
            questTable["10008"]["MBTIVALUE1"],
            questTable["10008"]["MBTIVALUE2"],
            new List<string> { "NPC_008", "NPC_029" }
            );
        /*  isItemConditions             보물 3개 찾아오기
        *   isMBTIConditions      N / S  흐름에 관해 질문
        */
        Quest_Jun quest_10009 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0009"],
            stringTable["Goal_Main_Quest_0009"],
            new List<int> { 3 },
            new List<int> { 0 },
            false,
            questTable["10009"]["REWARD"],
            questTable["10009"]["MBTIVALUE1"],
            questTable["10009"]["MBTIVALUE2"],
            new List<string> { "NPC_009", "NPC_030", "NPC_031" }
            );
        /*  isItemConditions             트리테와 대화
        *   isMBTIConditions             default
        */
        Quest_Jun quest_10010 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0010"],
            stringTable["Goal_Main_Quest_0010"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10010"]["REWARD"],
            null,
            null,
            new List<string> { "NPC_010" }
            );
        /*  isItemConditions             제한 시간 안에 정상 오르기
        *   isMBTIConditions      E / I  한 명 이상과 대화
        */
        Quest_Jun quest_10011 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0011"],
            stringTable["Goal_Main_Quest_0011"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10011"]["REWARD"],
            questTable["10011"]["MBTIVALUE1"],
            questTable["10011"]["MBTIVALUE2"],
            new List<string> { "NPC_011", "NPC_032", "NPC_033" }
            );
        /*  isItemConditions             고구마 3개 채집
        *   isMBTIConditions      F / T  고구마 나눠주기
        */
        Quest_Jun quest_10012 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0012"],
            stringTable["Goal_Main_Quest_0012"],
            new List<int> { 3 },
            new List<int> { 0 },
            default,
            questTable["10012"]["REWARD"],
            questTable["10012"]["MBTIVALUE1"],
            questTable["10012"]["MBTIVALUE2"],
            new List<string> { "NPC_012", "NPC_034" }
            );
        /*  isItemConditions             동물 3마리 사냥 물고기 3마리 낚시
        *   isMBTIConditions      S / N  바로 진행
        */
        Quest_Jun quest_10013 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0013"],
            stringTable["Goal_Main_Quest_0013"],
            new List<int> { 3, 3 },
            new List<int> { 0, 0 },
            default,
            questTable["10013"]["REWARD"],
            questTable["10013"]["MBTIVALUE1"],
            questTable["10013"]["MBTIVALUE2"],
            new List<string> { "NPC_013", "NPC_035" }
            );
        /*  isItemConditions             대추 1개 채집
        *   isMBTIConditions      J / P  얼마나 모아오는지 질문
        */
        Quest_Jun quest_10014 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0014"],
            stringTable["Goal_Main_Quest_0014"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10014"]["REWARD"],
            questTable["10014"]["MBTIVALUE1"],
            questTable["10014"]["MBTIVALUE2"],
            new List<string> { "NPC_014", "NPC_036" }
            );
        /*  isItemConditions             단단뭉과 대화
        *   isMBTIConditions             default
        */
        Quest_Jun quest_10015 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0015"],
            stringTable["Goal_Main_Quest_0015"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10015"]["REWARD"],
            null,
            null,
            new List<string> { "NPC_015" }
            );
        /*  isItemConditions             대추차 1개 제작
        *   isMBTIConditions      J / P  NPC에게 말걸기
        */
        Quest_Jun quest_10016 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0016"],
            stringTable["Goal_Main_Quest_0016"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10016"]["REWARD"],
            questTable["10016"]["MBTIVALUE1"],
            questTable["10016"]["MBTIVALUE2"],
            new List<string> { "NPC_016", "NPC_037" }
            );
        /*  isItemConditions             인벤토리에 있는 음식 3개 기부
        *   isMBTIConditions      T / F  NPC와 대화
        */
        Quest_Jun quest_10017 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0017"],
            stringTable["Goal_Main_Quest_0017"],
            new List<int> { 1 },
            new List<int> { 0 },
            false,
            questTable["10017"]["REWARD"],
            questTable["10017"]["MBTIVALUE1"],
            questTable["10017"]["MBTIVALUE2"],
            new List<string> { "NPC_017", "NPC_038", "NPC_039", "NPC_040" }
            );
        /*  isItemConditions             채칩 1, 낚시 2, 수렵 3
        *   isMBTIConditions      S / N  NPC와 상호작용
        */
        Quest_Jun quest_10018 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0018"],
            stringTable["Goal_Main_Quest_0018"],
            new List<int> { 1, 2, 3 },
            new List<int> { 0, 0, 0 },
            false,
            questTable["10018"]["REWARD"],
            questTable["10018"]["MBTIVALUE1"],
            questTable["10018"]["MBTIVALUE2"],
            new List<string> { "NPC_018", "NPC_041" }
            );
        /*  isItemConditions             제한 시간 안에 정상에서 아이템 가져오기
        *   isMBTIConditions      E / I  한명 이상과 상호작용
        */
        Quest_Jun quest_10019 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0019"],
            stringTable["Goal_Main_Quest_0019"],
            new List<int> { 1 },
            new List<int> { 0 },
            false,
            questTable["10019"]["REWARD"],
            questTable["10019"]["MBTIVALUE1"],
            questTable["10019"]["MBTIVALUE2"],
            new List<string> { "NPC_019", "NPC_042", "NPC_043" }
            );
        /*  isItemConditions             촌장과 대화
        *   isMBTIConditions             default
        */
        Quest_Jun quest_10020 = CreateQuest
            (
            QuestState_Jun.NOTACCEPTED,
            stringTable["Title_Main_Quest_0020"],
            stringTable["Goal_Main_Quest_0020"],
            new List<int> { 1 },
            new List<int> { 0 },
            default,
            questTable["10020"]["REWARD"],
            null,
            null,
            new List<string> { "NPC_020" }
            );

        questList.Add(quest_10001);
        questList.Add(quest_10002);
        questList.Add(quest_10003);
        questList.Add(quest_10004);
        questList.Add(quest_10005);
        questList.Add(quest_10006);
        questList.Add(quest_10007);
        questList.Add(quest_10008);
        questList.Add(quest_10009);
        questList.Add(quest_10010);
        questList.Add(quest_10011);
        questList.Add(quest_10012);
        questList.Add(quest_10013);
        questList.Add(quest_10014);
        questList.Add(quest_10015);
        questList.Add(quest_10016);
        questList.Add(quest_10017);
        questList.Add(quest_10018);
        questList.Add(quest_10019);
        questList.Add(quest_10020);
    }

    private Quest_Jun CreateQuest
        (
        QuestState_Jun _currentProgress,
        string _questTitle,
        string _questGoal,
        List<int> _targetValues,
        List<int> _currentValues,
        bool _isMBTIConditions,
        string _compensationItem,
        string _trueMBTI,
        string _falseMBTI,
        List<string> _npc
        )
    {
        Quest_Jun newQuest = ScriptableObject.CreateInstance<Quest_Jun>();

        newQuest.currentProgress = _currentProgress;
        newQuest.questTitle = _questTitle;
        newQuest.questGoal = _questGoal;
        newQuest.targetValues = _targetValues;
        newQuest.currentValues = _currentValues;
        newQuest.isMBTIConditions = _isMBTIConditions;
        newQuest.compensationItem = _compensationItem;
        newQuest.trueMBTI = _trueMBTI;
        newQuest.falseMBTI = _falseMBTI;
        newQuest.npc = _npc;

        return newQuest;
    }
    #endregion

    #region NPC Interaction
    // NPC Id값 반환
    public string Return_Id(string _name)
    {
        return npcTable[_name]["ID"];
    }

    // NPC Default 대사 반환
    public string Return_DefaultTalk(string _name)
    {
        string stringKey = npcTable[_name]["STRINGTABLEKEY"];
        return stringTable[stringKey];
    }

    // 테이블을 이용한 대화 리스트 반환
    public List<string> QuestTalk(string _questId, string _typeId, string _npcId)
    {
        List<string> newList = new List<string>();
        int maxValue = 100000 + questProgressTable.Count;
        int currentIndex = 0;

        for (int i = 100000; i < maxValue; i++)
        {
            if
                (
                questProgressTable[i.ToString()]["QUESTID"] == _questId &&
                questProgressTable[i.ToString()]["TYPE"] == _typeId &&
                questProgressTable[i.ToString()]["NPCID"] == _npcId
                )
            {
                if (currentIndex.ToString() == questProgressTable[i.ToString()]["ORDER"])
                {
                    newList.Add(stringTable[questProgressTable[i.ToString()]["STRINGTABLEKEY"]]);
                    currentIndex += 1;
                }
                else { break; }
            }
        }

        return newList.Count > 0 ? newList : null;
    }
    #endregion

    #region Quest Progress Function
    // 현재 진행중인 퀘스트 보상
    public void QuestCompensation()
    {
        GameManager.instance.player.GetComponent<Inventory>().AddInventory(questList[currentQuest].compensationItem, 1);
        GameManager.instance.AddMBTI(questList[currentQuest].isMBTIConditions ?
            questList[currentQuest].trueMBTI : questList[currentQuest].falseMBTI);
    }

    // 현재 진행중인 퀘스트가 완료 가능한지 체크
    public bool CheckQuestCompensation()
    {
        if (questList[currentQuest].currentProgress != QuestState_Jun.PROGRESSED) { return false; }
        if (questList[currentQuest].isMBTIConditions == default) { return false; }
        if (ItemCheck() == false) { return false; }

        return true;
    }

    // 현재 진행중인 퀘스트 value값 체크
    private bool ItemCheck()
    {
        for (int i = 0; i < questList[currentQuest].targetValues.Count; i++)
        {
            if (!(questList[currentQuest].targetValues[i] <= questList[currentQuest].currentValues[i]))
            { return false; }
        }

        return true;
    }

    // 현재 진행중인 퀘스트 상태 반환 메서드
    public QuestState_Jun FindCurrentQuestState()
    {
        if (CheckQuestCompensation())
        {
            questList[currentQuest].currentProgress = QuestState_Jun.COMPLETED;
        }

        return questList[currentQuest].currentProgress;
    }

    // 현재 진행중인 퀘스트를 인터페이스에 표시하는 메서드
    public void SetCurrentQuestInterface()
    {

    }

    // 현재 진행중인 퀘스트 상태를 변경하는 메서드
    public void SetCurrentQuestState(QuestState_Jun _state)
    {
        questList[currentQuest].currentProgress = _state;
    }

    // 현재 진행중인 퀘스트의 특정 인덱스의 값을 변경하는 메서드
    public void AddCurrentQuestValue(int _index)
    {
        questList[currentQuest].currentValues[_index] += 1;
    }

    public void CheckClear(string _name)
    {
        Debug.Log("받은 정보: " + _name);
        // 1. 동물 오브젝트.GetComponent<Animal>().data.name 가져오기 (영어)
        // 2. 낚시 int값에 1 더하기 (string으로 1 보내기)
        // 3. 딸기를 인벤토리에 보냄 (한글)
    }
    #endregion
}