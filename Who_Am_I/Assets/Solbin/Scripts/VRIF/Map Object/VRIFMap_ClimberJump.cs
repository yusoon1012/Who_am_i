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

    public Direction direction = default;

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
        if (playerClimbing != null) { playerClimbing.DoJump(direction, transform.parent); }
    }
}
