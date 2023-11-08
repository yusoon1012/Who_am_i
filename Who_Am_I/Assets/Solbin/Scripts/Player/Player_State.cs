using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{
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

    /// <summary>
    /// 플레이어 상태 변경 (등반 기준)
    /// </summary>
    /// <param name="_state">플레이어 상태</param>
    public void ChangeState(PlayerState _state) { playerState = _state; }
}
