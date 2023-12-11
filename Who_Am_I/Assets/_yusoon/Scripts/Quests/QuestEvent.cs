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

    public event Action<string> onAdvancedQuest;
    public void AdvancedQuest(string id)
    {
        if (onAdvancedQuest != null)
        {
            onAdvancedQuest(id);
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

    public event Action<int> onQuestIndexChange;
    public void QuestIndexChange(int index_)
    {
        if(onQuestIndexChange!=null)
        {
            onQuestIndexChange(index_);
        }
    }
}
