using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class Player_Climbing : MonoBehaviour
{
    // OVRCameraRig
    private OVRCameraRig ovrCameraRig = default;
    // ���� �ո�
    [SerializeField] Transform leftHand = default;
    // ������ �ո�
    [SerializeField] Transform rightHand = default;
    // ���� �ո� �浹 üũ
    Climbing_GrabCheck grabCheck_Left;
    // ������ �ո� �浹 üũ
    Climbing_GrabCheck grabCheck_Right;
    // ������ٵ�
    Rigidbody rigid = default;
    // ���� ��Ʈ�ѷ� �ӵ���
    private Vector3 leftVel = default;
    // ������ ��Ʈ�ѷ� �ӵ���
    private Vector3 rightVel = default;
    // ���� �̵� �� ��
    private const float climbingForce = 5.5f;

    private GameObject currentPoint = default;
    private List<GameObject> contactPoints = new List<GameObject>();
    private MeshRenderer meshRenderer = null;

    private void Start()
    {
        ovrCameraRig = transform.GetChild(0).GetComponent<OVRCameraRig>();

        grabCheck_Left = leftHand.GetComponent<Climbing_GrabCheck>();
        grabCheck_Right = rightHand.GetComponent<Climbing_GrabCheck>();

        rigid = transform.GetChild(0).GetComponent<Rigidbody>();
    }

    #region �� ���� ���� üũ
    /// <summary>
    /// �޼� ���� ���� üũ
    /// </summary>
    private bool LeftGrab
    {
        get
        {
            bool check = false;

            if (grabCheck_Left.thisHand && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) // Ȧ���� ���� && �׸� ��ư
            {
                check = true; 

                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) // �׸� ��ư�� ����
                {
                    check = false;
                }
            }
            else
            {
                check = false; 
            }

            return check;
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

            if (grabCheck_Right.thisHand) { check = true; }
            else { check = false; }

            return check;
        }
    }
    #endregion

    private void Update()
    {
        Climbing();

        leftVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        rightVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);

        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) { }
    }

    private void Climbing()
    {
        if (LeftGrab && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) // ���� ������ ���� �����
        {
            rigid.useGravity = false;
            leftHand.localPosition = leftHand.localPosition;
        }

        if (RightGrab && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) // ������ ������ ���� �����
        {
            rigid.useGravity = false;
        }

        if (!(LeftGrab && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) &&
            !(RightGrab && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            rigid.useGravity = true;
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
}
