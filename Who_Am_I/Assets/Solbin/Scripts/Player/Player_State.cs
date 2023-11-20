using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class Player_State : MonoBehaviour
{
    // Idle-�÷��̾� ������
    Player_Moving player_Moving = default;

    /// <summary>
    /// ���� �÷��̾� ����
    /// </summary>
    public enum PlayerState
    {
        IDLE, // ��� ����
        CLIMBING, // ��� ����
        LADDER, // ��ٸ� �̿� ����
        TREE // ���� Ÿ�� ����
    }

    public static PlayerState playerState;

    private void Start()
    {
        playerState = PlayerState.IDLE;
        player_Moving = transform.GetComponent<Player_Moving>();
    }

    /// <summary>
    /// �÷��̾� ���� ���� (��� ����)
    /// </summary>
    /// <param name="_state">�÷��̾� ����</param>
    public void ChangeState(PlayerState _state) 
    {
        playerState = _state;
        Debug.LogFormat("�÷��̾� ����: {0}", playerState);

        if (playerState == PlayerState.CLIMBING) { DeactivatePlayerMoving(); } // �÷��̾� ������ ��Ȱ��ȭ
        else if (playerState == PlayerState.IDLE) { ActivatePlayerMoving(); } // �÷��̾� ������ Ȱ��ȭ
    }

    /// <summary>
    /// Idle-�÷��̾� ������ ��Ȱ��ȭ
    /// </summary>
    private void DeactivatePlayerMoving()
    {
        player_Moving.enabled = false;
    }

    /// <summary>
    /// Idle-�÷��̾� ������ Ȱ��ȭ
    /// </summary>
    private void ActivatePlayerMoving()
    {
        player_Moving.enabled = true;
    }
}

