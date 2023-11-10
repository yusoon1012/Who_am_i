using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Hall : MonoBehaviour
{
    // ���� �ý��� �Ŵ���
    GameObject userSystemManager = default;
    // �ڷ���Ʈ �ý���
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
            Debug.Log("Ȧ�� ����!");
            ActivateHall();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ȧ���� ����");
            DeactivateHall();
        }
    }
    #endregion

    #region OnTriggerEnter / Exit
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Ȧ�� ����!");
            ActivateHall(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Ȧ���� ����");
            DeactivateHall();
        }
    }
    #endregion

    private void ActivateHall() { teleportSystem.teleportUI.gameObject.SetActive(true); }

    private void DeactivateHall() { teleportSystem.teleportUI.gameObject.SetActive(false); }
}
