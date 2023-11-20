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
    public bool[] questCondition;
    public List<string> questScript;
    public QuestState questState;
  
    public enum QuestState
    {
        CAN_START,
        IN_PROGRESS,
        CAN_FINISH,
        IS_FINISH
    }
}
