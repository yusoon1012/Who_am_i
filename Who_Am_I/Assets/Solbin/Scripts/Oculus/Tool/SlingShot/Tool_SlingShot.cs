using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tool_SlingShot : MonoBehaviour
{
    #region �ʵ�
    private LineRenderer whiteString; // ���� ���η�����
    [SerializeField] private Transform left = default;
    [SerializeField] private Transform middle = default; // ���� ����
    [SerializeField] private Transform right = default;

    [SerializeField] private OVRGrabbable middle_Grabbable = default; // ���� ���� Grabbable.cs
    [SerializeField] private Transform originMiddlePos = default; // ������ ���� ��ġ
    [SerializeField] private Transform poiner = default; // ������(ũ�ν� ���)
    [SerializeField] private LineRenderer trajectory = default; // ���� ����


    private float distance = 8.5f;
    private bool aimingPrey = false; // ���� ���� ����
    #endregion

    private void Start()
    {
        whiteString = GetComponent<LineRenderer>();
        whiteString.positionCount = 3; // ����� ����Ʈ�� �� ��   

        trajectory.positionCount = 2; // ������ ����� ����Ʈ�� �� �� 
    }

    /// <summary>
    /// ���� �׸���
    /// </summary>
    private void DrawString()
    {
        whiteString.SetPosition(0, left.position);
        whiteString.SetPosition(1, middle.position);
        whiteString.SetPosition(2, right.position);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void RestoreMiddle()
    {
        // TODO: �ڿ������� �߻��ϵ��� ���� ����
        middle.position = originMiddlePos.position;
        // ������ ������Ʈ Ǯ��
        poiner.position = PlayerSystem.poolPos;
        // ���� ����ġ��
        trajectory.SetPosition(0, PlayerSystem.poolPos);
        trajectory.SetPosition(1, PlayerSystem.poolPos);
    }

    private void Aiming()
    {
        Vector3 shootDir = originMiddlePos.position - middle.position; // ������ ��� ��ġ - ���� ����ġ
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
        Debug.Log("�߻�!");

        if (aimingPrey)
        {

        }
    }

    private void Update()
    {
        DrawString(); // ������ �׸���

        if (!middle_Grabbable.isGrabbed) { RestoreMiddle(); } // ������ ����� ���� ����

        if (middle_Grabbable.isGrabbed) // ������ ��� ����
        {
            Aiming();

            if (!middle_Grabbable.isGrabbed) // ������ ������
            {
                Shoot();
            } 
        }
    }
}
