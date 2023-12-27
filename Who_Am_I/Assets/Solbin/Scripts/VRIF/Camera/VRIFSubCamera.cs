using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFSubCamera : MonoBehaviour
{
    [Header("자식: CenterEyeAnchor")]
    [SerializeField] private Transform centerEyeAnchor = default;

    private void LateUpdate()
    {
        transform.position = centerEyeAnchor.position;
        transform.rotation = centerEyeAnchor.rotation;
    }
}
