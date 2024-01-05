using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [SerializeField] private QuestData questInfoForPoint;
    [Header("Config")]
    public bool isStartPoint = false;   //퀘스트 수락NPC인지 체크
    public bool isEndPoint = false;     //퀘스트 완료 NPC인지 체크
    private bool playerIsNear = false;  //플레이어가 근처에있는지 체크
    private QuestIcon questIcon;
    [SerializeField] private string questId;
    [SerializeField] private QuestState currentQuestState;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }
    private void OnEnable()
    {
        GameEventManager.instance.questEvent.onQuestStateChange += QuestStateChange;
    }
    private void OnDisable()
    {
        GameEventManager.instance.questEvent.onQuestStateChange -= QuestStateChange;

    }
    private void QuestStateChange(Quest quest)
    {
        if(quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, isStartPoint, isEndPoint);
            //Debug.Log("Quest with id: " + questId + "update to state: " + currentQuestState);
        }
    }
 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        SubmitPressed();
    }
    private void SubmitPressed()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!playerIsNear) { return; }
            if(currentQuestState.Equals(QuestState.CAN_START)&&isStartPoint)
            {
                GameEventManager.instance.questEvent.StartQuest(questId);
                //해당 id를 가진 퀘스트를 시작한다는 이벤트 전송
            }
            else if(currentQuestState.Equals(QuestState.CAN_FINISH)&&isEndPoint)
            {
                GameEventManager.instance.questEvent.FinishQuest(questId);
                //해당 id를 가진 퀘스트를 끝냈다는 이벤트 전송
            }

        }
    }       //SubmitPressed() 플레이어가 근처에서 말거는 버튼을 눌렀을때 동작하는 함수
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
