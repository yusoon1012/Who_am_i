using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TestNPC : MonoBehaviour
{
    public QuestData questData;
    public TMP_Text chatText;
    public TMP_Text questTitleTxt;
    public TMP_Text conditionText;
    public Image acceptImg;
    public Image clearImg;
    public GameObject chatObj;
    public string initQuest;
    public bool isTalking = false;
    public bool isAccept = false;
    public bool isClear = false;
    private bool[] conditionClears;
    private int textIdx = 0;
    private List<string> textList = new List<string>();
    private List<string> clearList = new List<string>();
    public Dictionary<string, int> questCondition;
    public Dictionary<string, int> currentCondition;
    QuestList questlist;
    Testpilot player;
    string conditionStr;
    int conditionIndex = 0;
    int conditionCount;
    int currentConditionCount;
    private void Awake()
    {
        questlist = new QuestList();
        questlist.MainQuestList(initQuest);
        


    }
    // Start is called before the first frame update
    void Start()
    {
        
        GameEventManager.instance.questLoadEvent.onQuestLoaded += QuestListUpdate;
        GameEventManager.instance.miscEvent.onItemCollected += AddQuestItem;

    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
           // Debug.Log(questData.questState);
        }
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(isAccept==false&&isClear==false)
                {

                if (textIdx == textList.Count)
                {
                    AcceptQuest();
                    return;

                }
                NextIndex();
                }
                if(isAccept==false&&isClear==true)
                {
                    if(textIdx==clearList.Count)
                    {
                        ClearQuest();
                        return;
                    }
                    
                    ClearIndex();
                }
            }

        }
        if(isClear)
        {
            clearImg.color = Color.white;
        }
        
    }
    public void AddQuestItem()
    {
        if(isAccept == false)
        {
            return;
        }
        if(isClear)
        {
            return;
        }
        Debug.Log("AddQuestItem 실행");
        foreach(KeyValuePair<string, int> item1 in questCondition)
        {
            Debug.Log("foreache문 들어옴");
            string key = item1.Key;
            int value1, value2;
            if(currentCondition.TryGetValue(key, out value2))
            {
                value1 = item1.Value;
                if (value1 == value2)
                {
                    Debug.Log($"키 '{key}'의 값이 두 딕셔너리에 존재하며 같습니다.");
                    conditionClears[conditionIndex] = true;
                    if(conditionClears.Length-1>conditionIndex)
                    {
                    conditionIndex += 1;

                    }
                    if(conditionClears.All(x => x))
                    {
                        Debug.Log("모든 조건 클리어");
                        questData.questState = QuestData.QuestState.CAN_FINISH;
                        isAccept = false;
                        isClear = true;

                    }
                }
                else
                {
                    Debug.Log($"키 '{key}'의 값이 두 딕셔너리에 존재하며 다릅니다.");
                    currentCondition[key] += 1;
                    Debug.Log($"키 '{key}'의 값이 증가되었습니다. 현재 값: {currentCondition[key]}");

                }
                
            }
            else
            {
                Debug.Log($"키 '{key}'의 값이 두 딕셔너리에 존재하지 않습니다.");

            }
        }
        foreach (KeyValuePair<string, int> item1 in questCondition)
        {
            string key = item1.Key;
            int value1, value2;
            if (currentCondition.TryGetValue(key, out value2))
            {
                value1 = item1.Value;
                if (value1 == value2)
                {
                    Debug.Log($"키 '{key}'의 값이 두 딕셔너리에 존재하며 같습니다.");
                    conditionClears[conditionIndex] = true;
                    if (conditionClears.Length - 1 > conditionIndex)
                    {
                        conditionIndex += 1;

                    }
                    if (conditionClears.All(x => x))
                    {
                        Debug.Log("모든 조건 클리어");
                        questData.questState = QuestData.QuestState.CAN_FINISH;
                        isAccept = false;
                        isClear = true;

                    }
                }
               
            }
        }
            if (conditionClears.All(x => x))
        {
            Debug.Log("모든 조건 클리어");
            questData.questState = QuestData.QuestState.CAN_FINISH;
            isAccept = false;
            isClear = true;

        }
    }
    public void MeetNpc()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            player = other.GetComponent<Testpilot>();
            if (player != null)
            {
                if (player.isAction)
                {
                    if (isTalking) { return; }
                    if (isAccept) { return; }

                    if(isClear==false) 
                    {
                         NextIndex();
                    }
                    else
                    {
                        
                        ClearIndex();
                    }
                    questTitleTxt.text = questlist.mainQuestName.ToString();
                    chatObj.SetActive(true);
                    isTalking = true;

                }
            }
        }
    }

    public void QuestListUpdate()
    {
        Debug.Log("퀘스트 리스트 업데이트");
        textList = questlist.questScriptList;
        //Debug.Log("questlist Count : " + questlist.conditionDict.Count);

        questCondition = questlist.conditionDict;
        Debug.Log("questCondition count  :"+questCondition.Count);
        //Debug.Log("TEST NPC)questlist.conditionDict Count : " + questlist.conditionDict.Count);
        //Debug.Log("TEST NPC) questlist.questScriptList Count : " + questlist.questScriptList);
        currentCondition = questCondition.ToDictionary(kv => kv.Key, kv => 0); // Set all values to 0
        Debug.Log("currentCondition Count : "+currentCondition.Count);
        conditionClears=new bool[questCondition.Count];
        foreach (var cond in currentCondition)
        {
            Debug.Log("cond key : "+cond.Key);
            Debug.Log("cond value : "+cond.Value);
        }
        clearList = questlist.clearScriptList;
        
       
    }
    public void QuestConditionInfo()
    {

       
        foreach(var key in questCondition.Keys)
        {
            //Debug.Log($"{key}");
           // Debug.Log(key.GetType());
            conditionStr = key;
           // Debug.Log($"{conditionStr}");
        }
        foreach (var value in questCondition.Values)
        {
           // Debug.Log($"{value}");
          // Debug.Log( value.GetType());
            conditionCount = value;
        }
        foreach (var value in currentCondition.Values)
        {
           // Debug.Log($"{value}");
           // Debug.Log(value.GetType());
            currentConditionCount = value;
        }

      //  Debug.LogFormat("{0}  {1} / {2}", conditionStr, conditionCount, currentConditionCount);
    }
    private void NextIndex()
    {
        chatText.text = textList[textIdx];
        textIdx += 1;
    }
    public void AcceptQuest()
    {
        acceptImg.enabled = false;
        questData.questState = QuestData.QuestState.IN_PROGRESS;
        clearImg.enabled = true;
        clearImg.color = Color.gray;
        chatObj.SetActive(false);
        isTalking = false;
        isAccept = true;
        textIdx = 0;
    }
    private void ClearIndex()
    {
        chatText.text = clearList[textIdx];
        if(textIdx<clearList.Count)
        {
        textIdx += 1;

        }
    }
    public void ClearQuest()
    {
        clearImg.enabled = false;
        isTalking = false;
        chatObj.SetActive(false);
    }
}
