using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing_FixGrabbable : MonoBehaviour
{
    private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    private OVRGrabbable grabbable;

    private void Awake()
    {
        defaultRotation = transform.rotation;
        defaultPosition = transform.position;

        grabbable = GetComponent<OVRGrabbable>();
    }

    private void LateUpdate()
    {
        transform.rotation = defaultRotation;
        transform.position = defaultPosition;

        // TODO: �� �޼��� ��ü�� ����� ���� �������� ��ü�� ����� �� ���� ������ �ٸ��� �˾Ƴ��� �����ؾ� �Ѵ�. 
        // => Physics ���� vs. ��ũ��Ʈ ���� ��� ����غ���
        if (grabbable.isGrabbed)
        {
            Debug.Log("��Ҵ�.");
        }
    }
}
