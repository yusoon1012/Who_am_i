using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public TMP_Text talkerNameTxt;
    public TMP_Text talkScriptTxt;
    public GameObject talkCanvas;
    [SerializeField] private int startNum;
    [SerializeField] private int endNum;
    private bool isQuestNpc;
    private Dialogue[] dialogues;
    private InteractionEvent interactionEvent;
    private QuestPoint questPoint;
    private string[] contexts;
    private string talkerName;
    private string[] eventNumber;
    private int contextIndex = 0;
    private int currentTalkIndex = 0;
    private bool isNearPlayer = false;
    private bool isNotTalking=false;

    // Start is called before the first frame update
    void Start()
    {
        interactionEvent = GetComponent<InteractionEvent>();
        dialogues = interactionEvent.GetDialogue(startNum, endNum);
        questPoint = GetComponent<QuestPoint>();
        contexts = dialogues[contextIndex].contexts;
        talkerName = dialogues[contextIndex].name;
        eventNumber = dialogues[contextIndex].number;
        Debug.Log("dialogues length: " + dialogues.Length);
        if (questPoint != null) { isQuestNpc = true; }
        else { isQuestNpc = false; }

    }
    private void Update()
    {
        SubmitPressed();
    }
    private void SubmitPressed()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            if (!isNearPlayer) { return; }
            if(!isNotTalking)
            {
                isNotTalking = true;
                StartTalk();
            }
            else
            {
                AdvanceTalk();
            }
        }
    }
    public void StartTalk()
    {
        talkCanvas.SetActive(true);
        contexts[currentTalkIndex] = contexts[currentTalkIndex].Replace("*", ",");

        talkerNameTxt.text = talkerName;
        talkScriptTxt.text = contexts[currentTalkIndex];
    }
    public void AdvanceTalk()
    {

        if (currentTalkIndex < contexts.Length - 1)
        {
            currentTalkIndex++;
            contexts[currentTalkIndex] = contexts[currentTalkIndex].Replace("*", ",");
            talkerNameTxt.text = talkerName;
            talkScriptTxt.text = contexts[currentTalkIndex];
        }
        else if (contextIndex < dialogues.Length - 1)
        {
            contextIndex++;

            currentTalkIndex = 0;
            contexts[currentTalkIndex] = contexts[currentTalkIndex].Replace("*", ",");
            contexts = dialogues[contextIndex].contexts;
            talkerName = dialogues[contextIndex].name;
            eventNumber = dialogues[contextIndex].number;

            // Reset isNotTalking to true for the next dialogue
            isNotTalking = true;

            // 대화를 시작할 때 현재 대화 인덱스를 0으로 설정
            StartTalk();
        }
        else
        {
            // 모든 대화가 끝났을 때 대화 종료
            EndTalk();
        }
    }
    public void EndTalk()
    {
        Debug.Log("EndTalk");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isNearPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPlayer = false;
        }
    }
}
