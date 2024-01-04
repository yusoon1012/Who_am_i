using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFItem_CropGravity : MonoBehaviour
{
    private Rigidbody cropRigid = default;
    private Collider cropCollider = default;

    private void Start()
    {
        cropRigid = transform.GetComponent<Rigidbody>();
        cropCollider = transform.GetComponent<Collider>();

        cropRigid.useGravity = false; // Start 시점에 중력 비활성화
    }

    public void ActivateGravity() { cropCollider.isTrigger = false; cropRigid.useGravity = true; }
}
