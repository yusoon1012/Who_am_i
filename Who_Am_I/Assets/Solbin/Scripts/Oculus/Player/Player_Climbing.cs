using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.XR.LegacyInputHelpers;
using System;

public class Player_Climbing : MonoBehaviour
{
    #region 필드: 참조
    [Header("Reference")]
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
    [Header("General Controller & Hand")]
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
    [Header("Grab Controller & Hand")]
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
    [Header("Essential Number")]
    // 왼쪽 컨트롤러 속도값
    private Vector3 leftVel = default;
    // 왼쪽 컨트롤러 Magnitude
    private float leftMagnitude = default;
    // 오른쪽 컨트롤러 속도값
    private Vector3 rightVel = default;
    // 오른쪽 컨트롤러 Magnitude 
    private float rightMagnitude = default;
    // 그랩시 왼손 위치
    private Vector3 leftPos = default;
    // 그랩시 오른손 위치
    private Vector3 rightPos = default;
    #endregion

    #region 필드: 등반 중 점프
    [Header("Climbing Jump")]
    // 콜라이더 소유 자식 오브젝트
    [SerializeField] private GameObject climbColliders = default;
    // 좌측 점프 콜라이더
    private Climbing_SideJump leftCol = default;
    // 우측 점프 콜라이더
    private Climbing_SideJump rightCol = default;
    #endregion

    // 추락
    bool fallDown = false;
    // 상승 점프
    bool superSwing = false;

    bool sideJumping = false;

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

        grabCheck_Left.finishLine += LastClimbing; // 마지막 등반 이벤트 구독
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

