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

    private float shiningInitialValue = 1.05f;

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

    /// <summary>
    /// 변기에 접근함
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("wc2")) // TODO: 이후 레이어마스크 이용 등 효율적인 방식으로 수정 필요 
        {
            Renderer renderer = other.GetComponent<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[materials.Length - 1].SetFloat("_Scale", shiningInitialValue); // Material Scale Up 
            }

            if (vrifAction.Player.Interaction.triggered || testAction.Test.Interaction.triggered)
            {
                VisitToilet(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("wc2")) // TODO: 이후 레이어마스크 이용 등 효율적인 방식으로 수정 필요 
        {
            Renderer renderer = other.GetComponent<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Reset 
            }
        }
    }

    /// <summary>
    /// 화장실 방문 메소드 
    /// </summary>
    private void VisitToilet(Transform toilet_)
    {
        // TODO: 변기 소리 추가 
        vrifStatusSystem.GetPoo(); // 배출 수치 초기화 
    }
}


