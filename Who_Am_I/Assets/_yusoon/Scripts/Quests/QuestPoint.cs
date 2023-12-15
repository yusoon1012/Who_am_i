using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [SerializeField] private QuestData questInfoForPoint;
    [Header("Config")]
    [SerializeField] private bool isStartPoint = false;
    [SerializeField] private bool isEndPoint = false;
    private bool playerIsNear = false;
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
            Debug.Log("Quest with id: " + questId + "update to state: " + currentQuestState);
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
            }
            else if(currentQuestState.Equals(QuestState.CAN_FINISH)&&isEndPoint)
            {
                GameEventManager.instance.questEvent.FinishQuest(questId);
            }

        }
    }
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
