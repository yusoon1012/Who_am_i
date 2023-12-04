using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{   
   [field: SerializeField] public string id { get; private set; }
    public string questName;
    public QuestData[] questPrerequisites;
    public GameObject[] questStepPrefabs;
    public QuestState questState;
    public enum QuestState
    {
        CAN_START,
        IN_PROGRESS,
        CAN_FINISH,
        IS_FINISH
    }
}
