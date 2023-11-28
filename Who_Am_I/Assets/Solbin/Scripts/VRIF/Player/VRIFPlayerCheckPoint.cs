using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Player Controller - Player Range)
/// </summary>

public class VRIFPlayerCheckPoint : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            Debug.LogWarning("Enter Check Point");
        }
    }
}
