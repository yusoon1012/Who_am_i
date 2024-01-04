using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Oculus.Interaction;

public class VRIFPlayerClimbingHelper : MonoBehaviour
{
    public static VRIFPlayerClimbingHelper Instance { get; private set; }

    [Header("Climbing Anchor")]
    [SerializeField] private Transform climbingAnchor = default;

    /// <Point> 등반 물체의 바로 위 부모 오브젝트가 암벽이라는 가정하에 짜인 코드이다. 이후 맵 구성 변경에 따라 수정 요함
    [Header("Grabbers")]
    [SerializeField] private Grabber leftGrabber = default;
    [SerializeField] private Grabber rightGrabber = default;

    // Transform: playerController
    private Transform playerController = default;
    // PlayerGravity
    private PlayerGravity playerGravity = default;

    [Header("Tracking Space")]
    [Tooltip("카메라가 실제로 비추는 것을 관할한다")]
    [SerializeField] private Transform trackingSpace = default;

    [Header("Sub Tracking Space")]
    [SerializeField] private Transform subTrackingSpace = default;

    [Header("Player Sub Camera")]
    [Tooltip("컷신용 카메라")]
    [SerializeField] Transform playerSubCamera = default;
    private CinemachineVirtualCamera virtualCamera = default;

    private bool posSetting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerController = transform;
        playerGravity = playerController.GetComponent<PlayerGravity>();

        virtualCamera = playerSubCamera.GetComponent<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// 등반 물체의 방향을 판단해 플레이어 앵커 설정 (암벽이 기준)
    /// </summary>
    public void SetAnchor(GameObject grabbable_)
    {
        if (grabbable_.CompareTag("ClimbingAnchor")) { Rotate(grabbable_.transform.GetChild(0)); } // 자동 회전

        if (grabbable_.GetComponentInChildren<CinemachineDollyCart>() && grabbable_.GetComponentInChildren<CinemachineSmoothPath>())
        {
            CinemachineSmoothPath path = grabbable_.GetComponentInChildren<CinemachineSmoothPath>(); // Dolly Track
            Transform dollyTrack = path.transform;
            Vector3 dir = dollyTrack.rotation.eulerAngles; // 트랙의 방향

            CinemachineDollyCart dollyCart = grabbable_.GetComponentInChildren<CinemachineDollyCart>(); // Dolly Cart
            Transform cart = dollyCart.transform; // 카트 트랜스폼

            subTrackingSpace.rotation = Quaternion.Euler(dir); // 서브 카메라는 트랙의 방향을 바라본다.  

            playerSubCamera.gameObject.SetActive(true);
            virtualCamera.Follow = cart;
            virtualCamera.LookAt = cart;

            dollyCart.m_Speed = 1f; // 카트 세팅 후 이동 시작

            StartCoroutine(CheckArrival(dollyCart, cart));
        }
    }

    /// <summary>
    /// 서브 카메라를 트랙을 따라 이동시킨다. 
    /// </summary>
    private IEnumerator CheckArrival(CinemachineDollyCart dollyCart_, Transform cart_)
    {
        yield return new WaitForSeconds(1.7f); // TODO: 카트 진행률로 교체 필요

        leftGrabber.ReleaseGrab(); // PC는 손을 놓는다
        rightGrabber.ReleaseGrab();

        playerController.position = playerSubCamera.position; // PC 재위치
        playerController.rotation = subTrackingSpace.rotation;

        playerGravity.enabled = false;
        posSetting = true;
        Invoke("Resetting", 0.15f); // 테스트

        playerSubCamera.gameObject.SetActive(false); // 카메라 재세팅

        virtualCamera.Follow = null;
        virtualCamera.LookAt = null;

        dollyCart_.m_Speed = 0f;
        dollyCart_.m_Position = 0f; // 카트 복귀
    }

    private void Rotate(Transform anchor_)
    {
        anchor_.LookAt(anchor_.parent);

        playerController.rotation = anchor_.rotation;
    }

    /// <Point> 코루틴을 통한 포지션 세팅에 문제가 있어 불가피하게 Update 사용
    private void Update()
    {
        if (posSetting) { playerController.position = playerSubCamera.position; }
    }

    private void Resetting() { posSetting = false; playerGravity.enabled = true; }
}