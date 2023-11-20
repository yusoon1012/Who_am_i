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
    #region 필드: 참조
    // OVRCameraRig
    private OVRCameraRig ovrCameraRig = default;
    // Input System
    private PlayerAction playerAction;
    // Player State.cs
    private Player_State player_State = default;
    // 리지드바디
    Rigidbody rigid = default;
    #endregion

    #region 필드: 일반 컨트롤러 & 손 모델
    // 왼쪽 손_제너럴(일반용)
    [SerializeField] GameObject leftHand_G = default;
    // 오른쪽 손_제너럴(일반용)
    [SerializeField] GameObject rightHand_G = default;
    // 오른쪽 컨트롤러_제너럴(일반용)
    [SerializeField] GameObject rightController_G = default;
    // 등반 전 사용하던 컨트롤러 종류
    private GameObject originController_G = default;
    #endregion

    #region 필드: 그랩용 컨트롤러 & 손 모델
    // 왼쪽 손(그랩용)
    [SerializeField] Transform leftHand = default;
    // 오른쪽 손(그랩용)
    [SerializeField] Transform rightHand = default;
    // 왼쪽 손(그랩용) 충돌 체크
    Climbing_GrabCheck grabCheck_Left;
    // 오른쪽 손(그랩용) 충돌 체크
    Climbing_GrabCheck grabCheck_Right;
    // 왼쪽 손(그랩용) 메쉬
    MeshRenderer leftGrabRenderer;
    // 오른쪽 손(그랩용) 메쉬
    MeshRenderer rightGrabRenderer;
    #endregion

    #region 필드: 추가 수치
    // 왼쪽 컨트롤러 속도값
    private Vector3 leftVel = default;
    // 오른쪽 컨트롤러 속도값
    private Vector3 rightVel = default;
    // 절벽 이동 시 힘
    private const float climbingForce = 1f;
    // 그랩시 왼손 위치
    private Vector3 leftPos = default;
    // 그랩시 오른손 위치
    private Vector3 rightPos = default;
    #endregion

    #region 필드: 등반 중 점프
    // 콜라이더 소유 자식 오브젝트
    [SerializeField] private GameObject climbColliders = default;
    // 좌측 점프 콜라이더
    private Climbing_SideJump leftCol = default;
    // 우측 점프 콜라이더
    private Climbing_SideJump rightCol = default;
    #endregion

    private void Start()
    {
        Setting(); 
    }

    #region 초기 세팅
    private void Setting()
    {
        ovrCameraRig = transform.GetChild(0).GetComponent<OVRCameraRig>();
        player_State = transform.GetComponent<Player_State>();

        grabCheck_Left = leftHand.GetComponent<Climbing_GrabCheck>();
        grabCheck_Right = rightHand.GetComponent<Climbing_GrabCheck>();
        leftGrabRenderer = leftHand.GetComponent<MeshRenderer>();
        rightGrabRenderer = rightHand.GetComponent<MeshRenderer>();

        leftGrabRenderer.enabled = false; // 등반 시에만 활성화 되어있어야 한다
        rightGrabRenderer.enabled = false;

        rigid = transform.GetChild(0).GetComponent<Rigidbody>();

        leftCol = climbColliders.transform.GetChild(0).GetComponent<Climbing_SideJump>();
        rightCol = climbColliders.transform.GetChild(1).GetComponent<Climbing_SideJump>();
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

    #region 손 접촉 여부 체크, 손 전환 (일반 - 등반)
    /// <summary>
    /// 왼손 접촉 여부 체크
    /// </summary>
    private bool LeftGrab
    {
        get
        {
            bool check = false;
            float arm = 0.15f; // 팔 길이

            if (grabCheck_Left.thisHand) // 홀더에 접촉 && 그립 버튼
            {
                if (playerAction.Player.LeftGrip.ReadValue<float>() > 0.5f && 
                    Vector3.Distance(leftHand_G.transform.position, leftHand.position) <= arm)
                {
                    check = true; // 등반 상태로 전환
                }
                else if (playerAction.Player.LeftGrip.ReadValue<float>() > 0.5f && 
                    Vector3.Distance(leftHand_G.transform.position, leftHand.position) > arm)
                {
                    rigid.velocity = Vector3.zero; // 리지드바디 zero
                }
            }
            else if (!grabCheck_Left.thisHand)
            {
                check = false; // 평시 상태로 전환
            }

            return check;
        }
    }

    /// <summary>
    /// 왼손 등반 구현
    /// </summary>
    private void LeftClimbing()
    {
        if (LeftGrab)
        {
            leftGrabRenderer.enabled = true; // 등반용 손 메쉬 On
            leftPos = grabCheck_Left.grabPos; // 왼손이 잡아야 할 위치 할당
            leftHand_G.SetActive(false); // 일반 손 Off
        }
        else if (!LeftGrab)
        {
            leftGrabRenderer.enabled = false; // 등반용 손 메쉬 Off
            leftHand.position = leftHand_G.transform.position; // 그랩용 손 위치 = 원래 손 위치
            leftHand_G.SetActive(true); // 일반 손 On
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
            float arm = 0.15f; // 팔 길이

            if (grabCheck_Right.thisHand)
            {
                if (playerAction.Player.RightGrip.ReadValue<float>() > 0.5f &&
                    Vector3.Distance(rightHand_G.transform.position, rightHand.position) <= arm)
                {
                    check = true; // 등반 상태로 전환
                }
                else if (playerAction.Player.RightGrip.ReadValue<float>() > 0.5f &&
                    Vector3.Distance(rightHand_G.transform.position, rightHand.position) > arm)
                {
                    rigid.velocity = Vector3.zero; // 리지드바디 zero
                }
            }
            else if (!grabCheck_Right.thisHand)
            { 
                check = false; // 평시 상태로 전환
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
    /// 오른손 등반 구현
    /// </summary>
    private void RightClimbing()
    {
        if (RightGrab)
        {
            rightGrabRenderer.enabled = true; // 등반용 손 메쉬 On
            rightPos = grabCheck_Right.grabPos; // 오른손이 잡아야 할 위치 할당
            DeactivateRightOrigin(); // 일반 손 Off
        }
        else if (!RightGrab)
        {
            rightGrabRenderer.enabled = false; // 등반용 손 메쉬 Off
            rightHand.position = rightHand_G.transform.position; // 그랩용 손 위치 = 원래 손 위치
            ActivateRightOrigin(); // 일반 손 On
        }
    }
    #endregion

    #region 등반 (rigidbody)
    private void Climbing()
    {
        if (LeftGrab) // 왼쪽 손으로 절벽 붙잡기
        {
            player_State.ChangeState(Player_State.PlayerState.CLIMBING); // 플레이어 상태 전환 - 등반

            rigid.useGravity = false; // 중력 비활성화
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            leftHand.position = leftPos;

            leftVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            AddForce(leftVel);
        }

        if (RightGrab) // 오른쪽 손으로 절벽 붙잡기
        {
            player_State.ChangeState(Player_State.PlayerState.CLIMBING); // 플레이어 상태 전환 - 등반

            rigid.useGravity = false; // 중력 비활성화
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rightHand.position = rightPos;

            rightVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            AddForce(rightVel);
        }

        if (!LeftGrab && !RightGrab) // 어느 손으로도 절벽을 붙잡고 있지 않다면 
        {
            player_State.ChangeState(Player_State.PlayerState.IDLE); // 플레이어 상태 전환 - 일반

            rigid.useGravity = true;
            rigid.constraints &= ~RigidbodyConstraints.FreezeRotationY; // y축 고정 해제
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
    #endregion

    /// <summary>
    /// 등반 중 추락
    /// </summary>
    private void Falling()
    {
        if (rigid.IsSleeping()) // 만약 추락하는 중이라면
        {
            // TODO: 그립을 쥔 채로 암벽 콜라이더에 접촉 시 rigidbody는 zero, CameraRig 위치는 고정? 이전에 만든 메소드를 활용해보자 
        }
    }

    private void Update()
    {
        LeftClimbing();
        RightClimbing();

        Climbing();

        Falling();
    }

    #region 측면 점프
    /// <summary>
    /// 등반 중 좌측 점프
    /// </summary>
    /// <param name="context">X키 매핑(이벤트)</param>
    public void LeftJump(InputAction.CallbackContext context)
    {
        if (leftCol.activateJump) // 좌측 점프가 가능한 상태라면
        {
            Debug.Log("좌측 점프");
        }
    }

    /// <summary>
    /// 등반 중 우측 점프
    /// </summary>
    /// <param name="context">A키 매핑(이벤트)</param>
    public void RightJump(InputAction.CallbackContext context)
    {
        if (rightCol.activateJump) // 우측 점프가 가능한 상태라면 
        {
            Debug.Log("우측 점프");
        }
    }

    // TODO: 아래로 떨어지던 중 절벽을 잡으면 고정되어야 한다. (rigidbody를 제로로 적용.) => (bool)rigid.IsSleeping으로 알 수 있다.
    // TODO: 카메라 위치 조정? 그랩을 잡으면 그랩쪽으로 시야가 약간 이동하는거 같다. 
    #endregion
}
