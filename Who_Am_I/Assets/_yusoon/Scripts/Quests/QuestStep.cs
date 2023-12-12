using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    public void InitializeQeustStep(string questId_,int stepIndex_)
    {
        this.questId = questId_;
        this.stepIndex = stepIndex_;

    }
    protected void  FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;
            GameEventManager.instance.questEvent.AdvanceQuest(questId);
            Destroy(this.gameObject);
        }
    }
    protected void ChangeState(string newState)
    {
        GameEventManager.instance.questEvent.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
    }
}
