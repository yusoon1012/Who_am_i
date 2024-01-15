using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 삽의 칼날(Blade Collider)에 붙어 땅을 파도록 기능하는 스크립트
/// </summary>
public class VRIFTool_Shovel : MonoBehaviour
{
    // Audio Source
    private AudioSource audioSource = default;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Mound")) // TODO: 이후 합의 시 레이어 등으로 교체 
        {
            if (VRIFInputSystem.Instance.rVelocity.y >= 0.03f) // 위로 삽질
            {
                Digging(other.gameObject);
            }
        }
    }

    private void Digging(GameObject _dirt)
    {
        audioSource.Play();
        _dirt.transform.localScale /= 2;
        _dirt.GetComponent<VRIFMap_DirtFile>().AddDestroy(); // 파괴 횟수 증가 
    }
}
