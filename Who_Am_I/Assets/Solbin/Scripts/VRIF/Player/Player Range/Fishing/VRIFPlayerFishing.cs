using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFPlayerFishing : MonoBehaviour
{
    public bool activateFishing { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Water")) // TODO: 후에 레이어 등으로 변경
        {
            activateFishing = true; // 낚시 가능 상태로 전환
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Water"))
        {
            activateFishing = false; // 낚시 불가능 상태로 전환
        }
    }
}
