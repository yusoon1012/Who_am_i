using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;
using System.Linq;
using Cinemachine;
using Yarn.Unity.Editor;
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

    [Header("테스트")]
    [SerializeField] private Transform testCart = default;
    [SerializeField] private Transform testSphere = default;

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
        if (grabbable_.CompareTag("ClimbingAnchor")) { StartCoroutine(Rotate()); } // 자동 회전

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
        yield return new WaitForSeconds(2.5f);

        playerGravity.enabled = false; // 중력 비활성화

        leftGrabber.ReleaseGrab(); // PC는 손을 놓는다
        rightGrabber.ReleaseGrab();

        playerController.position = playerSubCamera.position; // PC 재위치
        playerController.rotation = subTrackingSpace.rotation;

        playerGravity.enabled = true; // 중력 재활성화

        playerSubCamera.gameObject.SetActive(false); // 카메라 재세팅

        virtualCamera.Follow = null;
        virtualCamera.LookAt = null;

        dollyCart_.m_Speed = 0f;
        dollyCart_.m_Position = 0f; // 카트 복귀
    }

    private IEnumerator Rotate()
    {
        Vector3 dir = climbingAnchor.rotation.eulerAngles;
        dir.x = 0;
        dir.z = 0;

        playerController.rotation = Quaternion.Euler(dir);

        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            playerController.position = testCart.position;
            //StartCoroutine(TestCode());
        } // TODO: 돌리카트가 초기화되고 나서 플레이어가 이동을 시도한다. 고치기
    }

    private IEnumerator TestCode()
    {
        yield return new WaitForSeconds(2);

        bool test = true;
        
        while (test)
        {
            Debug.Log("이동 시도");

            playerController.position = testCart.position;

            yield return null;
        }
    }
}