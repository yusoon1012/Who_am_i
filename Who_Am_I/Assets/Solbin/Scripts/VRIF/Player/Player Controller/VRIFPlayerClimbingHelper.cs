using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;
using System.Linq;
using Cinemachine;

public class VRIFPlayerClimbingHelper : MonoBehaviour
{
    public static VRIFPlayerClimbingHelper Instance { get; private set; }

    [Header("Climbing Anchor")]
    [SerializeField] private Transform climbingAnchor = default;

    /// <Point> 등반 물체의 바로 위 부모 오브젝트가 암벽이라는 가정하에 짜인 코드이다. 이후 맵 구성 변경에 따라 수정 요함
    [Header("Grabbers")]
    [SerializeField] private Grabber leftGrabber = default;
    [SerializeField] private Grabber rightGrabber = default;

    private Transform playerController = default;

    [Header("Tracking Space")]
    [Tooltip("카메라가 실제로 비추는 것을 관할한다")]
    [SerializeField] private Transform trackingSpace = default;

    [Header("Player Sub Camera")]
    [Tooltip("컷신용 카메라")]
    [SerializeField] Transform playerSubCamera = default;
    private CinemachineVirtualCamera virtualCamera = default;

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
        virtualCamera = playerSubCamera.GetComponent<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// 등반 물체의 방향을 판단해 플레이어 앵커 설정 (암벽이 기준)
    /// </summary>
    public void SetAnchor(GameObject grabbable_)
    {
        if (grabbable_.CompareTag("ClimbingAnchor")) { StartCoroutine(Rotate()); } // 자동 회전

        if (grabbable_.GetComponentInChildren<CinemachineDollyCart>())
        {
            Transform cart = grabbable_.GetComponentInChildren<CinemachineDollyCart>().transform; // 카트 트랜스폼
            CinemachineDollyCart dollyCart = cart.GetComponent<CinemachineDollyCart>();

            playerSubCamera.gameObject.SetActive(true);
            virtualCamera.Follow = cart;
            virtualCamera.LookAt = cart;

            dollyCart.m_Speed = 1f;

            StartCoroutine(CheckArrival(dollyCart));
        }
    }

    /// <summary>
    /// 서브 카메라를 트랙을 따라 이동시킨다. 
    /// </summary>
    private IEnumerator CheckArrival(CinemachineDollyCart dollyCart_)
    {
        while(dollyCart_.m_Position <= 1.9) // 도착 직전까지
        {
            yield return null;
        }

        leftGrabber.ReleaseGrab(); // 손을 놓는다
        rightGrabber.ReleaseGrab();

        playerController.position = dollyCart_.transform.position; // PC 재위치

        playerSubCamera.gameObject.SetActive(false); // 카메라 재세팅
        virtualCamera.Follow = null;
        virtualCamera.LookAt = null;

        dollyCart_.m_Speed = 0f;
        dollyCart_.m_Position = 0f; // 카트 복귀

        // TODO: VR 기기를 쓴 채로 오른쪽으로 머리를 기울이면 화면 또한 오른쪽으로, 좌측으로 머리를 기울이면 화면 또한 좌측으로 기울어지는
        // 문제가 있다. centereyeanchor는 카메라를 어떻게 보정하는가?
        // 그럼 왜 UI 카메라는 멀쩡히 작동하는 것인가?
    }

    private IEnumerator Rotate()
    {
        //float time = 0f;

        //while (time < 1f)
        //{
        //    time += Time.deltaTime;

        //    playerController.rotation = 
        //        Quaternion.RotateTowards(playerController.rotation, climbingAnchor.rotation, 100f * Time.deltaTime);

        //    playerController.rotation = Quaternion.Euler(0, playerController.rotation.y, 0);

        //    yield return null;
        //}

        //while (Vector3.Distance(playerController.position, climbingAnchor.position) > 0.1f)
        //{
        //    playerController.position = Vector3.MoveTowards(playerController.position, climbingAnchor.position, 10f * Time.deltaTime);

        //    yield return null;
        //}

        Vector3 dir = climbingAnchor.rotation.eulerAngles;
        dir.x = 0;
        dir.z = 0;

        playerController.rotation = Quaternion.Euler(dir);

        yield return null;
    }
}

// TODO: 어떻게 하면 자연스럽게 회전하도록 할 수 있는가? (y축만 회전하도록)

