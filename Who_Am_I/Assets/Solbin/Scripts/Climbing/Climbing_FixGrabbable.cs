using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing_FixGrabbable : MonoBehaviour
{
    private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    private OVRGrabbable grabbable;

    private void Start()
    {
        defaultRotation = transform.rotation;
        defaultPosition = transform.position;

        grabbable = GetComponent<OVRGrabbable>();
    }

    private void LateUpdate()
    {
        transform.rotation = defaultRotation;
        transform.position = defaultPosition;
    }
}
