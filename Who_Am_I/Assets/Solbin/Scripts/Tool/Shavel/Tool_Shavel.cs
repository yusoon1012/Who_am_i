using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tool_Shavel : MonoBehaviour
{
    private GameObject player = default;
    Player_Item playerItem = default;

    private Vector3 rightVel = default;

    private void Start()
    {
        player = GameObject.Find("===Player==="); // TODO: 추후 문제가 될 수 있으니 수정 요함. 
        playerItem = player.GetComponent<Player_Item>();
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
                playerItem.GetCrop();
            }
        }
    }
}
