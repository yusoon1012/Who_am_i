using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string questName;
    public string npcName;
  
    private void Start()
    {
        GameEventManager.instance.miscEvent.onNpcTalked+= TalkNPC;      
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {

            GameEventManager.instance.miscEvent.NpcTalked(npcName);


        }
    }
    private void TalkNPC(string name)
    {
        Debug.Log(npcName + "과 대화");
    }
}
