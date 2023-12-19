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
            activateFishing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Water"))
        {
            activateFishing = false;
        }
    }
}
