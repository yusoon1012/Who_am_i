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
    // 왼쪽 손모델
    [SerializeField] Transform leftHand = default;
    // 오른쪽 손모델
    [SerializeField] Transform rightHand = default;
    // 왼쪽 손모델 충돌 체크
    Climbing_GrabCheck grabCheck_Left;
    // 오른쪽 손모델 충돌 체크
    Climbing_GrabCheck grabCheck_Right;
    // 리지드바디
    Rigidbody rigid = default;
    // 왼쪽 컨트롤러 속도값
    private Vector3 leftVel = default;
    // 오른쪽 컨트롤러 속도값
    private Vector3 rightVel = default;
    // 절벽 이동 시 힘
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

    #region 손 접촉 여부 체크
    /// <summary>
    /// 왼손 접촉 여부 체크
    /// </summary>
    private bool LeftGrab
    {
        get
        {
            bool check = false;

            if (grabCheck_Left.thisHand && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) // 홀더에 접촉 && 그립 버튼
            {
                check = true; 

                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) // 그립 버튼을 떼면
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
    /// 오른손 접촉 여부 체크
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
        if (LeftGrab && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) // 왼쪽 손으로 절벽 붙잡기
        {
            rigid.useGravity = false;
            leftHand.localPosition = leftHand.localPosition;
        }

        if (RightGrab && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) // 오른쪽 손으로 절벽 붙잡기
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
    /// 한 손 이용시 등반
    /// </summary>
    /// <param name="_moveDir">이동 방향</param>
    private void AddForce(Vector3 _moveDir)
    {
        rigid.useGravity = false;

        Vector3 moveDir = _moveDir;
        moveDir.z = 0;

        rigid.AddForce(-moveDir * climbingForce);
    }
}
