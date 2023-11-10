using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Laser : MonoBehaviour
{
    #region �ʵ�
    private Transform player = default;
    //������ ��ġ(��Ʈ�ѷ� & �ո�)
    [SerializeField] private Transform rightAnchor = default;
    // VR ������ ��ġ
    [SerializeField] protected Transform pointer = default;
    // �� ���̾�
    private int groundLayer = default;
    // UI ���̾�
    private int UILayer = default;
    // ���� ������ ������Ʈ
    private LineRenderer lineRenderer = default;
    // ������ �ִ� �Ÿ�(�ڷ���Ʈ �ִ� �Ÿ�)
    private float rayDistance = default;
    // �÷��̾� �ý���(��Ʈ�ѷ� or �� ��� ����)
    [SerializeField] private PlayerSystem playerSystem = default;
    #endregion

    private void Start() { Setting(); }

    /// <summary>
    /// (�ʱ� ����)���̾��ũ, ������Ʈ ����
    /// </summary>
    private void Setting()
    {
        player = transform.GetChild(0);
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        UILayer = 1 << LayerMask.NameToLayer("UI");
        lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // ���� �� ��
        rayDistance = Player_Status.playerStat.teleportDistance;
    }

    /// <summary>
    /// (����) �����տ��� ���� �߻�
    /// </summary>
    private void ShootRay()
    {
        // ���� ������ �߻�
        lineRenderer.SetPosition(0, rightAnchor.position);
        Debug.DrawRay(rightAnchor.position, rightAnchor.forward * rayDistance, Color.green);
        Ray ray = new Ray(rightAnchor.position, rightAnchor.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, groundLayer)) // ���� ���̰� �ε�������
        {
            GroundRay(hit);
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, UILayer)) // UI�� ���̰� �ε�������
        {
            UIRay(hit);
        }
        else
            lineRenderer.enabled = false; // �̿��� ��Ȳ�� ���� ������ ��Ȱ��ȭ
    }

    /// <summary>
    /// (����) �����̾ �浹
    /// </summary>
    private void GroundRay(RaycastHit _hit)
    {
        if (!playerSystem.handActivate)
        {
            RaycastHit hit = _hit;

            lineRenderer.enabled = true; // ���� ������ Ȱ��ȭ

            lineRenderer.SetPosition(1, hit.point);
            pointer.position = hit.point;

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                //TODO: �ڷ���Ʈ �� ��ġ���� �˸��� ȿ�� �߰�
                Vector3 teleportPos = hit.point;
                teleportPos.y += 1;

                player.position = teleportPos;
            }
        }
        else { lineRenderer.enabled = false; }
    }

    /// <summary>
    /// (����) UI���̾ �浹
    /// </summary>
    private void UIRay(RaycastHit _hit)
    {
        RaycastHit hit = _hit;

        lineRenderer.enabled = true; // ���� ������ Ȱ��ȭ

        lineRenderer.SetPosition(1, hit.point);
        pointer.position = hit.point;   

        Debug.Log("UI �浹 üũ");
    }

    private void Update()
    {
        ShootRay();
    }
}
