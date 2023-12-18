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
            Renderer renderer = other.GetComponent<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[materials.Length - 1].SetFloat("_Scale", shiningInitialValue); // Material Scale Up 
            }

            checkPoint = other.gameObject.transform;
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            Renderer renderer = other.GetComponent<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Down
            }

            inTrigger = false;
        }
    }

    private void PressInteraction(object sender, EventArgs e)
    {
        if (inTrigger) // 만약 체크포인트 범위 내에 있다면
        {
            // TODO: 체크포인트의 스크립트 이용해 빛을 켜고 활성화시키기
        }
    }
}