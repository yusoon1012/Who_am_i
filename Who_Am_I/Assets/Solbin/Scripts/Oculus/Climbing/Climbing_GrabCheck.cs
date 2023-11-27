using Oculus.Interaction.Surfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Climbing_GrabCheck : MonoBehaviour
{
    // 손이 잡을 수 있는 레이어
    private int climbingLayer = default;
    // 어느 손이 접촉 했는지 알 수 있도록 함
    public bool thisHand = false;
    // 마지막 등반 지점 접촉 이벤트
    public event EventHandler finishLine;
    // 손이 잡아야 할 좌표
    public Vector3 grabPos;
    // 마지막 등반 지점 접촉 여부
    public bool finishHand= false;
    // 마지막 등반 지점에서 손이 잡아야 할 좌표
    public Vector3 finishGrabPos;

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

        if (other.gameObject.name == "Cliff Finish")
        {
            finishGrabPos = other.transform.position; // 마지막 등반 지점 손의 위치
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == climbingLayer)
        {
            thisHand = true;
        }

        if (other.gameObject.name == "Cliff Finish")
        {
            finishLine(this, EventArgs.Empty); // 마지막 등반 지점 접촉 이벤트
            finishHand = true; // 마지막 등반 지점에 접촉함
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == climbingLayer)
        {
            thisHand = false;
        }

        if (other.gameObject.name == "Cliff Finish")
        {
            finishHand = false; // 마지막 등반 지점에 접촉하지 않음
        }
    }
}
