using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tool_SlingShot : MonoBehaviour
{
    #region 필드
    private LineRenderer whiteString; // 시위 라인렌더러
    [SerializeField] private Transform left = default;
    [SerializeField] private Transform middle = default; // 새총 시위
    [SerializeField] private Transform right = default;

    [SerializeField] private OVRGrabbable middle_Grabbable = default; // 새총 시위 Grabbable.cs
    [SerializeField] private Transform originMiddlePos = default; // 시위의 본래 위치
    [SerializeField] private Transform poiner = default; // 포인터(크로스 헤어)
    [SerializeField] private LineRenderer trajectory = default; // 새총 궤적


    private float distance = 8.5f;
    private bool aimingPrey = false; // 동물 조준 여부
    #endregion

    private void Start()
    {
        whiteString = GetComponent<LineRenderer>();
        whiteString.positionCount = 3; // 사용할 포인트는 두 개   

        trajectory.positionCount = 2; // 궤적에 사용할 포인트는 두 개 
    }

    /// <summary>
    /// 시위 그리기
    /// </summary>
    private void DrawString()
    {
        whiteString.SetPosition(0, left.position);
        whiteString.SetPosition(1, middle.position);
        whiteString.SetPosition(2, right.position);
    }

    /// <summary>
    /// 시위 복원
    /// </summary>
    private void RestoreMiddle()
    {
        // TODO: 자연스럽게 발사하도록 수정 요함
        middle.position = originMiddlePos.position;
        // 포인터 오브젝트 풀로
        poiner.position = PlayerSystem.poolPos;
        // 궤적 본위치로
        trajectory.SetPosition(0, PlayerSystem.poolPos);
        trajectory.SetPosition(1, PlayerSystem.poolPos);
    }

    private void Aiming()
    {
        Vector3 shootDir = originMiddlePos.position - middle.position; // 시위를 당긴 위치 - 시위 본위치
        trajectory.SetPosition(0, middle.position);

        Ray ray = new Ray(middle.position, shootDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            poiner.position = hit.point;
            trajectory.SetPosition(1, hit.point);
        }
    }

    private void Shoot()
    {
        Debug.Log("발사!");

        if (aimingPrey)
        {

        }
    }

    private void Update()
    {
        DrawString(); // 시위를 그린다

        if (!middle_Grabbable.isGrabbed) { RestoreMiddle(); } // 시위를 당기지 않은 상태

        if (middle_Grabbable.isGrabbed) // 시위를 당긴 상태
        {
            Aiming();

            if (!middle_Grabbable.isGrabbed) // 시위를 놓으면
            {
                Shoot();
            } 
        }
    }
}
