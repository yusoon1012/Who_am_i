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
    public TMP_Text nameText;
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
        dialogues= interactionEvent.GetDialogue(0,2);
        npcName = dialogues[contextIndex].name;
        contexts = dialogues[contextIndex].contexts;
        events = dialogues[contextIndex].number;
        clearMark.enabled = false;
        

    }

    public void Talk()
    {
        //chatBox.SetActive(true);
        //if (dialogues[contextIndex].name!="")
        //{
        //    nameText.text = dialogues[contextIndex].name;
        //}
        //contexts[talkIndex]= contexts[talkIndex].Replace("*", ",");
        //contextTxt.text = contexts[talkIndex];
        ////events = dialogues[talkIndex].number;
        //if (talkIndex < contexts.Length - 1)
        //{
        //    talkIndex += 1;
        //    Debug.Log("talkIndex : " + talkIndex);
        //    Debug.Log("contexts length : " + contexts.Length);


        //}
        //else if (talkIndex == contexts.Length - 1)
        //{
        //    //if (events[talkIndex]!="") 
        //    //{


        //    //    acceptButton.SetActive(true); return; 
        //    //}
        //    //Debug.Log("talkIndex"+talkIndex);
        //    //Debug.Log("contextIndex" + contextIndex);
        //    if (contextIndex < contexts.Length)
        //    {
        //        contextIndex += 1;
        //    contexts = dialogues[contextIndex].contexts;
        //    events = dialogues[contextIndex].number;
        //    }
        //    else
        //    {
        //        Debug.Log("contextIndex : " + contextIndex);
        //        Debug.Log("talkIndex == contexts.Length - 1 talkIndex : " + talkIndex);
        //        acceptButton.SetActive(true);
        //    }

        //    talkIndex = 0;
        //}
        chatBox.SetActive(true);

        if (dialogues[contextIndex].name != "")
        {
            nameText.text = dialogues[contextIndex].name;
        }

        contexts[talkIndex] = contexts[talkIndex].Replace("*", ",");
        contextTxt.text = contexts[talkIndex];

        if (talkIndex < contexts.Length - 1)
        {
            talkIndex += 1;
            Debug.Log("talkIndex: " + talkIndex);
        }
        else if (contextIndex < dialogues.Length - 1)
        {
            contextIndex += 1;
            contexts = dialogues[contextIndex].contexts;
            events = dialogues[contextIndex].number;
            talkIndex = 0;
            Debug.Log("contextIndex: " + contextIndex);
        }
        else
        {
            if(isClear==false)
            {

            acceptButton.SetActive(true);
            }
            else
            {
                Clear();
            }
            Debug.Log("Button activated");
        }
    }
    public void Clear()
    {
        chatBox.SetActive(false);
        clearMark.enabled = false;
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
            isClear = true;
            clearMark.color = Color.white;
            dialogues = interactionEvent.GetDialogue(5, 8);
            talkIndex = 0;
            contextIndex = 0;
            contexts= dialogues[contextIndex].contexts;
            acceptButton.SetActive(false);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
