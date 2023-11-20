using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tool_Shavel : MonoBehaviour
{
    // �÷��̾�
    private GameObject player = default;
    // User System Manager
    [SerializeField]private GameObject userSystemManager = default;
    // ������ �ý���
    ItemSystem itemSystem = default;

    private Vector3 rightVel = default;

    private void Start()
    {
        player = GameObject.Find("===Player==="); // TODO: ���� ������ �� �� ������ ���� ����. 
        itemSystem = userSystemManager.GetComponent<ItemSystem>();
    }

    private void Update()
    {
        rightVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crop")) // �۹� �ݶ��̴��� ������ ���¿���
        {
            if (rightVel.y >= 0.3f) // ���� ����
            {
                GetCrop(); // �۹��� ��´�. 
            }
        }
    }

    private void GetCrop()
    {
        // TODO: ���� �۹��� ����� �߰��Ǿ�� �Ѵ�. 
    }
}
