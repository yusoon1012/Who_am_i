using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �÷��̾� UI ��ũ��Ʈ
/// </summary>
public class UI_Player : MonoBehaviour
{
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

    ///<Point> m_fullness�� fullnessGage�� childCount�� ������ �������� �Ѵ�.
    // m_fullness / fullnessGage �ڽ� = 25 / 5 = 5��, 20 / 5 = 4��, 15 / 5 = 3��, 10 / 5 = 2��, 5 / 5 = 1��, 0 / 5 = 0�� 

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

    /// <summary>
    /// ������ ������ ������Ʈ
    /// </summary>
    private void FollowUp_Fullness()
    {
        int quotient = (int)(Player_Status.m_fullness / fullnessCount); // �� (���� ������ ���)
        float remainder = Player_Status.m_fullness % fullnessCount; // ������ (������ ������Ʈ ����)

        // TODO: ������, ���� ��ġ UI�� �ݿ� 
    }

    /// <summary>
    /// ���� ������ ������Ʈ
    /// </summary>
    private void FollowUp_Poo()
    {

    }

    private void Update()
    {
        FollowUp_Fullness();
        FollowUp_Poo();
    }

}
