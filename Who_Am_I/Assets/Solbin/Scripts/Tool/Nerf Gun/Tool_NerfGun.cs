using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Tool_NerfGun : MonoBehaviour
{
    // �������� ������� ���θ� ����
    OVRGrabbable ovrGrabbable = default;
    // ���� �߻� ����
    [SerializeField] Transform firePos = default;
    // �����Ÿ�
    private float distance = 8.5f;
    // ������(ũ�ν� ���)
    [SerializeField] Transform pointer = default;
    // ���� ���� ����(���� ����)
    private bool aimingPrey = false;
    // ������ ����
    [SerializeField] LineRenderer trajectory = default;

    private void Start()
    {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        trajectory.positionCount = 2; // ������ ����Ʈ�� �� �� 
    }

    private void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * distance, Color.green);

        if (ovrGrabbable.isGrabbed) // ���� �������� ����� �����϶�
        {
            Aiming(); // �������� �����Ѵ�

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) { Shoot(); }
        }
        else { Restore(); }
    }

    /// <summary>
    /// �������� ����, ũ�ν� �� ǥ���Ѵ�. 
    /// </summary>
    private void Aiming()
    {
        trajectory.SetPosition(0, firePos.position);

        Ray ray = new Ray(firePos.position, firePos.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            pointer.position = hit.point;
            trajectory.SetPosition(1, hit.point);

            // �ӽ� �ּ�
            //if (hit.transform.CompareTag("Animal")) // TODO: �Ŀ� ������ �±׿� ���� ����
            //{
            //    aimingPrey = true; // ���� ���� O
            //}
            //else
            //{
            //    aimingPrey = false; // ���� ���� X
            //}
        }
    }

    /// <summary>
    /// ���� �߻��Ѵ�. 
    /// </summary>
    private void Shoot()
    {
        Debug.Log("�߻�!");
        // TODO: ���̰� ���� �κ�, �ѱ��� ��ƼŬ(Ȥ�� �ٸ� ȿ��) �߻�
    }

    /// <summary>
    /// �������� ������ ���°� �ƴ� �� �缼��
    /// </summary>
    private void Restore()
    {
        trajectory.SetPosition(0, PlayerSystem.poolPos);
        trajectory.SetPosition(1, PlayerSystem.poolPos);
    }
}
