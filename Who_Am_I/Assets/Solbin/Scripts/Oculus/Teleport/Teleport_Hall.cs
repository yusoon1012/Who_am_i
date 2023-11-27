using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Hall : MonoBehaviour
{
    // 유저 시스템 매니저
    GameObject userSystemManager = default;
    // 텔레포트 시스템
    TeleportSystem teleportSystem;

    private void Start()
    {
        userSystemManager = FindObjectOfType<TeleportSystem>().gameObject;
        teleportSystem = userSystemManager.GetComponent<TeleportSystem>();
    }

    #region OnCollisionEnter / Exit
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivateHall();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DeactivateHall();
        }
    }
    #endregion

    #region OnTriggerEnter / Exit
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            ActivateHall(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            DeactivateHall();
        }
    }
    #endregion

    private void ActivateHall() { teleportSystem.teleportUI.gameObject.SetActive(true); }

    private void DeactivateHall() { teleportSystem.teleportUI.gameObject.SetActive(false); }
}
