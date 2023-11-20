using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TestNPC : MonoBehaviour
{
    public QuestData QuestData;
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
    private int textIdx = 0;
    private List<string> textList = new List<string>();
    private List<string> clearList = new List<string>();
    public Dictionary<string, int> questCondition;
    QuestList questlist = new QuestList();
    Testpilot player;
    string conditionStr;
    int conditionCount;
    private void Awake()
    {
        questlist.MainQuestList(initQuest);
        QuestListUpdate();
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }


    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isClear = true;
            isAccept = false;
        }
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

    private void QuestListUpdate()
    {
        Debug.Log("퀘스트 리스트 업데이트");
        textList = questlist.questScriptList;     
        questCondition = questlist.conditionDict;
        clearList = questlist.clearScriptList;
        for(int i=0;i<clearList.Count;i++) 
        {
            Debug.Log(clearList[i]);
        }
    }
    public void QuestConditionInfo()
    {

       
        foreach(var key in questCondition.Keys)
        {
            Debug.Log($"{key}");
            Debug.Log(key.GetType());
            conditionStr = key;
            Debug.Log($"{conditionStr}");
        }
        foreach (var value in questCondition.Values)
        {
            Debug.Log($"{value}");
           Debug.Log( value.GetType());
            conditionCount = value;
        }

        Debug.LogFormat("{0} / {1}", conditionStr, conditionCount);
    }
    private void NextIndex()
    {
        chatText.text = textList[textIdx];
        textIdx += 1;
    }
    public void AcceptQuest()
    {
        acceptImg.enabled = false;
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
        textIdx += 1;
    }
    public void ClearQuest()
    {
        clearImg.enabled = false;
        isTalking = false;
        chatObj.SetActive(false);
    }
}
