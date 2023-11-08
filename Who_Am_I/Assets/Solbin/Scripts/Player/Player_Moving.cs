using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Moving : Player_Ray
{
    #region �ʵ�
    // CenterEyeAnchor
    [SerializeField] Transform eye = default;
    // ������ٵ�
    Rigidbody rigid = default;
    // �̵� ���
    private const int TELEPORT = 1; // �ڷ���Ʈ
    private const int MOVING = 2; // ��Ʈ�ѷ� 
    private int howMove = default;
    #endregion

    private void Start()
    {
        rigid = transform.GetComponent<Rigidbody>();
        SelectMoving(); // �ӽ� �޼ҵ� (TODO: �̵� ��� ���� �߰�)
    }

    public void SelectTeleport() { howMove = TELEPORT; } // �ڷ���Ʈ ��� ����

    public void SelectMoving() { howMove = MOVING; } // ��Ʈ�ѷ� ��� ����

    private void Update()
    {
        if (Player_State.playerState == Player_State.PlayerState.IDLE) // ��ÿ��� �Է��� ����
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

    #region LEGACY: �þ� �̵�(���)
    //private void SightRotate() // ������ ��Ʈ�ѷ� => �þ�
    //{
    //    int rotateSpeed = 90; // �þ� ȸ�� �ӵ�

    //    if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
    //    {
    //        Vector2 stickDir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

    //        if (stickDir.x < 0) // ���� ȸ��
    //        {
    //            transform.Rotate(Vector3.down * Time.deltaTime * rotateSpeed);
    //        }
    //        else if (stickDir.x > 0) // ���� ȸ��
    //        {
    //            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    //        }
    //    }
    //}
    #endregion

    #region �̵� ����
    private void Teleport()
    {
        // TODO: �ڷ���Ʈ �̵���� �����ϱ�
        Debug.Log("�ڷ���Ʈ �̵� �õ�!");
    }

    private void Moving() // ���� ��Ʈ�ѷ� => �̵�
    {
        ///<Point> ���� ��Ʈ�ѷ� ���� �̵� �Ұ�
        Vector3 stickDir = default;

        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick) && !OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            stickDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); // ���� ��Ʈ�ѷ� �Է�
        }
        else if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick) && !OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            stickDir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick); // ���� ��Ʈ�ѷ� �Է� 
        }

        float _speed = Player_Status.m_speed;

        Vector3 moveDir = new Vector3(stickDir.x, 0, stickDir.y);
        transform.Translate(moveDir * _speed * Time.smoothDeltaTime);

        #region LEGACY: ����(���)
        //if (OVRInput.GetDown(OVRInput.Button.One))
        //{
        //    rigid.AddForce(Vector3.up * Player_Status.m_jumpForce);
        //}
        #endregion
    }
    #endregion
}

