using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetNPCStep : QuestStep
{
    private HashSet<string> talkedNPCs = new HashSet<string>();
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
        // Check if the NPC has not been talked to yet
        if (!talkedNPCs.Contains(name))
        {
            talkedNPCs.Add(name);

            // Increment the talk count
            npcTalkCount++;

            // Check if the required number of unique NPCs have been talked to
            if (npcTalkCount == npcTalkComplete)
            {
                FinishQuestStep();
            }
        }
    }

}
