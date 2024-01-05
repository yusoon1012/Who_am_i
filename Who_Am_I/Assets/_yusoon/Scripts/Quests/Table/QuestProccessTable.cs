using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProccessTable
{
    [Tooltip("퀘스트ID")]
   public int questId;
    [Tooltip("퀘스트지역")]
    public int questRocation;
    [Tooltip("퀘스트 그룹ID")]
   public string questGroup;
    [Tooltip("진행도 그룹")]
    public int proccessGroup;
    [Tooltip("진행도 카운트")]
    public int proccessCount;
    [Tooltip("대사출력시점")]
    public string scriptType;
    [Tooltip("NPC이름")]
    public string npcName;
    [Tooltip("스크립트 코드")]
    public string scriptCode;
    [Tooltip("조건1 수량")]
    public int condition1_Count;
    [Tooltip("조건1 MBTI 게이지 문자")]
    public string condition1_Mbti;
    [Tooltip("조건1 MBTI 게이지 증가값")]
    public int condition1_Value;
    [Tooltip("조건2 MBTI 게이지 증가값")]
    public int condition2_Count;
    [Tooltip("조건2 MBTI 게이지 문자")]
    public string condition2_Mbti;
    [Tooltip("조건2 수량")]
    public int condition2_Value;
}
