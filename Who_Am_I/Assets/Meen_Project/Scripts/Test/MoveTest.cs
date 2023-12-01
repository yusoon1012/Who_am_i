using Oculus.Interaction.Samples;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveTest : MonoBehaviour
{
    // 길안내 NPC 가 도달할 목표 위치 배열
    public Transform[] point = new Transform[3];
    // 플레이어 트랜스폼
    public Transform playerTf;

    // 길안내 NPC 트랜스폼
    private Transform npcTf;
    // 길안내 NPC 리짓바디
    private Rigidbody npcRb;

    // 길안내 NPC 가 움직일 수 있는 상황인지 체크
    private bool moveNPC = false;
    // 현재 목표 지점 구분값
    private int pointCheck = default;

    void Awake()
    {
        npcTf = GetComponent<Transform>().transform;
        npcRb = GetComponent<Rigidbody>();

        pointCheck = 0;
    }     // Awake()

    void Update()
    {
        // 길안내 NPC 가 길안내가 가능한 상태면 실행
        if (moveNPC == true)
        {
            MoveNPC();
        }
        // 길안내 NPC 가 길안내가 불가능한 상태면 실행
        else if (moveNPC == false)
        {
            StopNPC();
        }
    }     // Update()

    private void OnTriggerExit(Collider collision)
    {
        // 플레이어 태그 오브젝트가 콜라이더에서 벗어나고, 현재 길안내 중인 상태면 실행
        if (collision.tag == "Player" && moveNPC == true)
        {
            moveNPC = false;
        }
    }     // OnTriggerExit()

    private void OnTriggerEnter(Collider collision)
    {
        // 플레이어 태그 오브젝트가 콜라이더에 들어오고, 현재 길안내 중인 상태가 아니면 실행
        if (collision.tag == "Player" && moveNPC == false)
        {
            moveNPC = true;
        }
        // 길안내 NPC 가 0 번째 포인트 지점에 도착하면 실행
        else if (collision.transform == point[0] && pointCheck == 0)
        {
            pointCheck = 1;
        }
        // 길안내 NPC 가 1 번째 포인트 지점에 도착하면 실행
        else if (collision.transform == point[1] && pointCheck == 1)
        {
            pointCheck = 2;
        }
        // 길안내 NPC 가 2 번째 포인트 지점에 도착하면 실행
        else if (collision.transform == point[2] && pointCheck == 2)
        {
            pointCheck = 3;
            StartCoroutine(OffNPC());
        }
    }     // OnTriggerEnter()

    // 길안내 NPC 가 길안내를 위해 움직이는 함수
    private void MoveNPC()
    {
        if (pointCheck < 3)
        {
            // 길안내 목표 지점을 바라보고
            npcTf.transform.LookAt(point[pointCheck]);
            // Lerp 로 부드럽게 목표 지점으로 이동
            transform.position = Vector3.Lerp(transform.position, point[pointCheck].position, 0.0003f);
        }
    }     // MoveNPC()

    // 길안내 NPC 가 길안내를 잠시 정지하는 함수
    private void StopNPC()
    {
        // 플레이어를 바라보고 길안내를 정지
        npcTf.transform.LookAt(playerTf);
    }     // StopNPC()

    // 마지막 목표 지점에 도착했을 때 실행되는 코루틴 함수
    IEnumerator OffNPC()
    {
        yield return new WaitForSeconds(3f);

        // 길안내 NPC 를 비활성화 함
        npcTf.gameObject.SetActive(false);
    }     // OffNPC()
}
