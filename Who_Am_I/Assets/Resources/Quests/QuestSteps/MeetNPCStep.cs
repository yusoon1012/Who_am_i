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
       
        if (!talkedNPCs.Contains(name))
        {
            talkedNPCs.Add(name);
            Debug.Log(name + "과 대화");


            npcTalkCount++;
            UpdateState();

            
            if (npcTalkCount >= 1)
            {
               FinishQuestStep();
            }
        }
    }
    private void UpdateState()
    {
        string state = npcTalkCount.ToString();
        ChangeState(state);
        Debug.Log("NPCstep state : " + state);
    }
    protected override void SetQuestStepState(string state)
    {
        this.npcTalkCount=System.Int32.Parse(state);
        UpdateState();
    }

}
