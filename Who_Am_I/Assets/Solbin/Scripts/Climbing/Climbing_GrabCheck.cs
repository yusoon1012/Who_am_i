using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing_GrabCheck : MonoBehaviour
{
    private int climbingLayer = default; // ���� ���� �� �ִ� ���̾�
    public bool thisHand = false; // ��� ���� �����ߴ��� �� �� �ֵ��� ��

    private void Start()
    {
        climbingLayer = LayerMask.NameToLayer("Climbing"); // ��� ���� �� �ִ� ���̾� ����ũ
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
