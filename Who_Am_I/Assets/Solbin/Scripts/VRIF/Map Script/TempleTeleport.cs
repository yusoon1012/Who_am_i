using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleTeleport : MonoBehaviour
{
    [Header("신전 내부")]
    public Transform teleportPosition = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어라면
        {
            other.transform.position = teleportPosition.position; // 텔레포트
        }
    }
}
