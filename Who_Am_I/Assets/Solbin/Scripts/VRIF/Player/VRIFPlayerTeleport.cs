using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFPlayerTeleport : MonoBehaviour
{
    [SerializeField] private GameObject teleportUI = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Hall"))
        {
            teleportUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Hall"))
        {
            teleportUI.SetActive(false);
        }
    }
}
