using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tool_Shavel : MonoBehaviour
{
    // 플레이어
    private GameObject player = default;
    // User System Manager
    [SerializeField]private GameObject userSystemManager = default;
    // 아이템 시스템
    ItemSystem itemSystem = default;

    private Vector3 rightVel = default;

    private void Start()
    {
        player = GameObject.Find("===Player==="); // TODO: 추후 문제가 될 수 있으니 수정 요함. 
        itemSystem = userSystemManager.GetComponent<ItemSystem>();
    }

    private void Update()
    {
        rightVel = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crop")) // 작물 콜라이더에 접촉한 상태에서
        {
            if (rightVel.y >= 0.3f) // 위로 삽질
            {
                GetCrop(); // 작물을 얻는다. 
            }
        }
    }

    private void GetCrop()
    {
        // TODO: 무슨 작물을 얻는지 추가되어야 한다. 
    }
}
