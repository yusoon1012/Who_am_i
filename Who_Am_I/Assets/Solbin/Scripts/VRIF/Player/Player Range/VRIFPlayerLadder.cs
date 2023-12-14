using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Activation;
using Unity.VisualScripting;
using UnityEngine;

public class VRIFPlayerLadder : MonoBehaviour
{
    [Header("사다리 포인터 프리팹")]
    [SerializeField] private GameObject ladderPointerPrefab = default;
    // 사다리 포인터 오브젝트
    private GameObject ladderPointer = default;

    [Header("사다리 아이템 프리팹")]
    [SerializeField] private GameObject ladderItemPrefab = default;
    // 사다리 아이템 오브젝트
    private GameObject ladderItem = default;

    // 플레이어
    private Transform playerController = default;

    // 나무 근방에 있음을 나타냄
    private bool triggerStay = false;
    // 오브젝트 풀
    private Vector3 poolPos = new Vector3(0, -10, 0);
    // 사다리가 활성화 중
    private bool activeLadder = false;
    // 사다리 설치 가능 오브젝트 (설치 미확정)
    private GameObject tempObj = default;
    // 사다리 설치 가능 오브젝트 (설치 확정)
    private GameObject installObj = default;

    private void Start()
    {
        VRIFInputSystem.Instance.interaction += InstallLadder;
        VRIFInputSystem.Instance.interaction += CheckDownKey;

        ladderPointer = Instantiate(ladderPointerPrefab, poolPos, Quaternion.identity);
        ladderItem = Instantiate(ladderItemPrefab, poolPos, Quaternion.identity);

        playerController = transform.parent;

        enabled = false; // 초기 세팅을 마쳤다면 아이템 사용시까지 비활성화
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Tree")) // TODO: 후에 레이어 마스크를 이용하는 것으로 변경하기.
        {
            if (VRIFItemSystem.Instance.itemType == VRIFItemSystem.ItemType.LADDER) // 사다리 장착 중일때
            {
                tempObj = other.gameObject;
                PointerRay(tempObj);
                triggerStay = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Tree"))
        {
            triggerStay = false;
        }
    }

    /// <summary>
    /// 사다리 설치 가능한 지역에 접근시 포인터 표시 
    /// </summary>
    private void PointerRay(GameObject _tree)
    {
        Ray ray = new Ray(VRIFInputSystem.Instance.rController.position, VRIFInputSystem.Instance.rController.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f)) // 바닥을 향하고 있을 때, 3f 이내 
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor")) // 사다리가 비활성화 중일때
            {
                ladderPointer.transform.position = hit.point;

                ladderPointer.transform.LookAt(playerController);
                Vector3 rotation = ladderPointer.transform.rotation.eulerAngles;
                ladderPointer.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
            }
        }
        else
        {
            ladderPointer.transform.position = poolPos;
        }
    }

    /// <summary>
    /// 조건 만족시 사다리 설치 
    /// </summary>
    private void InstallLadder(object sender, EventArgs e)
    {
        if (triggerStay)
        {
            installObj = tempObj;

            ladderItem.transform.position = ladderPointer.transform.position;
            ladderItem.transform.rotation = ladderPointer.transform.rotation;
            ladderPointer.transform.position = poolPos;

            activeLadder = true; // 해당 조건은 사다리가 비활성화 될 때 false.

            StartCoroutine(CheckDistance());

            VRIFItemSystem.Instance.ReleaseItem(); // 아이템 장착 중이었다면 아이템 해제
        }
    }

    /// <summary>
    /// 회수 조건 체크 (거리)
    /// </summary>
    private IEnumerator CheckDistance()
    {
        while (Vector3.Distance(playerController.position, installObj.transform.position) < 20)
        {
            yield return null;
        }

        WithDrawLadder();
    }

    /// <summary>
    /// 회수 조건 체크 (회수 키)
    /// </summary>
    private void CheckDownKey(object sender, EventArgs e)
    {
        if (activeLadder) // 사다리 활성화 상태에 설치 대상 오브젝트까지 있을때
        {
            if (Vector3.Distance(playerController.position, ladderItem.transform.position) <= 2) // 근접 거리라면
            {
                StopCoroutine(CheckDistance()); // 거리 체크 코루틴 정지 
                WithDrawLadder(); // 사다리 회수 
            }    
        }
    }

    /// <summary>
    /// 사다리 회수
    /// </summary>
    private void WithDrawLadder()
    {
        ladderItem.transform.position = poolPos; // 사다리 아이템 풀로 이동 

        activeLadder = false;
    }

    // TODO: 만약 다른 설치가능 오브젝트 구역 안에서 설치를 시도한다면?
}
