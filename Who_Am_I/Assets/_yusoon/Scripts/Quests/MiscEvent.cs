using System;

public class MiscEvent
{
    public event Action onItemCollected;

    public void ItemCollected()
    {
        if(onItemCollected != null) 
        {
            onItemCollected();
        }
    }
}
