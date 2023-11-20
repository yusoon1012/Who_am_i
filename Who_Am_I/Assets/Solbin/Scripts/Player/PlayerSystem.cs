using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Meta.WitAi;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// �÷��̾� �ý���
/// </summary>
public class PlayerSystem : MonoBehaviour
{
    #region �ʵ�
    // OVRCameraRig
    //[SerializeField] private Transform player = default;
    // ������
    [SerializeField] private GameObject rHand = default;
    // ���� ��Ʈ�ѷ�
    [SerializeField] private GameObject rController = default;
    // ������ Ȱ��ȭ ����
    public bool handActivate = false;
    // ��ü Ǯ������
    public static Vector3 poolPos = new Vector3(0, -10, 0);
    #endregion

    private void Start()
    {
        ManageEvent(); // �̺�Ʈ ����
    }

    #region �ʱ� ����
    /// <summary>
    /// (�ʱ� ����)�� ��ũ��Ʈ���� �̺�Ʈ�� �޾� ��� ����
    /// </summary>
    private void ManageEvent()
    {
        // �÷��̾� ������ �̺�Ʈ
        Player_Status.playerDefeat += PlayerDefeat;
        // �÷��̾� ���� �̺�Ʈ
        Player_Status.playerPoo += PlayerPoo;
    }
    #endregion

    private void Update()
    {
        if (Player_State.playerState == Player_State.PlayerState.IDLE) // ��ÿ��� ��Ʈ�ѷ� ��ü ����
        {
            ChangeController();
            CheckController();
        }
    }

    /// <summary>
    /// (����)��Ʈ�ѷ� ���� ��ü
    /// </summary>
    private void ChangeController()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two)) // ���� ��Ʈ�ѷ� B��ư Ŭ��
        {
            if (rHand.activeSelf) // ���� Ȱ��ȭ�Ǿ� ���� ��
            {
                rHand.SetActive(false);
                rController.SetActive(true);
            }
            else if (!rHand.activeSelf)// ���� ��Ȱ��ȭ �Ǿ� ���� �� 
            {
                rHand.SetActive(true);
                rController.SetActive(false);
            }
        }
    }

    /// <summary>
    /// (����)��Ʈ�ѷ� �� üũ
    /// </summary>
    private void CheckController()
    {
        if (rHand.activeSelf) { handActivate = true; }
        else if (!rHand.activeSelf) { handActivate = false; }
    }

    /// <summary>
    /// (����)�÷��̾� ����
    /// </summary>
    private void PlayerPoo(object sender, EventArgs e)
    {
        // TODO: �÷��̾� ���� �̺�Ʈ �����ϱ�
        Debug.Log("�÷��̾ ����!");
    }

    /// <summary>
    /// (����)�÷��̾� ��� 
    /// </summary>
    private void PlayerDefeat(object sender, EventArgs e)
    {
        // TODO: �÷��̾� ��� �̺�Ʈ �����ϱ�
        Debug.Log("�÷��̾� ���!");
    }
}
