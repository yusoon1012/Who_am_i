using Oculus.Interaction.Unity.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Moving : MonoBehaviour
{
    #region �ʵ�
    // �÷��̾�
    Transform player = default;
    #endregion

    private void Start() { Setting(); }

    /// <summary>
    /// (�ʱ� ����) 
    /// </summary>
    private void Setting()
    {
        player = transform.GetChild(0);
    }

    #region LEGACY: �þ� �̵�(���)�̳� �׽�Ʈ������ ����
    /// <summary>
    /// ���� ��Ʈ�ѷ� ���̽�ƽ ���� �޾� �̵�
    /// </summary>
    /// <param name="value">���� ���̽�ƽ �Է°�</param>
    public void OnRotate(InputAction.CallbackContext context)
    {
        int rotateSpeed = 70; // �þ� ȸ�� �ӵ�

        Vector2 input = context.ReadValue<Vector2>();

        if (input.x < 0) // ���� ȸ��
        {
            player.Rotate(Vector3.down * Time.deltaTime * rotateSpeed);
        }
        else if (input.x > 0) // ���� ȸ��
        {
            player.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
    }
    #endregion

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        Vector3 moveDir = player.forward * input.y + player.right * input.x; // �÷��̾� �̵���
        moveDir.y = 0; // ���� ��ȭ ����

        player.Translate(moveDir * Time.deltaTime, Space.World); // ���� ������ �̵�
    }
}

