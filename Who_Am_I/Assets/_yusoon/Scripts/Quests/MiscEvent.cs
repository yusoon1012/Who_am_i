using System;

public class MiscEvent
{
    public event Action onItemCollected;
    public event Action<string> onNpcTalked;
    public event Action<string> onClearbleQuest;
    public void ItemCollected()
    {
        if(onItemCollected != null) 
        {
            onItemCollected();
        }
    }
    public void NpcTalked(string name)
    {
        if (onNpcTalked != null)
        {
            onNpcTalked(name);
        }
    }

    public void QuestClearble(string questName)
    {

        if(onClearbleQuest!= null)
        {
            onClearbleQuest(questName);
           
        }
    }
}
