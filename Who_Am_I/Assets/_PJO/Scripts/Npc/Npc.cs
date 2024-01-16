using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    #region private members
    public string id;                       // NPC ID 값
    private GameObject player;              // 플레이어
    private Coroutine inPlayerCoroutine;    // 콜라이더 안에 플레이어가 있는지 체크하는 코루틴
    private Coroutine talkCoroutine;        // 대화를 하는 코루틴
    private string defaultTalk;             // 기본적으로 가지고있는 대사
    private List<string> startTalks;        // 퀘스트 시작 대사
    private List<string> progressTalks;     // 퀘스트 진행 대사
    private List<string> completeTalks;     // 퀘스트 완료 대사
    private bool isInteraction;             // 1번이라도 상호작용 하였는가?
    #endregion

    #region Initialization and Setup
    private void Start()
    {
        InitializationSetup();
        AddEvent();
    }

    private void InitializationSetup()
    {
        id = QuestManager_Jun.instance.Return_Id(gameObject.name);
        defaultTalk = QuestManager_Jun.instance.Return_DefaultTalk(gameObject.name);
        isInteraction = false;
    }

    private void AddEvent()
    {
        QuestManager_Jun.setEvent += SetTalks;
        SetTalks((10000 + 1 + QuestManager_Jun.instance.currentQuest).ToString());
    }
    #endregion

    #region Talks Update
    private void SetTalks(string _questId)
    {
        isInteraction = false;
        startTalks = null;
        progressTalks = null;
        completeTalks = null;
        startTalks = QuestManager_Jun.instance.QuestTalk(_questId, "CAN_START", id);
        progressTalks = QuestManager_Jun.instance.QuestTalk(_questId, "IN_PROGRESS", id);
        completeTalks = QuestManager_Jun.instance.QuestTalk(_questId, "CAN_COMPLETE", id);
    }
    #endregion

    #region Coroutine and Check
    private void StopAllCoroutine()
    {
        if (inPlayerCoroutine != null) { StopCoroutine(inPlayerCoroutine); }
        if (talkCoroutine != null) { StopCoroutine(talkCoroutine); }
    }

    private void StartInPlayerCoroutine()
    {
        StopAllCoroutine();

        inPlayerCoroutine = StartCoroutine(InPlayer());
    }

    private IEnumerator InPlayer()
    {
        while (player != null)
        {
            if (Input.GetKeyDown(KeyCode.X))        // TODO: 대화시작
            {
                transform.LookAt(player.transform);
                StartTalkCoroutine();
                yield break;
            }

            yield return null;
        }
    }

    private void StartTalkCoroutine()
    {
        StopAllCoroutine();

        QuestState_Jun state = QuestManager_Jun.instance.FindCurrentQuestState();

        switch (state)
        {
            case QuestState_Jun.NOTACCEPTED: break;
            case QuestState_Jun.ACCEPTED: state = DetermineQuestState(startTalks, QuestState_Jun.ACCEPTED); break;
            case QuestState_Jun.PROGRESSED: state = DetermineQuestState(progressTalks, QuestState_Jun.PROGRESSED); break;
            case QuestState_Jun.COMPLETED: state = DetermineQuestState(completeTalks, QuestState_Jun.COMPLETED); break;
        }

        talkCoroutine = StartCoroutine(Talks(state));
    }

    private QuestState_Jun DetermineQuestState(List<string> _talks, QuestState_Jun _questState)
    {
        return _talks != null ? _questState : QuestState_Jun.NOTACCEPTED;
    }

    private IEnumerator Talks(QuestState_Jun _state)
    {
        int currentIndex = 0;
        bool isTrigger = false;
        List<string> talks = new List<string>();

        switch (_state)
        {
            case QuestState_Jun.NOTACCEPTED: talks.Add(defaultTalk); break;
            case QuestState_Jun.ACCEPTED: talks = startTalks; break;
            case QuestState_Jun.PROGRESSED: talks = progressTalks; break;
            case QuestState_Jun.COMPLETED: talks = completeTalks; break;
        }

        while (currentIndex < talks.Count + 1)
        {
            if (currentIndex == talks.Count)
            {
                HandleState(_state);
                StartInPlayerCoroutine();
                yield break;
            }
            else if (!isTrigger)
            {
                SetNpcTalk(talks[currentIndex]);
                isTrigger = !isTrigger;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && isTrigger)      // TODO: 대화 넘기기
            {
                currentIndex += 1;
                isTrigger = !isTrigger;
            }

            yield return null;
        }
    }

    private void HandleState(QuestState_Jun _state)
    {
        switch (_state)
        {
            case QuestState_Jun.NOTACCEPTED: break;
            case QuestState_Jun.ACCEPTED: StartSetup(); break;
            case QuestState_Jun.PROGRESSED: ProgressSetup(); break;
            case QuestState_Jun.COMPLETED: CompleteSetup(); break;
        }
    }

    private void StartSetup()
    {
        QuestManager_Jun.instance.SetCurrentQuestState(QuestState_Jun.PROGRESSED);
        QuestManager_Jun.instance.SetCurrentQuestInterface();
    }

    private void ProgressSetup()
    {
        CustomCaseData(QuestManager_Jun.instance.currentQuest);
    }

    private void CompleteSetup()
    {
        QuestManager_Jun.instance.SetCurrentQuestState(QuestState_Jun.NOTACCEPTED);
        QuestManager_Jun.instance.currentQuest += 1;
        QuestManager_Jun.instance.SetCurrentQuestState(QuestState_Jun.ACCEPTED);
        QuestManager_Jun.instance.SetCurrentQuestInterface();
        QuestManager_Jun.instance.QuestCompensation();
        QuestManager_Jun.instance.CallEvent();
    }

    private void SetNpcTalk(string _text)
    {
        // TODO 경민이형 과 상호작용해서 대화문 출력
        // 이때 STRING 과 NPC 이름 보낸다.
        // 잘생긴 준오가 요상한 경민이형의 스크립트 다이얼로그 꺼주는 스크립트 제작
        Debug.Log(_text);
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;

            StartInPlayerCoroutine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;

            StopAllCoroutine();
        }
    }
    #endregion

    #region CustomSet
    private void CustomCaseData(int _currentQuest)
    {
        if (isInteraction == true) { return; }

        switch (_currentQuest)
        {
            case 0:
                switch (id)
                {
                    case "NPC_002": CustomBool(0); isInteraction = true; break;
                    case "NPC_003": CustomBool(0); isInteraction = true; break;
                    case "NPC_004": CustomBool(0); isInteraction = true; break;
                }
                break;
            case 1: if (id == "NPC_021") { CustomBool(1, true); isInteraction = true; } break;
            case 2: if (id == "NPC_022") { CustomBool(2, true); isInteraction = true; } break;
            case 3: if (id == "NPC_023") { CustomBool(3, true); isInteraction = true; } break;
            case 4: break;
            case 5:
                switch (id)
                {
                    case "NPC_024": CustomBool(5, true); isInteraction = true; break;
                    case "NPC_025": CustomBool(5, true); isInteraction = true; break;
                }
                break;
            case 6:
                switch (id)
                {
                    case "NPC_026": CustomBool(6, true); isInteraction = true; break;
                    case "NPC_027": CustomBool(6, true); isInteraction = true; break;
                }
                break;
            case 7: if (id == "NPC_029") { CustomBool(7, true); isInteraction = true; } break;
            case 8:
                switch (id)
                {
                    case "NPC_030": CustomBool(8, true); isInteraction = true; break;
                    case "NPC_031": CustomBool(8, true); isInteraction = true; break;
                }
                break;
            case 9: break;
            case 10:
                switch (id)
                {
                    case "NPC_032": CustomBool(10, true); isInteraction = true; break;
                    case "NPC_033": CustomBool(10, true); isInteraction = true; break;
                }
                break;
            case 11: if (id == "NPC_034") { CustomBool(11, true); isInteraction = true; } break;
            case 12: if (id == "NPC_035") { CustomBool(11, true); isInteraction = true; } break;
            case 13: if (id == "NPC_036") { CustomBool(11, true); isInteraction = true; } break;
            case 14: break;
            case 15: if (id == "NPC_037") { CustomBool(15, true); isInteraction = true; } break;
            case 16:
                switch (id)
                {
                    case "NPC_038": CustomBool(16, true); isInteraction = true; break;
                    case "NPC_039": CustomBool(16, true); isInteraction = true; break;
                    case "NPC_040": CustomBool(16, true); isInteraction = true; break;
                }
                break;
            case 17: if (id == "NPC_041") { CustomBool(17, true); isInteraction = true; } break;
            case 18:
                switch (id)
                {
                    case "NPC_042": CustomBool(18, true); isInteraction = true; break;
                    case "NPC_043": CustomBool(18, true); isInteraction = true; break;
                }
                break;
            case 19: break;
        }
    }

    private void CustomBool(int _currentQuest)
    {
        if (QuestManager_Jun.instance.questList[_currentQuest].isMBTIConditions == default)
        { QuestManager_Jun.instance.questList[_currentQuest].isMBTIConditions = true; }
        else if (QuestManager_Jun.instance.questList[_currentQuest].isMBTIConditions == true)
        { QuestManager_Jun.instance.questList[_currentQuest].isMBTIConditions = false; }
        else
        { QuestManager_Jun.instance.questList[_currentQuest].isMBTIConditions = false; }
    }

    private void CustomBool(int _currentQuest, bool _setValue)
    {
        QuestManager_Jun.instance.questList[_currentQuest].isMBTIConditions = _setValue;
    }
    #endregion
}