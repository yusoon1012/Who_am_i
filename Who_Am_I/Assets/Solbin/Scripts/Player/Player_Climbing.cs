using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Player_Climbing : MonoBehaviour
{
    #region �ʵ�
    // OVRCameraRig
    private OVRCameraRig ovrCameraRig = default;
    // Input System
    private PlayerAction playerAction;
    // Player State.cs
    private Player_State player_State = default;
    // ���� ��_���ʷ�(�Ϲݿ�)
    [SerializeField] GameObject leftHand_G = default;
    // ������ ��_���ʷ�(�Ϲݿ�)
    [SerializeField] GameObject rightHand_G = default;
    // ������ ��Ʈ�ѷ�_���ʷ�(�Ϲݿ�)
    [SerializeField] GameObject rightController_G = default;
    // ��� �� ����ϴ� ��Ʈ�ѷ� ����
    private GameObject originController_G = default;

    // Ȱ��ȭ �� ���� ���ʷ� ��
    private GameObject leftModel_G = default;
    // Ȱ��ȭ �� ������ ���ʷ� ��
    private GameObject rightModel_G = default;

    // ���� ��(�׷���)
    [SerializeField] Transform leftHand = default;
    // ������ ��(�׷���)
    [SerializeField] Transform rightHand = default;
    // ���� ��(�׷���) �浹 üũ
    Climbing_GrabCheck grabCheck_Left;
    // ������ ��(�׷���) �浹 üũ
    Climbing_GrabCheck grabCheck_Right;
    // ���� ��(�׷���) �޽�
    MeshRenderer leftGrabRenderer;
    // ������ ��(�׷���) �޽�
    MeshRenderer rightGrabRenderer;

    // ������ٵ�
    Rigidbody rigid = default;
    // ���� ��Ʈ�ѷ� �ӵ���
    private Vector3 leftVel = default;
    // ������ ��Ʈ�ѷ� �ӵ���
    private Vector3 rightVel = default;
    // ���� �̵� �� ��
    private const float climbingForce = 1f;
    // �׷��� �޼� ��ġ
    private Vector3 leftPos = default;
    // �׷��� ������ ��ġ
    private Vector3 rightPos = default;

    bool climbing = false;
    #endregion

    private void Start()
    {
        Setting(); 
    }

    #region �ʱ� ����
    private void Setting()
    {
        ovrCameraRig = transform.GetChild(0).GetComponent<OVRCameraRig>();
        player_State = transform.GetComponent<Player_State>();

        grabCheck_Left = leftHand.GetComponent<Climbing_GrabCheck>();
        grabCheck_Right = rightHand.GetComponent<Climbing_GrabCheck>();
        leftGrabRenderer = leftHand.GetComponent<MeshRenderer>();
        rightGrabRenderer = rightHand.GetComponent<MeshRenderer>();

        leftGrabRenderer.enabled = false; // ��� �ÿ��� Ȱ��ȭ �Ǿ��־�� �Ѵ�
        rightGrabRenderer.enabled = false;

        rigid = transform.GetChild(0).GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playerAction = new PlayerAction();
        playerAction.Enable();
    }

    private void OnDisable()
    {
        playerAction.Disable();
    }
    #endregion

    #region �� ���� ���� üũ, �� ��ȯ (�Ϲ� - ���)
    /// <summary>
    /// �޼� ���� ���� üũ
    /// </summary>
    private bool LeftGrab
    {
        get
        {
            bool check = false;
            float arm = 0.15f; // �� ����

            if (grabCheck_Left.thisHand) // Ȧ���� ���� && �׸� ��ư
            {
                if (playerAction.Player.LeftGrip.ReadValue<float>() > 0.5f && 
                    Vector3.Distance(leftHand_G.transform.position, leftHand.position) <= arm)
                {
                    check = true; // ��� ���·� ��ȯ
                }
                else if (playerAction.Player.LeftGrip.ReadValue<float>() > 0.5f && 
                    Vector3.Distance(leftHand_G.transform.position, leftHand.position) > arm)
                {
                    rigid.velocity = Vector3.zero; // ������ٵ� zero
                }
            }
            else if (!grabCheck_Left.thisHand)
            {
                check = false; // ��� ���·� ��ȯ
            }

            return check;
        }
    }

    /// <summary>
    /// �޼� ��� ����
    /// </summary>
    private void LeftClimbing()
    {
        if (LeftGrab)
        {
            leftGrabRenderer.enabled = true; // ��ݿ� �� �޽� On
            leftPos = grabCheck_Left.grabPos; // �޼��� ��ƾ� �� ��ġ �Ҵ�
            leftHand_G.SetActive(false); // �Ϲ� �� Off
        }
        else if (!LeftGrab)
        {
            leftGrabRenderer.enabled = false; // ��ݿ� �� �޽� Off
            leftHand.position = leftHand_G.transform.position; // �׷��� �� ��ġ = ���� �� ��ġ
            leftHand_G.SetActive(true); // �Ϲ� �� On
        }
    }

    /// <summary>
    /// ������ ���� ���� üũ
    /// </summary>
    private bool RightGrab
    {
        get
        {
            bool check = false;
            float arm = 0.15f; // �� ����

            if (grabCheck_Right.thisHand)
            {
                if (playerAction.Player.RightGrip.ReadValue<float>() > 0.5f &&
                    Vector3.Distance(rightHand_G.transform.position, rightHand.position) <= arm)
                {
                    check = true; // ��� ���·� ��ȯ
                }
                else if (playerAction.Player.RightGrip.ReadValue<float>() > 0.5f &&
                    Vector3.Distance(rightHand_G.transform.position, rightHand.position) > arm)
                {
                    rigid.velocity = Vector3.zero; // ������ٵ� zero
                }
            }
            else if (!grabCheck_Right.thisHand)
            { 
                check = false; // ��� ���·� ��ȯ
            }

            return check;
        }
    }

    private void DeactivateRightOrigin()
    {
        if (rightHand_G.activeSelf) { originController_G = rightHand_G; }
        else if (rightController_G.activeSelf) { originController_G = rightController_G; }

        originController_G.SetActive(false);
    }

    private void ActivateRightOrigin()
    {
        if (rightHand_G.activeSelf) { originController_G = rightHand_G; }
        else if (rightController_G.activeSelf) { originController_G = rightController_G; }

        originController_G.SetActive(true);
    }

    /// <summary>
    /// ������ ��� ����
    /// </summary>
    private void RightClimbing()
    {
        if (RightGrab)
        {
            rightGrabRenderer.enabled = true; // ��ݿ� �� �޽� On
            rightPos = grabCheck_Right.grabPos; // �������� ��ƾ� �� ��ġ �Ҵ�
            DeactivateRightOrigin(); // �Ϲ� �� Off
        }
        else if (!RightGrab)
        {
            rightGrabRenderer.enabled = false; // ��ݿ� �� �޽� Off
            rightHand.position = rightHand_G.transform.position; // �׷��� �� ��ġ = ���� �� ��ġ
            ActivateRightOrigin(); // �Ϲ� �� On
        }
    }
    #endregion

    #region ��� (rigidbody)
    private void Climbing()
    {
        if (LeftGrab) // ���� ������ ���� �����
        {
            player_State.ChangeState(Player_State.PlayerState.CLIMBING); // �÷��̾� ���� ��ȯ - ���

            rigid.useGravity = false; // �߷� ��Ȱ��ȭ
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            leftHand.position = leftPos;

            leftVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            AddForce(leftVel);
        }

        if (RightGrab) // ������ ������ ���� �����
        {
            player_State.ChangeState(Player_State.PlayerState.CLIMBING); // �÷��̾� ���� ��ȯ - ���

            rigid.useGravity = false; // �߷� ��Ȱ��ȭ
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rightHand.position = rightPos;

            rightVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            AddForce(rightVel);
        }

        if (!LeftGrab && !RightGrab) // ��� �����ε� ������ ����� ���� �ʴٸ� 
        {
            player_State.ChangeState(Player_State.PlayerState.IDLE); // �÷��̾� ���� ��ȯ - �Ϲ�

            rigid.useGravity = true;
            rigid.constraints &= ~RigidbodyConstraints.FreezeRotationY; // y�� ���� ����
        }     
    }

    /// <summary>
    /// �� �� �̿�� ���
    /// </summary>
    /// <param name="_moveDir">�̵� ����</param>
    private void AddForce(Vector3 _moveDir)
    {
        rigid.useGravity = false;

        Vector3 moveDir = _moveDir;
        moveDir.z = 0;

        rigid.AddForce(-moveDir * climbingForce);
    }
    #endregion

    private void Falling()
    {
        if (rigid.IsSleeping()) // ���� �߶��ϴ� ���̶��
        {
            // TODO: �׸��� �� ä�� �Ϻ� �ݶ��̴��� ���� �� rigidbody�� zero, CameraRig ��ġ�� ����? ������ ���� �޼ҵ带 Ȱ���غ��� 
        }
    }

    private void Update()
    {
        LeftClimbing();
        RightClimbing();

        Climbing();

        Falling();
    }

    /// <summary>
    /// ��� �� ���� ����
    /// </summary>
    private void LeftJump()
    {

    }

    /// <summary>
    /// ��� �� ������ ���� 
    /// </summary>
    private void RightJump()
    {

    }

    // TODO: �Ʒ��� �������� �� ������ ������ �����Ǿ�� �Ѵ�. (rigidbody�� ���η� ����.) => (bool)rigid.IsSleeping���� �� �� �ִ�.
    // TODO: ī�޶� ��ġ ����? �׷��� ������ �׷������� �þ߰� �ణ �̵��ϴ°� ����. 
}
