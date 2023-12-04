using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTest : MonoBehaviour
{
    // 길안내 NPC 트랜스폼
    public Transform npcTf;

    // 길안내 NPC 가 활성화 된 상태인지 체크
    private bool npcOn = false;

    private void OnTriggerEnter(Collider collision)
    {
        // 시작 지점에 플레이어 태그 오브젝트가 들어오면 실행
        if (npcOn == false && collision.tag == "Player")
        {
            npcOn = true;
            npcTf.gameObject.SetActive(true);
        }
    }     // OnTriggerEnter()
}
