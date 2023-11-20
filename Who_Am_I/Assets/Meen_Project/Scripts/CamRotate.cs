using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    #region 변수 선언

    // 현재 각도
    Vector3 angle;

    // 마우스 감도
    private float sensitivity = default;
    // 마우스의 좌우 입력 받기
    private float moveX = default;
    private float moveY = default;

    #endregion 변수 선언

    void Awake()
    {
        // 마우스 감도
        sensitivity = 1000f;
        // 마우스의 좌우 입력 받기
        moveX = 0f;
        moveY = 0f;
    }     // Awake()

    void Start()
    {
        // 시작할 때 현재 카메라 각도를 적용
        angle.y = -Camera.main.transform.eulerAngles.x;
        angle.x = Camera.main.transform.eulerAngles.y;
        angle.z = Camera.main.transform.eulerAngles.z;
    }     // Start()

    void Update()
    {
        // 마우스 움직임의 좌우 입력을 받음
        moveX = Input.GetAxis("Mouse X");
        moveY = Input.GetAxis("Mouse Y");

        // 마우스 움직임 방향을 연산
        angle.x += moveX * sensitivity * Time.deltaTime;
        angle.y += moveY * sensitivity * Time.deltaTime;

        // 위에서 연산한 Axis 와 angle 을 사용해 회전
        transform.eulerAngles = new Vector3(-angle.y, angle.x, angle.z);
    }     // Update()
}
