using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFPlayerToilet : MonoBehaviour
{
    // VRIF Action
    private VRIFAction vrifAction;
    // Test Action
    private TestAction testAction;
    // VRIF Status System
    [SerializeField] private VRIFStatusSystem vrifStatusSystem = default;

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();

        testAction = new TestAction();
        testAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
        testAction?.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Toilet")) // TODO: 이후 레이어마스크 이용 등 효율적인 방식으로 수정 필요 
        {
            if (vrifAction.Player.Interaction.triggered || testAction.Test.Interaction.triggered)
            {
                VisitToilet();
            }
        }
    }

    /// <summary>
    /// 화장실 방문 메소드 
    /// </summary>
    private void VisitToilet()
    {
        vrifStatusSystem.m_Poo = 0; // 배출 수치 초기화 
        Debug.Log("플레이어는 장을 비웠다. 용변 수치: " + vrifStatusSystem.m_Poo);
    }
}


