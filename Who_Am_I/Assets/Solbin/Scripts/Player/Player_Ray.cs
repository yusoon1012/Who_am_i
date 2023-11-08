using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    #region �ʵ�
    public static Player_Ray instance;

    //������ ��Ʈ�ѷ�
    [SerializeField] private Transform rightController = default;
    // VR ������ ��ġ
    [SerializeField] protected Transform pointer = default;
    // �� ���̾�
    private int groundLayer = default;
    // Ray�� ���� ����Ű���� Ȯ��
    protected bool itGround = false;
    #endregion

    private void Awake() { instance = this; }

    private void Start() { groundLayer = 1 << LayerMask.NameToLayer("Ground"); } // CHECK: ���� üũ

    private void DrawRay()
    {
        if (rightController.gameObject.activeSelf) // ���� ��Ʈ�ѷ� Ȱ��ȭ ����
        {
            Vector3 rightControllerPos = rightController.position;

            Ray ray = new Ray(rightControllerPos, rightController.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                pointer.position = hit.point; // ������ ��Ÿ����

                if (hit.transform.gameObject.layer == groundLayer) // �� ���̾�� �浹
                {
                    itGround = true;
                }
                else itGround = false;

                // TODO: UI�� �浹 Ȯ��
            }
        }
    }

    /// <summary>
    /// �����ʹ� �÷��̾ �ٶ󺻴�
    /// </summary>
    private void PointerLook()
    {
        pointer.LookAt(transform.position);
    }

    private void Update()
    {
        DrawRay();
        PointerLook();
    }
}
