using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class VRIFMap_CropStretch : MonoBehaviour
{
    private VRIFMap_Crop vrifMap_Crop = default;

    private void Start()
    {
        vrifMap_Crop = transform.parent.GetComponent<VRIFMap_Crop>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == vrifMap_Crop.hand)
        {
            if (vrifMap_Crop.hand.CompareTag("Left") && VRIFInputSystem.Instance.lGrab >= 0.5f)
            {
                Debug.LogWarning("Left Check");
                vrifMap_Crop.hp -= 1;
            }
            else if (vrifMap_Crop.hand.CompareTag("Right") && VRIFInputSystem.Instance.rGrab >= 0.5f)
            {
                Debug.Log("Right Check");
                vrifMap_Crop.hp -= 1;
            }
        }
    }
}
