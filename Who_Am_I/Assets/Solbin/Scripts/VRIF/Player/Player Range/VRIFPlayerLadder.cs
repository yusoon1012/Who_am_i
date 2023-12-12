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

    // 나무 근방에 있음을 나타냄
    private bool triggerStay = false;
    // 오브젝트 풀
    private Vector3 poolPos = new Vector3(0, -10, 0);

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

        Debug.DrawRay(VRIFInputSystem.Instance.rController.position, VRIFInputSystem.Instance.rController.forward, Color.green);

        if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Floor"))) // 바닥을 향하고 있을 떄 
        {
            if (Vector3.Distance(_tree.transform.position, hit.point) <= 3)
            {
                ladderPointer.transform.position = hit.point;
                //Vector3 rotation = VRIFInputSystem.Instance.rController.rotation;
                //rotation = new Vector3 (0, rotation.y, 0);
                //ladderPointer.rotation = rotation;

            }
        }
    }

    private void InstallLadder(object sender, EventArgs e)
    {
        if (triggerStay)
        {
            ladderItem.position = ladderPointer.position;
            ladderPointer.position = poolPos;
        }
    }
}
