using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFPlayerFishing : MonoBehaviour
{
    public bool activateFishing { get; private set; } = false;

    private int waterLayer = default;

    private void Start()
    {
        waterLayer = LayerMask.NameToLayer("Water");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            activateFishing = true; // 낚시 가능 상태로 전환
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            activateFishing = false; // 낚시 불가능 상태로 전환
        }
    }
}
