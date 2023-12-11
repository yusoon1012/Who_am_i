using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    public MiscEvent miscEvent;
    public QuestEvent questEvent;
    public QuestLoadEvent questLoadEvent;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        miscEvent = new MiscEvent();
        questLoadEvent=new QuestLoadEvent();
        questEvent = new QuestEvent();
    }
}
