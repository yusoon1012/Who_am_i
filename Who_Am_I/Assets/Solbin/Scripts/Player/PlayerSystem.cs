using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

/// <summary>
/// �÷��̾� �ý���
/// </summary>
public class PlayerSystem : MonoBehaviour
{
    #region �ʵ�
    // OVRCameraRig
    [SerializeField] private Transform player = default;
    // ������
    [SerializeField] private GameObject rHand = default;
    // ���� ��Ʈ�ѷ�
    [SerializeField] private GameObject rController = default;
    #endregion

    private void Start()
    {
        ManageEvent(); // �̺�Ʈ ����
    }

    /// <summary>
    /// �� ��ũ��Ʈ���� �̺�Ʈ�� �޾� ��� ����
    /// </summary>
    private void ManageEvent()
    {
        // �÷��̾� ������ �̺�Ʈ
        Player_Status.playerDefeat += PlayerDefeat;
        // �÷��̾� ���� �̺�Ʈ
        Player_Status.playerPoo += PlayerPoo;
    }

    private void Update()
    {
        if (Player_State.playerState == Player_State.PlayerState.IDLE) // ��ÿ��� ��Ʈ�ѷ� ��ü ����
        {
            ChangeController();
        }
    }

    /// <summary>
    /// ��Ʈ�ѷ� ���� ��ü
    /// </summary>
    private void ChangeController()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two)) // ���� ��Ʈ�ѷ� B��ư Ŭ��
        {
            if (rHand.activeSelf) // �� Ȱ��ȭ ��
            {
                rHand.SetActive(false);
                rController.SetActive(true);
            }
            else // �� ��Ȱ��ȭ ��
            {
                rHand.SetActive(true);
                rController.SetActive(false);
            }
        }
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    private void PlayerPoo(object sender, EventArgs e)
    {
        // TODO: �÷��̾� ���� �̺�Ʈ �����ϱ�
        Debug.Log("�÷��̾ ����!");
    }

    /// <summary>
    /// �÷��̾� ��� 
    /// </summary>
    private void PlayerDefeat(object sender, EventArgs e)
    {
        // TODO: �÷��̾� ��� �̺�Ʈ �����ϱ�
        Debug.Log("�÷��̾� ���!");
    }
}
