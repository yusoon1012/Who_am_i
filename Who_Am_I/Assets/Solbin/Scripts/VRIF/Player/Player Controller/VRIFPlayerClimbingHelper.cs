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

            playerController.rotation = climbingAnchor.rotation;
        }
    }

    private IEnumerator Test()
    {
        bool test = true;

        while (test)
        {
            Debug.LogWarning(playerController.position);

            yield return null;
        }
    }
}

