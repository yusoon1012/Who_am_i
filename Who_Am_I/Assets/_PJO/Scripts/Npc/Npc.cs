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
    private bool isDataInteraction;         // 1번이라도 상호작용 하였는가?
    private bool isValueInteraction;        // 1번이라도 아이템 교환을 하였는가?
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
        isDataInteraction = false;
        isValueInteraction = false;
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
        isDataInteraction = false;
        isValueInteraction = false;
        startTalks = null;
        progressTalks = null;
        completeTalks = null;
        startTalks = QuestManager_Jun.instance.QuestTalk(_questId, "CAN_START", id);
        progressTalks = QuestManager_Jun.instance.QuestTalk(_questId, "IN_PROGRESS", id);
        completeTalks = QuestManager_Jun.instance.QuestTalk(_questId, "CAN_COMPLETE", id);
    }
    #endregion

    #region Solbin: VRIF Input Action
    private VRIFAction vrifAction = default;

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction.Disable();
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
            if (vrifAction.Player.Interaction.triggered || Input.GetKeyDown(KeyCode.J)) // 대화 시작
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
        Debug.Log("진행 중인 퀘스트: " + QuestManager_Jun.instance.currentQuest);
        Debug.Log("진행 상태: " + QuestManager_Jun.instance.questList[QuestManager_Jun.instance.currentQuest].currentProgress);

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
            else if ((vrifAction.Player.Interaction.triggered || Input.GetKeyDown(KeyCode.Z)) && isTrigger) // 대화 넘기기
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
        isValueInteraction = QuestManager_Jun.instance.CustomCaseValue(isValueInteraction, id);
        isDataInteraction = QuestManager_Jun.instance.CustomCaseData(isDataInteraction, id);
    }

    private void CompleteSetup()
    {
        QuestManager_Jun.instance.SetCurrentQuestState(QuestState_Jun.NOTACCEPTED);
        QuestManager_Jun.instance.currentQuest += 1;
        QuestManager_Jun.instance.SetCurrentQuestState(QuestState_Jun.ACCEPTED);
        QuestManager_Jun.instance.SetCurrentQuestInterface();
        QuestManager_Jun.instance.QuestCompensation();
        Debug.Log("부름");
        QuestManager_Jun.instance.CallEvent();
    }

    private void SetNpcTalk(string _text)
    {
        DialogManager.instance.PrintDialog(this.name, _text);
        //GameManager.instance.player.GetComponent<Inventory>().AddInventory(_text,)
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
    
    #endregion
}