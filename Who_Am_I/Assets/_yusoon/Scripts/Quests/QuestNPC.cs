using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
   
    public string npcName;
    public GameObject questIcon;
    private void Start()
    {
        GameEventManager.instance.miscEvent.onNpcTalked += TalkNPC;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameEventManager.instance.miscEvent.NpcTalked(npcName);
            questIcon.SetActive(false);
        }
    }
    private void TalkNPC(string name)
    {
    }
}
