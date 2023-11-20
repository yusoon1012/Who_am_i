using System;

public class MiscEvent
{
    public Action onItemCollected;

    public void ItemCollected()
    {
        if(onItemCollected != null) 
        {
            onItemCollected();
        }
    }
}
