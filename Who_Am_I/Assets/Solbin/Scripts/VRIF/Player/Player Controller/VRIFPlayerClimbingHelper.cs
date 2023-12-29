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

    [Header("Climbing Camera")]
    [SerializeField] private Transform ClimbingCamera = default;
    private CinemachineDollyCart dollyCart = default;

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
        dollyCart = ClimbingCamera.GetComponent<CinemachineDollyCart>();
    }

    /// <summary>
    /// 등반 물체의 방향을 판단해 플레이어 앵커 설정 (암벽이 기준)
    /// </summary>
    public void SetAnchor(GameObject grabbable_)
    {
        //if (grabbable_.transform.GetChild(0) != null)
        //{
        //    Transform anchor = grabbable_.transform.GetChild(0); // 자식 오브젝트 Anchor의 위치로 이동 

        //    climbingAnchor.position = anchor.position;
        //    climbingAnchor.LookAt(grabbable_.transform);

        if (grabbable_.CompareTag("ClimbingAnchor")) { StartCoroutine(Rotate()); } // 자동 회전

        // TODO: 왼손인가 오른손인가... 

        //Vector3 leftPos = (leftGrabber.transform.position + transform.position) / 2;
        //leftPos.y = grabbable_.transform.position.y;

        //Vector3 rightPos = (rightGrabber.transform.position + transform.position) / 2;
        //rightPos.y = grabbable_.transform.position.y;

        //// TODO: 임시 게임오브젝트가 등반물체를 바라보고, 그 축이 점프 축이 된다.

        //GameObject leftAnchor = new GameObject();
        //leftAnchor.transform.position = leftPos;

        //GameObject rightAnchor = new GameObject();
        //rightAnchor.transform.position = rightPos;

        if (grabbable_.GetComponentInChildren<CinemachineSmoothPath>()) // 마지막 등반 물체가 트랙을 가지고 있으면
        {
            ClimbingCamera.gameObject.SetActive(true);
            dollyCart.m_Path = grabbable_.GetComponentInChildren<CinemachineSmoothPath>(); // Path 할당
            dollyCart.m_Speed = 1;
        }
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

