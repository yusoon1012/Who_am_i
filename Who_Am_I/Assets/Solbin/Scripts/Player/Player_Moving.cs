using Oculus.Interaction.Unity.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Moving : MonoBehaviour
{
    #region 필드
    // 플레이어
    Transform player = default;
    #endregion

    private void Start() { Setting(); }

    /// <summary>
    /// (초기 세팅) 
    /// </summary>
    private void Setting()
    {
        player = transform.GetChild(0);
    }

    #region LEGACY: 시야 이동(폐기)이나 테스트용으로 오픈
    /// <summary>
    /// 우측 컨트롤러 조이스틱 값을 받아 이동
    /// </summary>
    /// <param name="value">우측 조이스틱 입력값</param>
    public void OnRotate(InputAction.CallbackContext context)
    {
        int rotateSpeed = 70; // 시야 회전 속도

        Vector2 input = context.ReadValue<Vector2>();

        if (input.x < 0) // 좌측 회전
        {
            player.Rotate(Vector3.down * Time.deltaTime * rotateSpeed);
        }
        else if (input.x > 0) // 우측 회전
        {
            player.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
    }
    #endregion

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        Vector3 moveDir = player.forward * input.y + player.right * input.x; // 플레이어 이동값
        moveDir.y = 0; // 높이 변화 방지

        player.Translate(moveDir * Time.deltaTime, Space.World); // 월드 포지션 이동
    }
}

