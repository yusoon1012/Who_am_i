using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;

    public void InitializeQeustStep(string questId_)
    {
        this.questId = questId_;

    }
    protected void  FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;
            GameEventManager.instance.questEvent.AdvancedQuest(questId);
            Destroy(this.gameObject);
        }
    }
}
