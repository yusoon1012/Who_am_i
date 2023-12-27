using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meen_QuestManager : MonoBehaviour
{
    public static Meen_QuestManager instance;

    public GameObject onNpcCheck { get; set; } = default;

    private QuestsMain questInfo = new QuestsMain();

    private int questDialogCheck = default;

    private bool doingQuestCheck = false;

    Dictionary<string, QuestsMain> questDic = new Dictionary<string, QuestsMain>();

    void Awake()
    {
        if (instance == null || instance == default) { instance = this; DontDestroyOnLoad(instance.gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        questDic.Add("QuestNPC", new Quest000());

        questDialogCheck = 0;
    }

    public void QuestTypeCheck()
    {
        if (doingQuestCheck == false)
        {
            GetQuestInfomation(onNpcCheck.name, out questInfo);

            if (questInfo.questType == QuestType.CONDITION)
            {
                QuestCountCheck();
            }
        }
        else
        {
            if (questInfo != null)
            {
                QuestCountCheck();
            }
        }
    }

    private void QuestCountCheck()
    {
        if (questInfo.questState == QuestState.NONE)
        {
            ShowDialogs(0);
        }
        else if (questInfo.questState == QuestState.GOING)
        {
            LootCountCheck();
        }
        else if (questInfo.questState == QuestState.DONE)
        {
            ShowDialogs(3);
        }
    }

    public void LootCountCheck()
    {
        string lootName = questInfo.lootItem;
        int lootCount = questInfo.lootCount;

        ItemManager.instance.ItemTypeCheck(lootName, out int itemType);

        ItemManager.instance.QuestLootItemCheck(lootName, itemType, lootCount, out bool lootCheck);

        if (lootCheck == true)
        {
            ShowDialogs(2);
        }
        else
        {
            ShowDialogs(1);
        }
    }

    private void ShowDialogs(int questCount)
    {
        switch (questCount)
        {
            case 0:
                if (questInfo.questDialogs[questCount, questDialogCheck] != null)
                {
                    Debug.LogFormat("{0}", questInfo.questDialogs[questCount, questDialogCheck]);

                    questDialogCheck += 1;
                }
                else
                {
                    questInfo.questState = QuestState.GOING;

                    questDialogCheck = 0;

                    Debug.Log("대화 끝");
                }
                break;
            case 1:
                if (questInfo.questDialogs[questCount, questDialogCheck] != null)
                {
                    Debug.LogFormat("{0}", questInfo.questDialogs[questCount, questDialogCheck]);

                    questDialogCheck += 1;
                }
                else
                {
                    questDialogCheck = 0;

                    Debug.Log("대화 끝");
                }
                break;
            case 2:
                if (questInfo.questDialogs[questCount, questDialogCheck] != null)
                {
                    Debug.LogFormat("{0}", questInfo.questDialogs[questCount, questDialogCheck]);

                    questDialogCheck += 1;
                }
                else
                {
                    questInfo.questState = QuestState.DONE;

                    questDialogCheck = 0;

                    Debug.Log("대화 끝");
                }
                break;
            case 3:
                if (questInfo.questDialogs[questCount, questDialogCheck] != null)
                {
                    Debug.LogFormat("{0}", questInfo.questDialogs[questCount, questDialogCheck]);

                    questDialogCheck += 1;
                }
                else
                {
                    questDialogCheck = 0;

                    Debug.Log("대화 끝");
                }
                break;
            default:
                break;
        }
    }

    private QuestsMain GetQuestInfomation(string npcName, out QuestsMain questInfo)
    {
        if (questDic.ContainsKey(npcName))
        {
            questInfo = questDic[npcName];
        }
        else
        {
            questInfo = null;
        }

        return questInfo;
    }

}
