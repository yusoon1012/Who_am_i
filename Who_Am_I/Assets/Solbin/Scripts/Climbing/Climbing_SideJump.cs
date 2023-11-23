using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing_SideJump : MonoBehaviour
{
    // 손이 잡을 수 있는 레이어
    private int climbingLayer = default;
    // 점프 가능 여부
    public bool activateJump = false;
    // 점프할 좌표
    public Vector3 jumpPos = default;

    private void Start()
    {
        climbingLayer = LayerMask.NameToLayer("Climbing"); // 잡고 오를 수 있는 레이어 마스크
    }

    /// <summary>
    /// 측면 점프 가능
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == climbingLayer) // 뛸 수 있는 레이어에 접촉
        {
            jumpPos = other.transform.position; // 뛸 수 있는 좌표로 등록
            activateJump = true; // 점프 가능 bool
        }
    }

    /// <summary>
    /// 측면 점프 불가능
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == climbingLayer)
        {
            jumpPos = default;
            activateJump = false; // 점프 불가능 bool 
        }
    }
}
