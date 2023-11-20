using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing_GrabCheck : MonoBehaviour
{
    // 손이 잡을 수 있는 레이어
    private int climbingLayer = default;
    // 어느 손이 접촉 했는지 알 수 있도록 함
    public bool thisHand = false;

    public Vector3 grabPos = default;

    private void Start()
    {
        climbingLayer = LayerMask.NameToLayer("Climbing"); // 잡고 오를 수 있는 레이어 마스크
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == climbingLayer)
        {
            grabPos = other.transform.GetChild(0).position; // Grab Pos 위치
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == climbingLayer)
        {
            thisHand = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == climbingLayer)
        {
            thisHand = false;
        }
    }
}
