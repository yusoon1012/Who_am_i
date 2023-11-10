using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlingShot_Shoot : MonoBehaviour
{
    #region 필드
    private LineRenderer lineRenderer;
    [SerializeField] private Transform left = default;
    [SerializeField] private Transform middle = default; // 새총 시위
    [SerializeField] private Transform right = default;

    [SerializeField] private OVRGrabbable middle_Grabbable = default; // 새총 시위 Grabbable.cs
    [SerializeField] private Transform originMiddlePos = default; // 시위의 본래 위치
    #endregion

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3; // 사용할 포인트는 두 개   
    }

    /// <summary>
    /// 시위 그리기
    /// </summary>
    private void DrawString()
    {
        lineRenderer.SetPosition(0, left.position);
        lineRenderer.SetPosition(1, middle.position);
        lineRenderer.SetPosition(2, right.position);
    }

    /// <summary>
    /// 시위 복원
    /// </summary>
    private void RestoreMiddle()
    {
        // TODO: 자연스럽게 발사하도록 수정 요함
        middle.position = originMiddlePos.position;
    }

    private void ReleaseString()
    {
        // TODO: 새총 발사 구현하기
    }

    private void Update()
    {
        DrawString();

        if (!middle_Grabbable.isGrabbed) { RestoreMiddle(); } // 시위를 당기지 않은 상태

        if (middle_Grabbable.isGrabbed) // 시위를 당긴 상태
        {
            if (!middle_Grabbable.isGrabbed) // 시위를 놓으면
            {
                Debug.Log("새총 발사!"); 
                ReleaseString();
            } 
        }
    }
}
