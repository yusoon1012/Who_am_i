using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsMain
{
    public int questNum = default;

    public string[,] questDialogs = new string[30,30];

    public string doneNpc = default;

    public string lootItem = default;

    public int lootCount = default;

    public string completeItem = default;

    public int completeCount = default;

    public QuestType questType;

    public QuestState questState;

    public virtual void Init()
    {
        /* Empty */
    }
}

public enum QuestType
{
    DIALOG,
    INTERACTION,
    CONDITION
}