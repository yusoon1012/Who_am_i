using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Hall : MonoBehaviour
{
    TeleportSystem teleportSystem;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) { ActivateHall(); }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) { DeactivateHall(); }
    }

    private void ActivateHall() { teleportSystem.teleportUI.gameObject.SetActive(true); }

    private void DeactivateHall() { teleportSystem.teleportUI.gameObject.SetActive(false); }
}
