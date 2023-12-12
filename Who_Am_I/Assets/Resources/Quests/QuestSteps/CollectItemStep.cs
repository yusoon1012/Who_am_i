using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemStep : QuestStep
{
    private int collectItemCount = 0;
    private int completeCount = 5;

    private void OnEnable()
    {
        GameEventManager.instance.miscEvent.onItemCollected += CollectItem;
    }
    private void OnDisable()
    {
        GameEventManager.instance.miscEvent.onItemCollected -= CollectItem;

    }
    public void CollectItem(string name)
    {
        if(name=="딸기")
        {
        collectItemCount++;

        }
        if(collectItemCount>=completeCount)
        {
            FinishQuestStep();
        }
    }
}
