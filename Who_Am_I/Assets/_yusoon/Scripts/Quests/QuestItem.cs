using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    private void Start()
    {
        GameEventManager.instance.miscEvent.onItemCollected += CollectedItem;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameEventManager.instance.miscEvent.ItemCollected(itemName);
        }
    }
    private void CollectedItem(string name)
    {
        Debug.Log(name + "획득");
    }

}
