using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFItem_TreeFruit : MonoBehaviour
{
    private Rigidbody fruitRigid = default;

    private void Start()
    {
        fruitRigid = transform.GetComponent<Rigidbody>();
        fruitRigid.useGravity = false; // 나무 과일은 Start 시점에 중력 비활성화
    }

    public void ActivateGravity() { fruitRigid.useGravity = true; }
}
