using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFPlayerLadder : MonoBehaviour
{
    [Header("사다리 포인터")]
    [SerializeField] private Transform ladderPointer = default;
    [Header("사다리 아이템")]
    [SerializeField] private Transform ladderItem = default;

    [Header("플레이어")]
    [SerializeField] private Transform playerController = default;

    // 나무 근방에 있음을 나타냄
    private bool triggerStay = false;
    // 오브젝트 풀
    private Vector3 poolPos = new Vector3(0, -10, 0);
    // 사다리가 활성화 중
    private bool activeLadder = false;

    private void Start()
    {
        VRIFInputSystem.Instance.interaction += InstallLadder;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Tree")) // TODO: 후에 레이어 마스크를 이용하는 것으로 변경하기.
        {
            GameObject tree = other.gameObject;
            PointerRay(tree);
            triggerStay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Tree"))
        {
            triggerStay = false;
        }
    }

    private void PointerRay(GameObject _tree)
    {
        Ray ray = new Ray(VRIFInputSystem.Instance.rController.position, VRIFInputSystem.Instance.rController.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // 바닥을 향하고 있을 떄 
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor") && !activeLadder) // 사다리가 비활성화 중일때
            {
                ladderPointer.position = hit.point;

                ladderPointer.LookAt(playerController);
            }
        }
    }

    private void InstallLadder(object sender, EventArgs e)
    {
        if (triggerStay)
        {
            ladderItem.position = ladderPointer.position;
            ladderItem.rotation = ladderPointer.rotation;
            ladderPointer.transform.position = poolPos;

            activeLadder = true;
        }
    }
        
    // TODO: 설치된 사다리 앞에서 Y키를 누르면 사다리는 인벤토리로 회수
    // TODO: PC가 설치한 사다리 앞에서 20이상 떨어진 후 10초가 넘으면 사다리는 인벤토리로 회수 
}
