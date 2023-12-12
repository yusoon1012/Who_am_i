using System;

public class QuestEvent
{
    public event Action<string> onStartQuest;
    public void StartQuest(string id)
    {
        if(onStartQuest!=null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onAdvanceQuest;
    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public event Action<string> onFinishQuest;
    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }
    public event Action<string,int,QuestStepState> onQuestStepStateChange;
    public void QuestStepStateChange(string id, int stepIndex, QuestStepState stepState)
    {
        if (onQuestStepStateChange != null)
        {
            onQuestStepStateChange(id,stepIndex,stepState);
        }
    }
    public event Action<int> onQuestIndexChange;
    public void QuestIndexChange(int index_)
    {
        if(onQuestIndexChange!=null)
        {
            onQuestIndexChange(index_);
        }
    }
}
