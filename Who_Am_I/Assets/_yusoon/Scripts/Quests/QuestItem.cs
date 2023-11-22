using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string questName;
    public string itemName;
   
    private void Awake()
    {


    }
    private void Start()
    {

        if (GameEventManager.instance != null && GameEventManager.instance.miscEvent != null)
        {
            GameEventManager.instance.miscEvent.onItemCollected += CollectItem;
           // Debug.Log("성공");
        }
        else
        {
            Debug.LogWarning("GameEventManager.instance 또는 GameEventManager.instance.miscEvent가 null입니다.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {

            GameEventManager.instance.miscEvent.ItemCollected();


        }
    }
    public void CollectItem()
    {
        Debug.Log(itemName + "획득");
        Debug.Log(questName);
    }
}
