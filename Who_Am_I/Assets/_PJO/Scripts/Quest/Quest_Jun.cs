using System.Collections.Generic;
using UnityEngine;

public enum QuestState_Jun
{
    NOTACCEPTED,
    ACCEPTED,
    PROGRESSED,
    COMPLETED
}

[CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
public class Quest_Jun : ScriptableObject
{
    public QuestState_Jun currentProgress;     // 퀘스트 현재 진행상황
    public string questTitle;                  // 퀘스트 제목
    public string questGoal;                   // 퀘스트 목적
    public List<int> targetValues;             // 목표 값
    public List<int> currentValues;            // 현재 값
    public bool? isMBTIConditions;             // MBTI 조건에 충족 했는지
    public string compensationItem;            // 보상 아이템
    public string trueMBTI;                    // MBTI 조건에 충족 했을때 증가할 값
    public string falseMBTI;                   // MBTI 조건에 충족하지 못했을때 증가할 값
    public List<string> npc;                   // 연관된 NPC
}