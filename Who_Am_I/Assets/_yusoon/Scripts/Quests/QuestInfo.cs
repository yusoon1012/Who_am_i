using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInfo
{
    public QuestState state;
    public int questStepIndex;
    public QuestStepState[] queststepStates;

    public QuestInfo(QuestState state_,int questStepIndex_, QuestStepState[] queststepStates_)
    {
        this.state = state_;
        this.questStepIndex = questStepIndex_;
        this.queststepStates = queststepStates_;
    }
    
}
