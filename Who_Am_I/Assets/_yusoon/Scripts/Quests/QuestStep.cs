using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    public void InitializeQeustStep(string questId_,int stepIndex_,string questStepState)
    {
        this.questId = questId_;
        this.stepIndex = stepIndex_;
        if(questStepState!=null&&questStepState!="")
        {
            SetQuestStepState(questStepState);
        }

    }
    protected void  FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;
            GameEventManager.instance.questEvent.AdvanceQuest(questId);
            //해당 퀘스트ID를 가진 퀘스트 완료상태로 변경하는 이벤트 호출
           // Destroy(this.gameObject);
        }
    }
    protected void ChangeState(string newState)
    {
        GameEventManager.instance.questEvent.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
    }

    protected abstract void SetQuestStepState(string state);

}
