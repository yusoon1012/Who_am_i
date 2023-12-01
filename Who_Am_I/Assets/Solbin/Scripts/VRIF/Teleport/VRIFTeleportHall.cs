using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTeleportHall : MonoBehaviour
{
    [SerializeField] GameObject playerTeleportRange = default;
    [SerializeField] GameObject teleportUI = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerTeleportRange) { teleportUI.SetActive(true); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerTeleportRange) { teleportUI.SetActive(false); }
    }
}
