using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFSubCamera : MonoBehaviour
{
    [SerializeField] private Transform centerEyeAnchor = default;

    private void LateUpdate()
    {
        transform.position = centerEyeAnchor.position;
        transform.rotation = centerEyeAnchor.rotation;
    }
}
