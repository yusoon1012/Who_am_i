using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class Player_State : MonoBehaviour
{
    // Idle-플레이어 움직임 cs
    Player_Moving player_Moving = default;
    // Climb - 측면 점프를 위한 콜라이더
    [SerializeField] private GameObject colliders = default;

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

    private void Start() { Setting(); }

    private void Setting()
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
        Debug.LogWarning($"Player State: {playerState}");

        if (playerState == PlayerState.CLIMBING) { ClimbingState(); } // 등산 상태
        else if (playerState == PlayerState.IDLE) { IdleState(); } // 기본 상태
    }

    /// <summary>
    /// Climbing-플레이어 움직임 비활성화
    /// </summary>
    private void ClimbingState()
    {
        player_Moving.enabled = false; // 플레이어 기본 움직임X

        colliders.SetActive(true); // 측면 점프 콜라이더 활성화
    }

    /// <summary>
    /// Idle-플레이어 움직임 활성화
    /// </summary>
    private void IdleState()
    {
        player_Moving.enabled = true; // 플레이어 기본 움직임O

        colliders.SetActive(false); // 측면 점프 콜라이더 비활성화
    }
}

