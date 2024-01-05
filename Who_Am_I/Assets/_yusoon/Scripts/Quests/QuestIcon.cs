using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject notMetToStartIcon;
    [SerializeField] private GameObject canstartIcon;
    [SerializeField] private GameObject inProgressIcon;
    [SerializeField] private GameObject notMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;

    public void SetState(QuestState newState, bool startPoint, bool endPoint)
    {
        notMetToStartIcon.SetActive(false);
        canstartIcon.SetActive(false);
        notMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);
        inProgressIcon.SetActive(false);

        switch (newState)
        {
            case QuestState.NOT_MET:
                if (startPoint) { notMetToStartIcon.SetActive(true); }
                break;
            case QuestState.CAN_START:
                if (startPoint) { canstartIcon.SetActive(true); }
                break;
            case QuestState.IN_PROGRESS:
                if (endPoint) { notMetToFinishIcon.SetActive(true); }
                if (!endPoint && !startPoint) { inProgressIcon.SetActive(true); }
                break;
            case QuestState.CAN_FINISH:
                if (endPoint) { canFinishIcon.SetActive(true); }
                if (!endPoint && !startPoint) { inProgressIcon.SetActive(true); }

                break;
            case QuestState.FINISHED:
                break;
            default:
                break;
        }
    }
}
