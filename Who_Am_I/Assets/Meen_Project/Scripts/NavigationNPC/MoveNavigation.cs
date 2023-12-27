using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNavigation : MonoBehaviour
{
    // 길안내 NPC 가 도달할 목표 위치 배열
    public Transform[] point = new Transform[5];
    // 플레이어 트랜스폼
    public Transform playerTf;
    // NPC 컨트롤러 트랜스폼
    public Transform npcControllerTf;

    // 길안내 NPC 의 속도
    public float npcSpeed = default;

    // 1번째 NPC 대기 코루틴 변수
    IEnumerator WaitNPC;
    // 2번째 NPC 대기 코루틴 변수
    IEnumerator SecondWaitNPC;

    // 길안내 NPC 트랜스폼
    private Transform npcTf;
    // 길안내 NPC 리짓바디
    private Rigidbody npcRb;

    // 길안내 NPC 가 움직일 수 있는 상황인지 체크
    private bool moveNPC = false;
    // 길안내 재시작 시 딜레이 시간 존재 체크
    private bool restartMoveDelay = false;

    // NPC 와 떨어졌을 때 플레이어를 기다리는 상태인지 체크
    // 0 : NPC 가 길안내를 진행중인 상태
    // 1 : 처음 10 초간 대기중인 상태
    // 2 : 대화창을 출력하고 다시 30 초간 대기중인 상태
    private int waitNpcCheck = default;
    // 현재 목표 지점 구분값
    private int pointCheck = default;

    void Awake()
    {
        npcTf = GetComponent<Transform>().transform;
        npcRb = GetComponent<Rigidbody>();

        pointCheck = 0;
        waitNpcCheck = 0;
    }     // Awake()

    void Start()
    {
        npcSpeed = 0.03f;
    }     // Start()

    // 길안내 NPC 가 활성화 되었을 때 마다 Update 에서 실시간으로 실행시키는 함수
    public void UpdateFunction()
    {
        // 길안내 NPC 가 길안내가 가능한 상태면 실행
        if (moveNPC == true && restartMoveDelay == false)
        {
            MoveNPC();
        }
        // 길안내 NPC 가 길안내가 불가능한 상태면 실행
        else if (moveNPC == false)
        {
            StopNPC();
        }
    }     // UpdateFunction()

    private void OnTriggerExit(Collider collision)
    {
        // 플레이어 태그 오브젝트가 콜라이더에서 벗어나고, 현재 길안내 중인 상태면 실행
        if (collision.tag == "Player" && moveNPC == true)
        {
            // 길안내 NPC 가 길안내가 불가능한 상태로 변경
            moveNPC = false;
            waitNpcCheck = 1;

            // 길안내 NPC 가 대기하는 함수를 실행
            StartWaitNPC();
        }
    }     // OnTriggerExit()

    private void OnTriggerEnter(Collider collision)
    {
        // 플레이어 태그 오브젝트가 콜라이더에 들어오고, 현재 길안내 중인 상태가 아니면 실행
        if (collision.tag == "Player" && moveNPC == false)
        {
            // 길안내 NPC 가 길안내가 가능한 상태면 실행
            moveNPC = true;

            // 길안내 NPC 의 대기 상태 값에 따라 구분지어 실행함
            switch (waitNpcCheck)
            {
                // 대기 상태 값이 1 이면 첫번째 대기 함수를 중지시킴
                case 1:
                    waitNpcCheck = 0;
                    StopWaitNPC();
                    break;
                // 대기 상태 값이 2 면 두번째 대기 함수를 중지시킴
                case 2:
                    waitNpcCheck = 0;
                    StopSecondWaitNPC();
                    break;
                default:
                    break;
            }
        }
        // 길안내 NPC 가 마지막 포인트 지점에 도착하면 실행
        else if (collision.transform == point[4])
        {
            pointCheck = 5;
            // 길안내 NPC 가 비활성화 되는 코루틴 함수를 실행함
            StartCoroutine(OffNPC());
        }
        // 길안내 NPC 가 마지막이 아닌 포인트 지점에 도착하면 실행
        else if (collision.transform == point[pointCheck])
        {
            // 포인트 지점의 타입의 수치를 1 증가시킴
            pointCheck += 1;
        }
    }     // OnTriggerEnter()

    // 길안내 NPC 가 길안내를 위해 움직이는 함수
    private void MoveNPC()
    {
        // 목표 지점의 단계가 마지막 지점에 도착한 상태 수치보다 작으면
        if (pointCheck < 5)
        {
            // 길안내 목표 지점을 바라보고
            npcTf.transform.LookAt(point[pointCheck]);
            // 현재 단계의 목표 지점으로 천천히 길안내 NPC 를 이동시킴
            transform.position = Vector3.MoveTowards(transform.position, point[pointCheck].position, npcSpeed);
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

    // 1 단계 길안내 NPC 의 대기 상태를 시작하는 함수
    private void StartWaitNPC()
    {
        WaitNPC = FunctionWaitNPC();
        // 1 단계 길안내 NPC 의 대기 상태 코루틴을 실행 시킴
        StartCoroutine(WaitNPC);
    }     // StartWaitNPC()

    // 2 단계 길안내 NPC 의 대기 상태를 시작하는 함수
    private void StartSecondWaitNPC()
    {
        SecondWaitNPC = SecondFunctionWaitNPC();
        // 2 단계 길안내 NPC 의 대기 상태 코루틴을 실행 시킴
        StartCoroutine(SecondWaitNPC);
    }     // StartSecondWaitNPC()

    // 1 단계 길안내 NPC 의 대기 상태를 중지하는 함수
    private void StopWaitNPC()
    {
        if (WaitNPC != null)
        {
            // 1 단계 길안내 NPC 의 대기 상태 코루틴을 중지 시킴
            StopCoroutine(WaitNPC);
        }
    }     // StopWaitNPC()

    // 2 단계 길안내 NPC 의 대기 상태를 중지하는 함수
    private void StopSecondWaitNPC()
    {
        if (SecondWaitNPC != null)
        {
            // 2 단계 길안내 NPC 의 대기 상태 코루틴을 중지 시킴
            StopCoroutine(SecondWaitNPC);
        }
    }     // StopSecondWaitNPC()

    // 1 단계 길안내 NPC 의 대기 상태를 진행하는 코루틴 함수
    IEnumerator FunctionWaitNPC()
    {
        yield return new WaitForSeconds(10f);

        // 대기 상태 값을 2 단계로 변경 시킴
        waitNpcCheck = 2;
        // 2 단계 길안내 NPC 의 대기 상태를 시작하는 함수를 실행함
        StartSecondWaitNPC();
        // 길안내 NPC 의 안내 대화창을 출력하는 함수를 실행함
        npcControllerTf.GetComponent<NPCController>().LeavePlayerDialog();
    }     // FunctionWaitNPC()

    // 2 단계 길안내 NPC 의 대기 상태를 진행하는 코루틴 함수
    IEnumerator SecondFunctionWaitNPC()
    {
        yield return new WaitForSeconds(30f);

        // 대기 상태 값을 3 단계로 변경 시킴
        waitNpcCheck = 3;
        // 길안내 NPC 를 임시 비활성화 상태로 변경하고 대기 상태로 변경하는 함수를 실행함
        npcControllerTf.GetComponent<NPCController>().StopNavigationNPC();
    }     // SecondFunctionWaitNPC()

    // 임시 비활성화 상태에서 활성화 되고 길안내를 재시작 할 때 딜레이 시간을 주는 함수
    public void RestartDelay()
    {
        // 길안내 NPC 가 플레이어를 쳐다보게 함
        npcTf.transform.LookAt(playerTf);
        // 길안내 재시작 딜레이 구분 값을 true 로 바꿔줌
        restartMoveDelay = true;

        // 딜레이 시간을 진행하는 코루틴 함수를 실행함
        StartCoroutine(RestartDelayCoroutine());
    }     // RestartDelay()

    // 임시 비활성화 상태에서 활성화 되고 길안내를 재시작 할 때 딜레이 시간을 진행하는 코루틴 함수
    IEnumerator RestartDelayCoroutine()
    {
        yield return new WaitForSeconds(2f);

        // 길안내 재시작 딜레이 구분 값을 false 로 바꿔줌
        restartMoveDelay = false;
    }     // RestartDelayCoroutine()
}
