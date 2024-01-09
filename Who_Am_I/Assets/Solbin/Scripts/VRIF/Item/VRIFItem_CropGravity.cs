using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공중에 떠있어야 하는 아이템을 위한 스크립트. (나무, 수풀 등). 플레이어가 획득하면 중력을 적용한다.
/// </summary>
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

    /// <summary>
    /// 아이템은 실체를 가지고 중력을 적용받는다.
    /// </summary>
    public void ActivateGravity() { cropCollider.isTrigger = false; cropRigid.useGravity = true; }
}
