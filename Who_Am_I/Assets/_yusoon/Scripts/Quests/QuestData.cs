using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{   
   [field: SerializeField] public string id { get; private set; }
    public string questName;
    public int questIndex;
    public QuestData[] questPrerequisites;
    public GameObject[] questStepPrefabs;
}
