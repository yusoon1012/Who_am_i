using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class Player_State : MonoBehaviour
{
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
    }

    /// <summary>
    /// �÷��̾� ���� ���� (��� ����)
    /// </summary>
    /// <param name="_state">�÷��̾� ����</param>
    public void ChangeState(PlayerState _state) { playerState = _state; }
}

