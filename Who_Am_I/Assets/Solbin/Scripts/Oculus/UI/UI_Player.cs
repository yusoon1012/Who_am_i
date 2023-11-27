using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �÷��̾� UI ��ũ��Ʈ
/// </summary>
public class UI_Player : MonoBehaviour
{
    #region �ʵ�
    [SerializeField] Transform fullnessGage = default;
    [SerializeField] Transform pooGage = default;
    // ������ ������ ���� ��
    int fullnessCount = default;
    // ���� ������ ���� �� 
    int pooCount = default;
    // ������ ������ �迭
    GameObject[] fullnessArray = default;
    // ���� ������ �迭
    GameObject[] pooArray = default;
    #endregion

    private void Start()
    {
        fullnessCount = fullnessGage.childCount;
        pooCount = pooGage.childCount;

        SetAray();
    }

    /// <summary>
    /// ������ �迭 ����
    /// </summary>
    private void SetAray()
    {
        fullnessArray = new GameObject[fullnessCount];
        for (int i = 0; i < fullnessCount; i++)
        {
            fullnessArray[i] = fullnessGage.GetChild(i).gameObject;
        }

        pooArray = new GameObject[pooCount];
        for (int i = 0; i < pooCount; i++)
        {
            pooArray[i] = pooGage.GetChild(i).gameObject;
        }
    }

    #region ������ ������Ʈ
    /// <summary>
    /// ������ ������ ������Ʈ
    /// </summary>
    private void FollowUp_Fullness()
    {
        float maxFullness = Player_Status.playerStat.fullness; // �����ѹ�

        for (int i = 0; i < fullnessCount; i++)
        {
            if (i < Player_Status.m_fullness / ( maxFullness / fullnessCount))
            {
                fullnessArray[i].SetActive(true);
            }
            else
            {
                fullnessArray[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���� ������ ������Ʈ
    /// </summary>
    private void FollowUp_Poo()
    {
        float maxPoo = Player_Status.playerStat.poo; // �����ѹ�

        for (int i = 0; i < pooCount; i++)
        {
            if (i < Player_Status.m_poo / (maxPoo / pooCount))
            {
                pooArray[i].SetActive(true);
            }
            else
            {
                pooArray[i].SetActive(false);
            }
        }
    }
    #endregion

    private void Update()
    {
        FollowUp_Fullness();
        FollowUp_Poo();
    }

}
