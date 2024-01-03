using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestMainTable
{
    [Tooltip("퀘스트이름")]
    public string questName;
    [Tooltip("퀘스트목표")]
    public string questTarget;
    [Tooltip("퀘스트지역")]
    public string questRocation;
    [Tooltip("선행퀘스트")]
    public int previousQuest;
    [Tooltip("진행테이블코드")]
    public string proccessCode;
    [Tooltip("퀘스트타입")]
    public string questType;
    [Tooltip("Value 갯수")]
    public int valueCount;
    [Tooltip("Value 이름")]
    public string valueName;
    [Tooltip("보상 이름")]
    public string resultName;
    [Tooltip("보상 카운트")]
    public int resultCount;
    [Tooltip("퀘스트 진행테이블")]
    public QuestProccessTable[] questProccessTables;
}
