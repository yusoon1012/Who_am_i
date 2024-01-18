using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// (Player Controller - Player Range)
/// </summary>

public class VRIFPlayerCheckPoint : MonoBehaviour
{
    // Shining Material Value
    private float shiningInitialValue = 1.02f;
    // 범위 내에 있는지 판단
    private bool inTrigger = false;
    // 체크포인트
    private Transform checkPoint = default;

    private void Start()
    {
        VRIFInputSystem.Instance.interaction += PressInteraction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            if (!other.GetComponent<VRIFMap_CheckPoint>().activated) // 아직 비활성화된 체크포인트라면 
            {
                Renderer[] renderers = other.GetComponentsInChildren<Renderer>(); // 렌더러 찾기

                foreach (var renderer in renderers)
                {
                    Material[] materials = renderer.materials;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[materials.Length - 1].SetFloat("_Scale", shiningInitialValue); // Material Scale Up 
                    }
                }

                checkPoint = other.gameObject.transform;
                inTrigger = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            Renderer[] renderers = other.GetComponentsInChildren<Renderer>(); // 렌더러 찾기

            foreach (var renderer in renderers)
            {
                Material[] materials = renderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Down
                }
            }

            inTrigger = false;
            checkPoint = null;
        }
    }

    private void PressInteraction(object sender, EventArgs e)
    {
        if (inTrigger) // 만약 비활성화 체크포인트 범위 내에 있다면
        {
            checkPoint.GetComponent<VRIFMap_CheckPoint>().Activated(); // 활성화
        }
    }
}