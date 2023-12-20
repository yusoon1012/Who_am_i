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
        if (other.name.Contains("Dirt File")) // TODO: 이후 합의 시 레이어 등으로 교체 
        {
            if (VRIFInputSystem.Instance.rVelocity.y >= 0.03f) // 위로 삽질
            {
                Digging(other.gameObject);
            }
        }
    }

    private void Digging(GameObject _dirt)
    {
        _dirt.transform.localScale /= 2;
        _dirt.GetComponent<VRIFMap_DirtFile>().AddDestroy(); // 파괴 횟수 증가 
    }
}
