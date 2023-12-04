using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class Select
{
    [Tooltip("이벤트 번호")]
    public string[] eventNumber;
    [Tooltip("대사 내용")]
    public string[] contexts;
    [Tooltip("점프 번호")]
    public string[] jumpNumber;
}
[System.Serializable]
public class SelectEvent
{
    public string name;
    public Vector2 line;
    public Select[] selects;
}