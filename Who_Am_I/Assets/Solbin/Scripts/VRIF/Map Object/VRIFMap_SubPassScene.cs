using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFMap_SubPassScene : MonoBehaviour
{
    [Header("Hinge Joint")]
    [SerializeField] private HingeJoint doorHinge = default;

    // Mesh Collider (씬 이동 콜라이더)
    private Collider passCollider = default;

    private void Start()
    {
        doorHinge.useLimits = false; // 각도 제약 미사용

        passCollider = transform.GetComponent<Collider>();
        passCollider.enabled = false;

        VRIFSceneManager.Instance.openDoorEvent += OpenDoor; // 메인씬 로딩이 다 되어야 문을 열 수 있게 된다. 
    }

    private void OpenDoor(object sender, EventArgs e) { doorHinge.useLimits = true; passCollider.enabled = true; } // 각도 제약 사용 (문을 열 수 있다.)

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어라면
        {
            VRIFSceneManager.Instance.OpenMainScene(); // 메인씬 열기
        }
    }

}

// MainPassScene과 비슷한 처리가 필요하다. 문을 열고 닿으면 메인씬으로 이동할 수 있도록 처리한다. 
