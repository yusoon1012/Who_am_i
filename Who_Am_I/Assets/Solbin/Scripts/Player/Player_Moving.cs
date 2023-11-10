using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Moving : MonoBehaviour
{
    #region 필드
    // CenterEyeAnchor
    //[SerializeField] Transform eye = default;
    // 플레이어
    Transform player = default;
    #endregion

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// (초기 세팅) 
    /// </summary>
    private void Setting()
    {
        player = transform.GetChild(0);
    }

    private void Update()
    {
        SightRotate(); // 테스트용 오픈

        if (Player_State.playerState == Player_State.PlayerState.IDLE) // 평시에만 입력을 받음
        {
            Moving();
        }
    }

    #region LEGACY: 시야 이동(폐기)이나 테스트용으로 오픈
    private void SightRotate() // 오른쪽 컨트롤러 => 시야
    {
        int rotateSpeed = 70; // 시야 회전 속도

        if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            Vector2 stickDir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            if (stickDir.x < 0) // 좌측 회전
            {
                player.Rotate(Vector3.down * Time.deltaTime * rotateSpeed);
            }
            else if (stickDir.x > 0) // 우측 회전
            {
                player.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
            }
        }
    }
    #endregion

    #region 이동 구현
    private void Moving() // 왼쪽 컨트롤러 => 이동
    {
        ///<Point> 막바지에 양쪽 컨트롤러 동시 이동 불가하게 설정할 것
        Vector3 stickDir = default;

        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            stickDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); // 조이스틱 입력 
            float _speed = Player_Status.m_speed; // 스피드

            Vector3 moveDir = player.forward * stickDir.y + player.right * stickDir.x; // 정면 y값 이동, 양방향 x값 이동
            moveDir.y = 0; // 높이 변화 방지

            player.Translate(moveDir * Time.deltaTime, Space.World); // 월드 포지션 이동
        }

        #region LEGACY: 점프(폐기)
        //if (OVRInput.GetDown(OVRInput.Button.One))
        //{
        //    rigid.AddForce(Vector3.up * Player_Status.m_jumpForce);
        //}
        #endregion
    }
    #endregion
}

