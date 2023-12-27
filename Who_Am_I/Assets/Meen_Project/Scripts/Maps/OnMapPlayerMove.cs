using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapPlayerMove : MonoBehaviour
{
    //// 지도상의 플레이어 리짓바디
    //private Rigidbody onMapPlayerRb = default;

    //// 지도상의 플레이어의 움직임 값
    //private Vector3 moveVector = Vector3.zero;

    //// X 축 움직임 값
    //private float xSpeed = default;
    //// Y 축 움직임 값
    //private float zSpeed = default;

    //void Start()
    //{
    //    onMapPlayerRb = GetComponent<Rigidbody>();
    //}     // Start()

    //// 지도상의 플레이어도 실제 플레이어가 움직이는 값만큼 동기화 시키는 함수
    //public void MoveOnMapPlayer(float xMove, float zMove)
    //{
    //    // 실제 플레이어의 움직임 값을 불러와 지도 크기만큼 나눔
    //    xSpeed = xMove / 10f;
    //    zSpeed = zMove / 10f;

    //    // 지도상의 플레이어가 이동할 최종 위치
    //    moveVector = new Vector3(xSpeed, 0f, zSpeed);
    //    // 지도상의 플레이어 리짓바디를 이동시킴
    //    onMapPlayerRb.velocity = moveVector;
    //}     // MoveOnMapPlayer()
}
