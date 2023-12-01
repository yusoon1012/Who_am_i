using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    // 플레이어가 입력값을 받아 움직일 최종 위치 Vector 값
    private Vector3 moveVector = Vector3.zero;

    // 플레이어 리짓바디
    private Rigidbody playerRb = default;

    // 움직이는 속도값
    private float speed = default;
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
        speed = 10f;
    }     // Awake()

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }     // Start()

    void Update()
    {
        // "Horizontal" 과 "Vertical" 움직임 입력을 받음
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        // 움직임 입력을 받으면 실행
        if (xInput != 0 || zInput != 0)
        {
            MovePlayer();
        }
    }     // Update()

    // 플레이어가 입력값 만큼 이동하는 함수
    private void MovePlayer()
    {
        // 입력받은 움직임 입력값에 속도값을 곱해줌
        xSpeed = xInput * speed;
        zSpeed = zInput * speed;

        // 플레이어가 이동할 최종 위치
        moveVector = new Vector3(xSpeed, 0f, zSpeed);

        // 플레이어 리짓바디를 이동시킴
        playerRb.velocity = moveVector;
    }     // MovePlayer()
}
