using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue;
    [SerializeField] private string questName;

    public Dialogue[] GetDialogue()
    {
        Debug.Log("InteractionEvent GetDialogue ");
        DialogueManager.instance.GetParsing(questName);
        dialogue.dialogues = DialogueManager.instance.GetDialogue((int)dialogue.line.x,(int)dialogue.line.y);
        return dialogue.dialogues;
    }

}
