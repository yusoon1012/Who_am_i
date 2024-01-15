using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class VRIFMap_ClimberJump : MonoBehaviour
{
    public enum Direction
    {
        Left, 
        Right, 
        Up
    }

    [Header("점프 방향")]
    public Direction direction = default;

    [Header("측면점프_위")]
    public float sideUpForce = 3.5f;
    [Header("측면점프_옆")]
    public float sideForce = 1.5f;
    [Header("상승점프_위")]
    public float upForce = 4f;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Grabber>())
        {
            Grabber grabber = other.GetComponent<Grabber>();

            if (grabber.HoldingItem) { OrderJump(); } // 닿은 손이 그랩 중이면 
        }
    }

    private void OrderJump()
    {
        VRIFPlayerClimbing playerClimbing = VRIFStateSystem.Instance.playerController.GetComponent<VRIFPlayerClimbing>();
        if (playerClimbing != null) { playerClimbing.DoJump(direction, transform.GetChild(0), sideUpForce, sideForce, upForce); }
    }
}
