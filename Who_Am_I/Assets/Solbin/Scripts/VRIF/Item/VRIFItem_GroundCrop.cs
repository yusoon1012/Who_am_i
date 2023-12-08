using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class VRIFItem_GroundCrop : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Grabber>()) // 만약 손이라면 
        {

        }
    }
}
