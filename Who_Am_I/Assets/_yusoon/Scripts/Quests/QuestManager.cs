using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance { get { return instance; } }
    int appleCount = 0;
    private int npcCount = 0;
    private bool[] talkedNpcs = new bool[3];
    private bool isClear = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameEventManager.instance.miscEvent.onNpcTalked += CheckNpcTalked;
        GameEventManager.instance.miscEvent.onClearbleQuest += ClearQuest;
    }

        // Update is called once per frame
        void Update()
    {

    }
    public void CheckNpcTalked(string npcName)
    {
        Debug.Log("CheckNpcTalked");

        // 특정 NPC와의 대화가 이미 기여한 경우 무시
        int npcIndex = GetNpcIndex(npcName);
        if (npcIndex != -1 && talkedNpcs[npcIndex])
        {
            Debug.Log("Already talked to this NPC in this quest.");
            return;
        }

        // 여기서 특정 NPC 이름을 확인하여 퀘스트 진행 상황을 업데이트
        if (IsTargetNpc(npcName))
        {
            npcCount++;
            Debug.Log("npc와 대화로인한 카운트 증가: " + npcCount);

            // 대화한 NPC 기록에 추가
            if (npcIndex != -1)
                talkedNpcs[npcIndex] = true;

            // 특정 NPC 3명을 만나면 퀘스트 클리어
            if (npcCount >= 3)
            {
                Debug.Log("Quest Cleared!");
                // 클리어 퀘스트 관련 처리 추가
                if(!isClear)
                {
                 GameEventManager.instance.miscEvent.QuestClearble("Tutorial");
                isClear = true;
                }
                // 클리어 후에 카운트 초기화 및 대화한 NPC 기록 초기화
                npcCount = 0;
                talkedNpcs = new bool[3];
            }
        }
    }
    public void ClearQuest(string questName)
    {
        Debug.Log(questName + "clear");
    }
    public bool IsTargetNpc(string npcName)
    {
        
        // 특정 NPC인지 확인하는 로직을 추가 (예: 미리 지정된 NPC 이름들과 비교)
        // 예시로 "NPC1", "NPC2", "NPC3"이라는 NPC를 타겟으로 설정
        return npcName.Equals("NPC1") || npcName.Equals("NPC2") || npcName.Equals("NPC3");
    }
    private int GetNpcIndex(string npcName)
    {
        switch(npcName)
        {
            case "NPC1":
                return 0;
            case "NPC2":
                return 1;
            case "NPC3":
                return 2;
            default:
                return -1;
        }
    }
}
