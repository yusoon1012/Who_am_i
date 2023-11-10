using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Moving : MonoBehaviour
{
    #region �ʵ�
    // CenterEyeAnchor
    //[SerializeField] Transform eye = default;
    // �÷��̾�
    Transform player = default;
    #endregion

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// (�ʱ� ����) 
    /// </summary>
    private void Setting()
    {
        player = transform.GetChild(0);
    }

    private void Update()
    {
        SightRotate(); // �׽�Ʈ�� ����

        if (Player_State.playerState == Player_State.PlayerState.IDLE) // ��ÿ��� �Է��� ����
        {
            Moving();
        }
    }

    #region LEGACY: �þ� �̵�(���)�̳� �׽�Ʈ������ ����
    private void SightRotate() // ������ ��Ʈ�ѷ� => �þ�
    {
        int rotateSpeed = 70; // �þ� ȸ�� �ӵ�

        if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            Vector2 stickDir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            if (stickDir.x < 0) // ���� ȸ��
            {
                player.Rotate(Vector3.down * Time.deltaTime * rotateSpeed);
            }
            else if (stickDir.x > 0) // ���� ȸ��
            {
                player.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
            }
        }
    }
    #endregion

    #region �̵� ����
    private void Moving() // ���� ��Ʈ�ѷ� => �̵�
    {
        ///<Point> �������� ���� ��Ʈ�ѷ� ���� �̵� �Ұ��ϰ� ������ ��
        Vector3 stickDir = default;

        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            stickDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); // ���̽�ƽ �Է� 
            float _speed = Player_Status.m_speed; // ���ǵ�

            Vector3 moveDir = player.forward * stickDir.y + player.right * stickDir.x; // ���� y�� �̵�, ����� x�� �̵�
            moveDir.y = 0; // ���� ��ȭ ����

            player.Translate(moveDir * Time.deltaTime, Space.World); // ���� ������ �̵�
        }

        #region LEGACY: ����(���)
        //if (OVRInput.GetDown(OVRInput.Button.One))
        //{
        //    rigid.AddForce(Vector3.up * Player_Status.m_jumpForce);
        //}
        #endregion
    }
    #endregion
}

