using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetNPCStep : QuestStep
{
    private int npcTalkCount = 0;
    private int npcTalkComplete = 3;

    private void OnEnable()
    {
        GameEventManager.instance.miscEvent.onNpcTalked += NpcTalk;
    }
    private void OnDisable()
    {
        GameEventManager.instance.miscEvent.onNpcTalked -= NpcTalk;

    }

    private void NpcTalk(string name)
    {
        if(npcTalkCount<npcTalkComplete)
        {
            npcTalkCount += 1;
        }
        if(npcTalkCount==npcTalkComplete)
        {
            FinishQuestStep();
        }
    }
}
