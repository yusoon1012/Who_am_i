using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestData info;

    public QuestState state;

    private int currentQuestStepIndex;

    public Quest(QuestData questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.NOT_MET;
        this.currentQuestStepIndex = 0;
    }
    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }
    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex<info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parent)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab!=null)
        {
           QuestStep questStep= Object.Instantiate<GameObject>(questStepPrefab, parent).GetComponent<QuestStep>();
            questStep.InitializeQeustStep(info.id);
        }
    }
    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if(CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("퀘스트 프리팹을 가져오려했지만 퀘스트 인덱스 범위를 벗어났습니다. " +
                "퀘스트ID : " + info.id + "퀘스트 스텝 인덱스 : " + currentQuestStepIndex);
        }
        return questStepPrefab;
    }
}
