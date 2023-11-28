using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTool_Shavel : MonoBehaviour
{
    // User System Manager
    [SerializeField] private GameObject userManager = default;
    // VRIF ItemSystem
    VRIFItemSystem vrifItemSystem = default;

    private void Start()
    {
        vrifItemSystem = userManager.GetComponent<VRIFItemSystem>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crop")) // 작물 콜라이더에 접촉한 상태에서
        {
            if (VRIFControllerSystem.rVelocity.y >= 0.3f) // 위로 삽질
            {
                string cropName = other.name; // 작물의 이름 전달 
                GetCrop(cropName); // 작물을 얻는다. 
            }
        }
    }

    private void GetCrop(string _name)
    {
        // TODO: 무슨 작물을 얻는지 추가되어야 한다. (작물의 구분은 이름? 태그?)
        Debug.LogWarning("Get Crop!");
    }
}
