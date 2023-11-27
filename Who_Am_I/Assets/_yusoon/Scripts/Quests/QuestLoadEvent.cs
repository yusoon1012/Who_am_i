using System;

public class QuestLoadEvent 
{
    public event Action onQuestLoaded;

    public void QuestLoaded()
    {
        if(onQuestLoaded != null)
        {
            onQuestLoaded();
        }
    }
}
