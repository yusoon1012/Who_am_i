using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{
    public int prevQuestID;
    public int nextQuestID;
    public int questID;
    public string questName;
    public Dictionary<string,int> questCondition;
    public List<string> questScript;
    public QuestState questState;
    
    public void AddScript(List<string> script)
    {
        questScript=script;
    }
    public void AddCondition(Dictionary<string,int> condition)
    {
        questCondition=condition;
    }
    public enum QuestState
    {
        CAN_START,
        IN_PROGRESS,
        CAN_FINISH,
        IS_FINISH
    }
}
