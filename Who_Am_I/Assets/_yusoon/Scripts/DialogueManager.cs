using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private string csv_FileName;

    Dictionary<int, Dialogue> dialogueDict = new Dictionary<int, Dialogue>();
    public static bool isFinish = false;

    private void Awake()
    {
        if(instance==null)
        {
            Debug.Log("DialogueManager Awake");
            instance = this;
           
          
        }
    }
    public void GetParsing(string name)
    {
        DialogueParse dialogueParse = GetComponent<DialogueParse>();
        Dialogue[] dialogues = dialogueParse.Parse(name);
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDict.Add(i, dialogues[i]);
        }
        isFinish = true;
    }
    public Dialogue[] GetDialogue(int startNum, int endNum)
    {
        

        List<Dialogue> dialogueList = new List<Dialogue>();

        for(int i=1;i<=endNum-startNum;i++)
        {
            dialogueList.Add(dialogueDict[startNum + i]);
        }
        return dialogueList.ToArray();
    }

    

}
