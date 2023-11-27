using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Gloves : MonoBehaviour
{
    // ���� �尩 (����� Ȱ��ȭ)
    [SerializeField] Transform leftGlove = default;
    // ������ �尩 (����� Ȱ��ȭ)
    [SerializeField] Transform rightGlove = default;
    // ���� ��Ŀ (����� �θ�)
    [SerializeField] Transform leftAnchor = default;
    // ������ ��Ŀ (����� �θ�)
    [SerializeField] Transform rightAnchor = default;
    // ���� �尩 (������� Ȱ��ȭ)
    [SerializeField] Transform leftItem = default;
    // ������ �尩 (������� Ȱ��ȭ)
    [SerializeField] Transform rightItem = default;
    // �� ������ ������Ʈ (������� �θ�)
    [SerializeField] Transform gloves = default;
    // �÷��̾�
    [SerializeField] Transform player = default;
    // OVRGrabbable
    OVRGrabbable ovrGrabbable = default;

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// (�ʱ� ����)
    /// </summary>
    private void Setting()
    {
        leftItem.gameObject.SetActive(true);
        rightItem.gameObject.SetActive(true);

        leftGlove.gameObject.SetActive(false);
        rightGlove.gameObject.SetActive(false);

        ovrGrabbable = GetComponent<OVRGrabbable>();
    }

    /// <summary>
    /// �尩 ����
    /// </summary>
    private void GetGloves()
    {
        // TODO: �尩 ����, ��ġ�� �� �𵨰� ��ġ���Ѿ� �Ѵ�. 

        Debug.Log("�尩 ������");

        leftGlove.SetParent(leftAnchor); // ���� �尩 ���� �� ��Ŀ ������
        rightGlove.SetParent(rightAnchor); // ������ �尩 ������ �� ��Ŀ ������

        leftGlove.gameObject.SetActive(true); // �尩 Ȱ��ȭ
        rightGlove.gameObject.SetActive(true);

        leftItem.gameObject.SetActive(false); // ������ ��Ȱ��ȭ
        rightItem.gameObject.SetActive(false);
    }

    /// <summary>
    /// �尩 ����
    /// </summary>
    private void TakeOffGloves()
    {
        leftGlove.SetParent(gloves); // ��Ŀ ����
        rightGlove.SetParent(gloves);

        leftGlove.gameObject.SetActive(false); // �尩 ��Ȱ��ȭ
        rightGlove.gameObject.SetActive(false);

        leftItem.gameObject.SetActive(true); // ������ Ȱ��ȭ
        rightItem.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(ovrGrabbable.isGrabbed) // �尩�� ����
        {
            GetGloves();
        }
    }

    // TODO: �尩�� ������ => ���� ������ ��Ȱ��ȭ, OVRGrabbable ��Ȱ��ȭ, ���� ������ Ȱ��ȭ. 
    // TODO: �尩�� ���°���? => ��ȹ���� ���� 
    // TODO: �尩�� ������ ���¿��� ä���� ������� �Ϸ���? ������ ���¿��� OverlapSphere�� �˻� => ���� ä�� ������ ���� ��ó��
    // ������ �´� Ű�� �����ٸ�...?

    // ä�� ���� ���� 
    private void FindForaging()
    {
        int radius = 10;
        Collider[] foraging = Physics.OverlapSphere(player.position, radius); // �ݰ� �� ä��ǰ �˻�

        // TODO: ä��ǰ ȹ��, ����� ������?
        Debug.Log("ä��ǰ ȹ��!");
    }
}
