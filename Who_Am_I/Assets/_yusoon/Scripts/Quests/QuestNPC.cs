using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
   
    public string npcName;

    private void Start()
    {
        GameEventManager.instance.miscEvent.onNpcTalked += TalkNPC;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {

            GameEventManager.instance.miscEvent.NpcTalked(npcName);


        }
    }
    private void TalkNPC(string name)
    {
        Debug.Log(name + "과 대화");
    }
}
