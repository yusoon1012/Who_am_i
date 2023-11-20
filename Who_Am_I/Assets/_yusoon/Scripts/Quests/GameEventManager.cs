using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    public MiscEvent miscEvent;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        miscEvent = new MiscEvent();
    }
}
