using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Gloves : MonoBehaviour
{
    // ���� �尩
    [SerializeField] Transform leftGlove = default;
    // ������ �尩
    [SerializeField] Transform rightGlove = default;
    // ���� ��Ŀ
    [SerializeField] Transform leftAnchor = default;
    // ������ ��Ŀ
    [SerializeField] Transform rightAnchor = default;

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// (�ʱ� ����)
    /// </summary>
    private void Setting()
    {
        leftGlove.gameObject.SetActive(false);
        rightGlove.gameObject.SetActive(false);

        leftGlove.SetParent(leftAnchor); // ���� �尩 ���� �� ��Ŀ ������
        rightGlove.SetParent(rightAnchor); // ������ �尩 ������ �� ��Ŀ ������
    }

    // TODO: �尩�� ������ => ���� ������ ��Ȱ��ȭ, OVRGrabbable ��Ȱ��ȭ, ���� ������ Ȱ��ȭ. 
    // TODO: �尩�� ���°���? => ��ȹ���� ���� 
}
