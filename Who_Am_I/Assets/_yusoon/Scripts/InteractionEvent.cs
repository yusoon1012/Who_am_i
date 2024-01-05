using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue;
    [SerializeField] public string questName;

    public Dialogue[] GetDialogue(int firstline,int endline)
    {
        //Debug.Log("InteractionEvent GetDialogue ");
        
        dialogue.dialogues = DialogueManager.instance.GetDialogue(firstline, endline);
        return dialogue.dialogues;
    }

}
