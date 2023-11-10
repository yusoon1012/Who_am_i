using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlingShot_Shoot : MonoBehaviour
{
    #region �ʵ�
    private LineRenderer lineRenderer;
    [SerializeField] private Transform left = default;
    [SerializeField] private Transform middle = default; // ���� ����
    [SerializeField] private Transform right = default;

    [SerializeField] private OVRGrabbable middle_Grabbable = default; // ���� ���� Grabbable.cs
    [SerializeField] private Transform originMiddlePos = default; // ������ ���� ��ġ
    #endregion

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3; // ����� ����Ʈ�� �� ��   
    }

    /// <summary>
    /// ���� �׸���
    /// </summary>
    private void DrawString()
    {
        lineRenderer.SetPosition(0, left.position);
        lineRenderer.SetPosition(1, middle.position);
        lineRenderer.SetPosition(2, right.position);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void RestoreMiddle()
    {
        // TODO: �ڿ������� �߻��ϵ��� ���� ����
        middle.position = originMiddlePos.position;
    }

    private void ReleaseString()
    {
        // TODO: ���� �߻� �����ϱ�
    }

    private void Update()
    {
        DrawString();

        if (!middle_Grabbable.isGrabbed) { RestoreMiddle(); } // ������ ����� ���� ����

        if (middle_Grabbable.isGrabbed) // ������ ��� ����
        {
            if (!middle_Grabbable.isGrabbed) // ������ ������
            {
                Debug.Log("���� �߻�!"); 
                ReleaseString();
            } 
        }
    }
}
