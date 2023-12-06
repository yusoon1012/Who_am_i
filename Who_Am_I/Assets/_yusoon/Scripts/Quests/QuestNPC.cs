using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    public string npcName;
    public int startNumber;
    public int endNumber;
    public GameObject chatUI;
    private InteractionEvent interactionEvent;
    public Dialogue[] dialogues;
    private int talkCount=0;
    private int contextIndex = 0;
    private string[] contexts;
    public TMP_Text chatText;
    public TMP_Text nameText;
    private bool isTalkEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        
        interactionEvent = GetComponent<InteractionEvent>();
        dialogues = interactionEvent.GetDialogue(startNumber, endNumber);
        npcName = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
           
        }
    }
    public void Chat()
    {
        chatUI.SetActive(true);
        nameText.text = dialogues[talkCount].name;
        contexts = dialogues[contextIndex].contexts;
        chatText.text = contexts[talkCount];
        if(talkCount<contexts.Length-1&&contexts.Length-1!=0)
        {
            talkCount += 1;
        }
        else
        {
            if(isTalkEnd)
            {
            EndChat();

            }
            else
            {
                isTalkEnd = true;
            }
        }
    }
    public void EndChat()
    {
        
        QuestManager.Instance.CheckNpcTalked(npcName);
        chatUI.SetActive(false);
        isTalkEnd = false;
        talkCount = 0;
    }

    private void Talk(string name)
    {
        Debug.Log(name + "Talk");   
    }

}
