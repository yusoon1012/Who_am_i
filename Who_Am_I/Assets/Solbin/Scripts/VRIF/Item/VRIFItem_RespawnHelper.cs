using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class VRIFItem_RespawnHelper : MonoBehaviour
{
    // 리스폰 대기 시간
    private WaitForSeconds waitTime = new WaitForSeconds(30);
    // 오브젝트 풀
    private Vector3 poolPos = new Vector3(0, -10, 0);
    // 원래 위치
    private Vector3 originPos = default;
    // Grabbable이 붙어있는 진짜 아이템 
    private Transform realItem = default;

    private void Start()
    {
        if (transform.parent.GetComponent<Grabbable>())
        {
            realItem = transform.parent;
            originPos = realItem.position;
        }
        else { Debug.LogError("<Solbin> 아이템 재설정 바람."); }
    }

    public IEnumerator Respawn()
    {
        realItem.GetComponent<Rigidbody>().useGravity = false; // 풀 이동 전 중력 비활성화
        realItem.position = poolPos; // 오브젝트 풀로 이동

        yield return waitTime; // 30초 대기 

        realItem.position = originPos; // 시작 자리로 복귀
        realItem.GetComponent<Rigidbody>().useGravity = true; // 복귀 후 중력 활성화
    }
}
