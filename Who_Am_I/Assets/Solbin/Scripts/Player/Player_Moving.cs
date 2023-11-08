using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Moving : Player_Ray
{
    #region 필드
    // CenterEyeAnchor
    [SerializeField] Transform eye = default;
    // 리지드바디
    Rigidbody rigid = default;
    // 이동 방식
    private const int TELEPORT = 1; // 텔레포트
    private const int MOVING = 2; // 컨트롤러 
    private int howMove = default;
    #endregion

    private void Start()
    {
        rigid = transform.GetComponent<Rigidbody>();
        SelectMoving(); // 임시 메소드 (TODO: 이동 방식 결정 추가)
    }

    public void SelectTeleport() { howMove = TELEPORT; } // 텔레포트 방식 선택

    public void SelectMoving() { howMove = MOVING; } // 컨트롤러 방식 선택

    private void Update()
    {
        if (Player_State.playerState == Player_State.PlayerState.IDLE) // 평시에만 입력을 받음
        {
            if (howMove == TELEPORT)
            {
                if (itGround) { Teleport(); }
            }
            else if (howMove == MOVING)
            {
                Moving();
            }
        }
    }

    #region LEGACY: 시야 이동(폐기)
    //private void SightRotate() // 오른쪽 컨트롤러 => 시야
    //{
    //    int rotateSpeed = 90; // 시야 회전 속도

    //    if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
    //    {
    //        Vector2 stickDir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

    //        if (stickDir.x < 0) // 좌측 회전
    //        {
    //            transform.Rotate(Vector3.down * Time.deltaTime * rotateSpeed);
    //        }
    //        else if (stickDir.x > 0) // 우측 회전
    //        {
    //            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    //        }
    //    }
    //}
    #endregion

    #region 이동 구현
    private void Teleport()
    {
        // TODO: 텔레포트 이동방식 구현하기
        Debug.Log("텔레포트 이동 시도!");
    }

    private void Moving() // 왼쪽 컨트롤러 => 이동
    {
        ///<Point> 양쪽 컨트롤러 동시 이동 불가
        Vector3 stickDir = default;

        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick) && !OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            stickDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); // 우측 컨트롤러 입력
        }
        else if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick) && !OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            stickDir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick); // 좌측 컨트롤러 입력 
        }

        float _speed = Player_Status.m_speed;

        Vector3 moveDir = new Vector3(stickDir.x, 0, stickDir.y);
        transform.Translate(moveDir * _speed * Time.smoothDeltaTime);

        #region LEGACY: 점프(폐기)
        //if (OVRInput.GetDown(OVRInput.Button.One))
        //{
        //    rigid.AddForce(Vector3.up * Player_Status.m_jumpForce);
        //}
        #endregion
    }
    #endregion
}

