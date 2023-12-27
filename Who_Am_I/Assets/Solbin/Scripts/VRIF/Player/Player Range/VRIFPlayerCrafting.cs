using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFPlayerCrafting : MonoBehaviour
{
    [Header("UI Controller")]
    [SerializeField] private UIController uiController = default;

    private bool triggerEnter = false;

    private void Start()
    {
        

        VRIFInputSystem.Instance.interaction += ClickCrafting;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Colletor")) { triggerEnter = true; } // TODO: 이후 레이어 비교로 수정 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Colletor")) 
        {
            triggerEnter = false; 
        }
    }

    private void ClickCrafting(object sender, EventArgs e)
    {
        if (triggerEnter) // 조합대 영역에 진입한 상태라면 
        {
            uiController.OpenCraftingUI();
        }
    }
}
