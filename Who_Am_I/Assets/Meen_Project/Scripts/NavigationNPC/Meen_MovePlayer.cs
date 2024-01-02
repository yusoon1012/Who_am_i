using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meen_MovePlayer : MonoBehaviour
{
    // 맵 컨트롤러 트랜스폼
    public Transform mapControllerTf;
    // 지도상의 플레이어 트랜스폼
    public Transform onMapPlayerTf;

    // 움직이는 속도값
    public float speed = default;

    //// 플레이어 리짓바디
    //private Rigidbody playerRb = default;

    private GameObject onNpcCheck = null;

    // 플레이어가 입력값을 받아 움직일 최종 위치 Vector 값
    private Vector3 moveVector = Vector3.zero;

    // x 축 입력값
    private float xInput = default;
    // z 축 입력값
    private float zInput = default;
    // x 축 움직임 계산값
    private float xSpeed = default;
    // z 축 움직임 게산값
    private float zSpeed = default;

    void Awake()
    {
        speed = 500f;
    }     // Awake()

    void Start()
    {
        //playerRb = GetComponent<Rigidbody>();
    }     // Start()

    // Update 마다 조건에 맞으면 실행되는 함수
    public void UpdateFunction()
    {
        //// "Horizontal" 과 "Vertical" 움직임 입력을 받음
        //xInput = Input.GetAxis("Horizontal");
        //zInput = Input.GetAxis("Vertical");

        //// 움직임 입력을 받으면 실행
        //if (xInput != 0 || zInput != 0)
        //{
        //    MovePlayer();
        //}

        //if (Input.GetKeyDown(KeyCode.J) && Meen_QuestManager.instance.onNpcCheck != null)
        //{
        //    Meen_QuestManager.instance.QuestTypeCheck();
        //}
    }     // UpdateFunction()

    // 플레이어가 입력값 만큼 이동하는 함수
    private void MovePlayer()
    {
        // 입력받은 움직임 입력값에 속도값을 곱해줌
        xSpeed = xInput * speed;
        zSpeed = zInput * speed;

        // 플레이어가 이동할 최종 위치
        moveVector = new Vector3(xSpeed, 0f, zSpeed);
        // 플레이어 리짓바디를 이동시킴
        //playerRb.velocity = moveVector;

        //* LEGACY : 지도 UI 를 활성화 할 때 지도상의 플레이어 위치를 실제 플레이어 위치에서 계산하여 이동시켜주는 방법으로 변경됨
        // 지도상의 플레이어에게 움직임 값을 동기화 시키는 함수를 실행
        //onMapPlayerTf.GetComponent<OnMapPlayerMove>().MoveOnMapPlayer(xSpeed, zSpeed);
    }     // MovePlayer()
}
