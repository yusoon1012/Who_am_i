using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour
{
    DatabaseReference m_Reference;
    public List<bool> clearQuest = new List<bool>();
    public List<string> questScriptList = new List<string>();
    public List<string> clearScriptList = new List<string>();
    public Dictionary<string,int> conditionDict = new Dictionary<string,int>();
    public string mainQuestName;
   
    public void MainQuestList(string questName)
    {
        questScriptList.Clear();
        m_Reference = FirebaseDatabase.DefaultInstance.GetReference("users");
        m_Reference.Child("Quests").Child("MainQuest").Child(questName)
             .GetValueAsync().ContinueWithOnMainThread(task =>
             {
                 if(task.IsFaulted&&task.IsCanceled) { Debug.Log("읽어오기 실패"); }
                 else if(task.IsCompleted)
                 {
                     DataSnapshot snapshot=task.Result;
                     Debug.LogFormat("snapshot 의 자식데이터 갯수 : {0}", snapshot.ChildrenCount);
                     foreach(var child in snapshot.Children)
                     {
                         if(child.Key=="questname")
                         {
                            // Debug.Log("퀘스트네임 찾음");
                             mainQuestName = child.Value.ToString();
                         }
                         else if(child.Key=="questlines")
                         {
                             foreach(var scripts in child.Children)
                             {
                                // Debug.LogFormat("퀘스트라인의 키 : {0}", scripts.Key.ToString());

                                // Debug.LogFormat("퀘스트라인의 값 : {0}",scripts.Value.ToString());
                                 
                                 questScriptList.Add(scripts.Value.ToString());
                             }
                         }
                         else if(child.Key== "condition")
                         {
                             Debug.Log("조건에 들어옴");
                             foreach(var condition  in child.Children)
                             {
                                 string keyCode=condition.Key.ToString();
                                  
                                 long count = (long)condition.Value;
                                 conditionDict.Add(condition.Key.ToString(), (int)count);
                             }
                         }
                         else if( child.Key== "clearlines")
                         {
                             foreach (var script in child.Children)
                             {
                                 string scriptvalue=script.Value.ToString();
                                 clearScriptList.Add(scriptvalue);
                                 Debug.Log(scriptvalue);
                             }
                         }
                     }
                    // Debug.LogFormat("퀘스트이름 : {0}", mainQuestName);
                 }
             });
    }
    public void SavedScript()
    {
        foreach(string chat in questScriptList)
        {
            Debug.Log($"{chat}");
        }
    }
}
