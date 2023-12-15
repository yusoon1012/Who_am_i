using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestData info;

    public QuestState state;

    private int currentQuestStepIndex;

    private QuestStepState[] queststepStates;
    public Quest(QuestData questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.NOT_MET;
        this.currentQuestStepIndex = 0;
        this.queststepStates = new QuestStepState[info.questStepPrefabs.Length];
        for(int i=0;i<queststepStates.Length;i++)
        {
            queststepStates[i] = new QuestStepState();
        }
    }
    public Quest(QuestData questData,QuestState _state,int _currentQuestStepIndex, QuestStepState[] _questStepStates)
    {
        this.info = questData;
        this.state = _state;
        this.currentQuestStepIndex = _currentQuestStepIndex;
        this.queststepStates = _questStepStates;
        if(this.queststepStates.Length!=this.info.questStepPrefabs.Length)
        {

        }
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
            questStep.InitializeQeustStep(info.id, currentQuestStepIndex, queststepStates[currentQuestStepIndex].state);
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
    public void StoreQuestStepState(QuestStepState queststepState_,int stepIndex_)
    {
        if(stepIndex_<queststepStates.Length)
        {
            queststepStates[stepIndex_].state = queststepState_.state;
        }
        else
        {
            Debug.LogWarning("퀘스트 스텝 데이터에 접근하려했지만 인덱스 범위를 벗어났습니다. QuestId :" + info.id + "StepIndex : " + stepIndex_);
        }
    }
    public QuestInfo GetQuestInfo()
    {
        return new QuestInfo(state, currentQuestStepIndex, queststepStates);
    }
}
