using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string questName;
    public string itemName;
    private TestNPC npc;
    private Dictionary<string,int> conditions = new Dictionary<string,int>();
    private int[] conditionCount;
    private string[] conditionName;
    int countIdx=0;
    // Start is called before the first frame update
    void Start()
    {
        npc = GameObject.Find(questName).GetComponent<TestNPC>();
        conditions = npc.questCondition;
        //Debug.Log(conditions);
        conditionCount=new int[conditions.Count];
        conditionName=new string[conditions.Count];
       
        foreach(KeyValuePair<string,int> kvp in conditions)
        {
           if(kvp.Key==itemName)
            {
                Debug.LogFormat("필요한 아이템 : {0} 필요한 갯수 {1}/{2}",kvp.Key ,conditionCount[countIdx], kvp.Value);
                conditionName[countIdx] = kvp.Key;
            }
           countIdx++;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            int tempIdx = 0;
            foreach(KeyValuePair<string,int> kvp in conditions)
            {
                if (itemName == kvp.Key)
                {
                    conditionCount[tempIdx] += 1;
                    Debug.Log(conditionCount[tempIdx]);
                }
                if (conditionName[tempIdx] == kvp.Key)
                {
                    if (conditionCount[tempIdx]==kvp.Value)
                    {
                        npc.isClear = true;
                        npc.isAccept = false;
                    }
                }
                    tempIdx++;
            }
           
           
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
