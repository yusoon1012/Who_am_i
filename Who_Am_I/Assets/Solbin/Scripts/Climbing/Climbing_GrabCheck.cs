using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing_GrabCheck : MonoBehaviour
{
    private int climbingLayer = default; // 손이 잡을 수 있는 레이어
    public bool thisHand = false; // 어느 손이 접촉했는지 알 수 있도록 함

    private void Start()
    {
        climbingLayer = LayerMask.NameToLayer("Climbing"); // 잡고 오를 수 있는 레이어 마스크
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
