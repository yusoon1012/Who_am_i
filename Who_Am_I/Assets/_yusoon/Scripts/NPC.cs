using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string questName;
    public Image startMark;
    public Image clearMark;
    public GameObject chatBox;
    public GameObject acceptButton;
    public Dialogue[] dialogues;
    public TMP_Text contextTxt;
    private int talkIndex = 0;
    private int contextIndex = 0;
    private bool isAccept = false;
    private bool isClear = false;
    InteractionEvent interactionEvent;
    string[] contexts;
    string npcName;
    string[] events;
    // Start is called before the first frame update
    void Start()
    {
        interactionEvent = GetComponent<InteractionEvent>();
        GameEventManager.instance.miscEvent.onClearbleQuest += ClearAble;
        dialogues= interactionEvent.GetDialogue();
        npcName = dialogues[contextIndex].name;
        contexts = dialogues[contextIndex].contexts;
        events = dialogues[contextIndex].number;
        clearMark.enabled = false;


    }

    public void Talk()
    {
        chatBox.SetActive(true);
        contexts[talkIndex]= contexts[talkIndex].Replace("*", ",");
        contextTxt.text = contexts[talkIndex];
        events = dialogues[talkIndex].number;
        if (talkIndex < contexts.Length - 1)
        {
            talkIndex += 1;
        }
        else if (talkIndex == contexts.Length - 1)
        {
            //if (events[talkIndex]!="") 
            //{
                

            //    acceptButton.SetActive(true); return; 
            //}
            Debug.Log("talkIndex"+talkIndex);
            Debug.Log("contextIndex" + contextIndex);
            if(contextIndex+1< contexts.Length) 
            {
            contextIndex += 1;
            contexts = dialogues[contextIndex].contexts;
            events = dialogues[contextIndex].number;
            }
            else
            {
                acceptButton.SetActive(true); 
            }
            
            talkIndex = 0;
        }
    }
    public  void Accept()
    {
        isAccept = true;
        chatBox.SetActive(false);
        startMark.enabled = false;
        clearMark.enabled = true;
        clearMark.color = Color.gray;

    }
    public void ClearAble(string name)
    {
        Debug.Log("ClearAble method called for quest: " + name);
        if (name== questName)
        {
            Debug.Log("ClearAble"+ questName);
            
            clearMark.color = Color.white;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
