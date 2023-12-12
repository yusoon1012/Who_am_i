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
        talkerNameTxt.text = talkerName;
        talkScriptTxt.text = contexts[currentTalkIndex];
    }
    public void AdvanceTalk()
    {
        if(currentTalkIndex>contexts.Length-2)
        {
            contextIndex++;
            currentTalkIndex = 0;
            contexts = dialogues[contextIndex].contexts; 

            Debug.Log(contexts);
        }
        else if(currentTalkIndex < contexts.Length - 1)
        {
        currentTalkIndex++;
        talkerNameTxt.text = talkerName;
        talkScriptTxt.text = contexts[currentTalkIndex];
        

        }
    }
    public void EndTalk()
    {

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
