using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    #region ���� ����

    // ���� ����
    Vector3 angle;

    // ���콺 ����
    private float sensitivity = default;
    // ���콺�� �¿� �Է� �ޱ�
    private float moveX = default;
    private float moveY = default;

    #endregion ���� ����

    void Awake()
    {
        // ���콺 ����
        sensitivity = 1000f;
        // ���콺�� �¿� �Է� �ޱ�
        moveX = 0f;
        moveY = 0f;
    }     // Awake()

    void Start()
    {
        // ������ �� ���� ī�޶� ������ ����
        angle.y = -Camera.main.transform.eulerAngles.x;
        angle.x = Camera.main.transform.eulerAngles.y;
        angle.z = Camera.main.transform.eulerAngles.z;
    }     // Start()

    void Update()
    {
        // ���콺 �������� �¿� �Է��� ����
        moveX = Input.GetAxis("Mouse X");
        moveY = Input.GetAxis("Mouse Y");

        // ���콺 ������ ������ ����
        angle.x += moveX * sensitivity * Time.deltaTime;
        angle.y += moveY * sensitivity * Time.deltaTime;

        // ������ ������ Axis �� angle �� ����� ȸ��
        transform.eulerAngles = new Vector3(-angle.y, angle.x, angle.z);
    }     // Update()
}
