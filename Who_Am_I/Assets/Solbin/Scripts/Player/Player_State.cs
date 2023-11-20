using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class Player_State : MonoBehaviour
{
    // Idle-플레이어 움직임
    Player_Moving player_Moving = default;

    /// <summary>
    /// 현재 플레이어 상태
    /// </summary>
    public enum PlayerState
    {
        IDLE, // 평시 상태
        CLIMBING, // 등반 상태
        LADDER, // 사다리 이용 상태
        TREE // 나무 타기 상태
    }

    public static PlayerState playerState;

    private void Start()
    {
        playerState = PlayerState.IDLE;
        player_Moving = transform.GetComponent<Player_Moving>();
    }

    /// <summary>
    /// 플레이어 상태 변경 (등반 기준)
    /// </summary>
    /// <param name="_state">플레이어 상태</param>
    public void ChangeState(PlayerState _state) 
    {
        playerState = _state;
        Debug.LogFormat("플레이어 상태: {0}", playerState);

        if (playerState == PlayerState.CLIMBING) { DeactivatePlayerMoving(); } // 플레이어 움직임 비활성화
        else if (playerState == PlayerState.IDLE) { ActivatePlayerMoving(); } // 플레이어 움직임 활성화
    }

    /// <summary>
    /// Idle-플레이어 움직임 비활성화
    /// </summary>
    private void DeactivatePlayerMoving()
    {
        player_Moving.enabled = false;
    }

    /// <summary>
    /// Idle-플레이어 움직임 활성화
    /// </summary>
    private void ActivatePlayerMoving()
    {
        player_Moving.enabled = true;
    }
}