    #region 구현: 손 접촉 여부 체크, 손 전환 (일반 - 등반)
    /// <summary>
    /// 왼손 접촉 & 그립 여부 체크 
    /// </summary>
    private bool LeftGrab
    {
        get
        {
            bool check = false;

            if (grabCheck_Left.thisHand)
            {
                if (playerAction.Player.LeftGrip.ReadValue<float>() >= 0.5f) // 그립을 쥐었으면
                {
                    check = true; // 등반 상태 전환
                }
            }

            if (playerAction.Player.LeftGrip.ReadValue<float>() < 0.5f) // 그립을 놓으면 어느 조건에서든
            {
                check = false;
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
    /// 오른손 접촉 & 그립 여부 체크
    /// </summary>
    private bool RightGrab
    {
        get
        {
            bool check = false;

            if (grabCheck_Right.thisHand)
            {
                if (playerAction.Player.RightGrip.ReadValue<float>() >= 0.5f) // 그립을 쥐었으면
                {
                    check = true; // 등반 상태 전환
                }
            }

            if (playerAction.Player.RightGrip.ReadValue<float>() < 0.5f) // 그립을 놓으면 어느 조건에서든
            {
                check = false;
            }

            return check;
        }
    }

    /// <summary>
    /// 아래 두 메소드는 기존 컨트롤러(혹은 손)를 기억해두었가가 IDLE 상태에 본 컨트롤러를 반환한다. 
    /// </summary>
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
            DeactivateRightOrigin(); // 본 컨트롤러 Off
        }
        else if (!RightGrab)
        {
            rightGrabRenderer.enabled = false; // 등반용 손 메쉬 Off
            rightHand.position = rightHand_G.transform.position; // 그랩용 손 위치 = 원래 손 위치
            ActivateRightOrigin(); // 본 컨트롤러 On
        }
    }
    #endregion

    #region 구현: 등반 (Rigidbody, Transform)
    private void Climbing()
    {
        // 왼쪽 컨트롤러 Velocity
        leftVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        // 왼쪽 컨트롤러 Magnitude
        leftMagnitude = Vector3.Magnitude(leftVel);
        // 오른쪽 컨트롤러 Velocity
        rightVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        // 오른쪽 컨트롤러 Velocity
        rightMagnitude = Vector3.Magnitude(rightVel);

        // TODO: 왼손이든 오른손이든 잡고만 있다면 점프가 가능하도록 변경

        //if (LeftGrab) // 왼손 등반
        //{
        //    if (playerAction.Player.ClimbingLeftJump.ReadValue<float>() > 0.5f) // 오르던 중 좌측 점프 버튼
        //    {
        //        LeftJump();
        //    }
        //    else // 점프 순간이 아닐 때만
        //    {
        //        player_State.ChangeState(Player_State.PlayerState.CLIMBING); // 플레이어 상태 전환 - 등반

        //        rigid.useGravity = false; // 중력 비활성화
        //        rigid.constraints = RigidbodyConstraints.FreezeRotation; // 축 고정
        //        leftHand.position = leftPos;

        //        AddForce(leftVel, leftMagnitude);
        //    }
        //}

        //if (RightGrab) // 오른손 등반
        //{
        //    if (playerAction.Player.ClimbingRightJump.ReadValue<float>() > 0.5f) // 오르던 중 우측 점프 버튼
        //    {
        //        RightJump();
        //    }
        //    else // 점프 순간이 아닐 때만
        //    {
        //        player_State.ChangeState(Player_State.PlayerState.CLIMBING); // 플레이어 상태 전환 - 등반

        //        rigid.useGravity = false; // 중력 비활성화
        //        rigid.constraints = RigidbodyConstraints.FreezeRotation; // 축 고정
        //        rightHand.position = rightPos;

        //        AddForce(rightVel, rightMagnitude);
        //    }
        //}

        if (LeftGrab || RightGrab)
        {
            if (playerAction.Player.ClimbingLeftJump.triggered)
            {
                Debug.LogWarning("Left Jump");
                sideJumping = true;
                LeftJump();
                sideJumping = false;
            }

            if (playerAction.Player.ClimbingRightJump.triggered)
            {
                sideJumping = true;
                RightJump();
                sideJumping = false;
            }

            if (LeftGrab && !sideJumping)
            {
                player_State.ChangeState(Player_State.PlayerState.CLIMBING); // 플레이어 상태 전환 - 등반

                rigid.useGravity = false; // 중력 비활성화
                rigid.constraints = RigidbodyConstraints.FreezeRotation; // 축 고정
                leftHand.position = leftPos;

                AddForce(leftVel, leftMagnitude);
            }

            if (RightGrab && !sideJumping)
            {
                player_State.ChangeState(Player_State.PlayerState.CLIMBING); // 플레이어 상태 전환 - 등반

                rigid.useGravity = false; // 중력 비활성화
                rigid.constraints = RigidbodyConstraints.FreezeRotation; // 축 고정
                rightHand.position = rightPos;

                AddForce(rightVel, rightMagnitude);
            }
        }

        if (Player_State.playerState == Player_State.PlayerState.CLIMBING && 
            !LeftGrab && !RightGrab) // 추락
        {
            player_State.ChangeState(Player_State.PlayerState.IDLE); // 플레이어 상태 전환 - 일반

            rigid.useGravity = true;
            rigid.constraints &= ~RigidbodyConstraints.FreezeRotationY; // y축 고정 해제

            fallDown = true;
        }     
    }

    /// <summary>
    /// 한 손 이용시 등반
    /// </summary>
    /// <param name="_moveDir">이동 방향</param>
    private void AddForce(Vector3 _moveDir, float _magnitude)
    {
        rigid.useGravity = false;

        Vector3 moveDir = _moveDir;
        moveDir.z = 0;

        float magnitude = _magnitude + 0.3f; // 컨트롤러 속도 절댓값 (보정값 추가)

        ovrCameraRig.transform.Translate(-moveDir * magnitude * Time.deltaTime);
    }
    #endregion

    #region 구현: 측면 점프 (Rigidbody)
    /// <summary>
    /// 등반 중 좌측 점프
    /// </summary>
    public void LeftJump()
    {
        float upJumpForce = 1.5f; // 위로 점프하는 힘
        float leftJumpForce = 1f; // 좌측으로 점프하는 힘

        if (leftCol.activateJump) // 좌측 점프가 가능한 상태라면
        {
            Vector3 targetPos = leftCol.jumpPos; // 인식한 타겟 좌표
            targetPos.z = 0;
            Vector3 playerPos = ovrCameraRig.transform.position; // 플레이어 좌표 
            playerPos.z = 0;
            Vector3 dir = targetPos - playerPos; // 점프할 방향

            rigid.useGravity = true; // 중력 활성화

            rigid.velocity = Vector3.zero; // 점프 전 초기화  

            rigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
            rigid.AddForce(dir.normalized * leftJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
        }
        else if (!leftCol.activateJump) { Debug.LogWarning("Can't Left Jump"); }
    }

    /// <summary>
    /// 등반 중 우측 점프
    /// </summary>
    public void RightJump()
    {
        float upJumpForce = 1.5f; // 위로 점프하는 힘
        float leftJumpForce = 1f; // 좌측으로 점프하는 힘

        if (rightCol.activateJump) // 우측 점프가 가능한 상태라면 
        {
            Vector3 targetPos = rightCol.jumpPos; // 인식한 타겟 좌표
            targetPos.z = 0;
            Vector3 playerPos = ovrCameraRig.transform.position; // 플레이어 좌표 
            playerPos.z = 0;
            Vector3 dir = targetPos - playerPos; // 점프할 방향

            rigid.useGravity = true; // 중력 활성화

            rigid.velocity = Vector3.zero; // 점프 전 초기화  

            rigid.AddForce(Vector3.up * upJumpForce, ForceMode.Impulse); // 위쪽으로 점프
            rigid.AddForce(dir.normalized * leftJumpForce, ForceMode.Impulse); // 왼쪽으로 점프 
        }
        else if (!rightCol.activateJump) { Debug.LogWarning("Can't Right Jump"); }
    }
    #endregion

    #region 구현: 상승 점프 (Rigidbody)
    /// <summary>
    /// 상승 점프
    /// </summary>
    private void HighJump()
    {
        float jumpForce = 5f;

        // 두 손으로 암벽을 잡은 상태에서 아래로 강하게 휘두르면
        if ((LeftGrab && (leftVel.y <= -1f)) && (RightGrab && (rightVel.y <= -1f)))
        {
            superSwing = true;
        }
        
        // 슈퍼 점프가 가능한 상태 
        if (superSwing)
        {
            if (playerAction.Player.LeftGrip.ReadValue<float>() <= 0.45f &&
                playerAction.Player.RightGrip.ReadValue<float>() <= 0.45f) // 아래로 휘두르며 손을 놓았다면
            {
                rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 상승 점프
                superSwing = false;
            }

            Invoke("ClearHighJump", 0.7f); // 시간차 상승 점프 상태 해제 (그랩을 놓는 동작이 늦을 경우를 대비)
        }
    }

    /// <summary>
    /// 상승 점프 상태 해제 
    /// </summary>
    private void ClearHighJump() { superSwing = false; }
    #endregion

    #region 구현: 등반 중 추락
    /// <summary>
    /// 등반 중 추락
    /// </summary>
    private void Falling()
    {
        if (fallDown) // 만약 추락하는 중이라면
        {
            if (LeftGrab || RightGrab) // 왼손이나 오른손으로 절벽을 잡는 것에 성공했다면
            {
                rigid.velocity = Vector3.zero; // 가해지던 힘을 0으로 만듦
                rigid.useGravity = false; // 중력 해제 

                fallDown = false;
            }
        }
    }
    #endregion

    #region 구현: 마지막 등반 
    /// <summary>
    /// 마지막 등반 조건 (평지 오르기)
    /// </summary>
    private void LastClimbing(object sender, EventArgs e)
    {
        if (grabCheck_Left.finishHand && playerAction.Player.LeftGrip.ReadValue<float>() >= 0.5f)
        {
            if (leftVel.y <= -0.5f)
            {
                Vector3 grabPos = grabCheck_Left.finishGrabPos;
                StartCoroutine(LastClimbingAction(grabPos));
            }
        }

        if (grabCheck_Right.finishHand && playerAction.Player.RightGrip.ReadValue<float>() >= 0.5f)
        {
            if (rightVel.y <= -0.5f)
            {
                Vector3 grabPos = grabCheck_Right.finishGrabPos;
                StartCoroutine(LastClimbingAction(grabPos));
            }
        }
    }

    /// <summary>
    /// 마지막 등반 구현
    /// </summary>
    private IEnumerator LastClimbingAction(Vector3 _grabPos)
    {
        // TODO: 무한 루프가 발생해 일단 주석 처리했다. 

        //rigid.useGravity = false; // 중력 해제
        //Vector3 targetPos = _grabPos;
        //targetPos.y += 1;

        //bool climbing = true; // 평지 오르기 활성화

        //while(climbing) // 등반 동작이 끝나지 않은 동안 
        //{
        //    ovrCameraRig.transform.Translate(targetPos * 2f * Time.smoothDeltaTime); // 위치로 이동

        //    if (Vector3.Distance(ovrCameraRig.transform.position, targetPos) <= 0.1f) // 목표 위치에 거의 도착했다면
        //    {
        //        ovrCameraRig.transform.position = targetPos; // 위치 고정
        //        rigid.useGravity = true; // 중력 재활성화 

        //        climbing = false; // 평지 오르기 비활성화
        //    }
        //}

        yield return null;
    }
    #endregion

    private void Update()
    {
        LeftClimbing(); // 왼손 등반 (손 조작)
        RightClimbing(); // 오른손 등반 (손 조작)

        Climbing(); // 실 등반

        Falling(); // 추락

        HighJump(); // 상승 점프
    }
}
