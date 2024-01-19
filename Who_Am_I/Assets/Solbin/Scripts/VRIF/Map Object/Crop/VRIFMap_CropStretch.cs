using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class VRIFMap_CropStretch : MonoBehaviour
{
    private VRIFMap_Crop vrifMap_Crop = default;
    // 뽑는 행동을 한 번만 체크하기 위한 bool값. 
    private bool oneCheck = false;

    private void Start()
    {
        vrifMap_Crop = transform.parent.GetComponent<VRIFMap_Crop>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == vrifMap_Crop.hand) { oneCheck = false; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == vrifMap_Crop.hand)
        {
            if (vrifMap_Crop.hand.CompareTag("Left") && VRIFInputSystem.Instance.lGrab >= 0.5f && !oneCheck)
            {
                PullLeaf(); // 잎을 잡아당겼을 때
            }
            else if (vrifMap_Crop.hand.CompareTag("Right") && VRIFInputSystem.Instance.rGrab >= 0.5f && !oneCheck)
            {
                PullLeaf(); // 잎을 잡아당겼을 때
            }
        }
    }

    /// <summary>
    /// 잎을 잡아당겼을 때
    /// </summary>
    private void PullLeaf()
    {
        oneCheck = true;
        vrifMap_Crop.hp -= 1;

        Invoke("ClearOneCheck", 0.5f);
    }

    private void ClearOneCheck() { oneCheck = false; }
}
