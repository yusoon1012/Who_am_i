using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartNavigation : MonoBehaviour
{
    // NPC 컨트롤러 트랜스폼
    public Transform npcControllerTf;

    private void OnTriggerEnter(Collider collision)
    {
        // 길안내 재시작 지점에 플레이어 태그 오브젝트와, 길안내 체크 변수값이 2 면 실행
        if (collision.tag == "Player" && npcControllerTf.GetComponent<NPCController>().onNavigationCheck == 2)
        {
            // NPC 컨트롤러 스크립트의 길안내 NPC 의 길안내 재시작 기능의 함수를 실행함
            npcControllerTf.GetComponent<NPCController>().RestartNavigationNPC();
        }
    }     // OnTriggerEnter()
}
