using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;


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
    }

    /// <summary>
    /// 등반 물체의 방향을 판단해 플레이어 앵커 설정 (암벽이 기준)
    /// </summary>
    public void SetAnchor(GameObject _grabbable)
    {
        if (_grabbable.CompareTag("ClimbingAnchor"))
        {
            Transform anchor = _grabbable.transform.GetChild(0); // 자식 오브젝트 Anchor의 위치로 이동 

            climbingAnchor.position = anchor.position;
            climbingAnchor.LookAt(_grabbable.transform);

            // playerController.rotation = climbingAnchor.rotation;

            StartCoroutine(Rotate());
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

        playerController.rotation = climbingAnchor.rotation;

        yield return null;
    }
}

// TODO: 어떻게 하면 자연스럽게 회전하도록 할 수 있는가? (y축만 회전하도록)

