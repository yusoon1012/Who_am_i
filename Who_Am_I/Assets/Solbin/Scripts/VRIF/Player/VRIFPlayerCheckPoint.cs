using BNG;
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
    private float shiningInitialValue = 1.06f;
    // VRIF Action
    private VRIFAction vrifAction = default;
    // CheckPoint UI
    [SerializeField] private GameObject checkPointUI = default;
    // VRIF State System
    [SerializeField] private VRIFStateSystem vrifStateSystem = default;
    // Select_SaveImage
    [SerializeField] private GameObject saveImage = default;
    // Select_ReturnImage
    [SerializeField] private GameObject returnImage = default;
    // 어느 선택지를 선택했는지 여부 (기본값 = 저장 후 종료 선택)
    private bool saveSelect = true;
    // 컨트롤러 입력 기본값
    private float input = 0.55f;

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            Renderer renderer = other.GetComponentInChildren<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[materials.Length - 1].SetFloat("_Scale", shiningInitialValue); // Material Scale Up 
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            if (vrifAction.Player.Interaction.triggered)
            {
                ActivateCheckPointUI(); // Activate CheckPoint UI
            }

            if (checkPointUI.activeSelf) // 만약 CheckPointUI가 활성화 되어있다면
            {
                UIControl();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            DeactivateCheckPointUI(); // Deactivate CheckPoint UI

            Renderer renderer = other.GetComponentInChildren<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Down
            }
        }
    }

    /// <summary>
    /// 체크포인트 UI 활성화
    /// </summary>
    private void ActivateCheckPointUI()
    {
        checkPointUI.SetActive(true);
        vrifStateSystem.ChangeState(VRIFStateSystem.GameState.UI); // 게임 UI 상태로 변경 
    }


    /// <summary>
    /// UI 컨트롤
    /// </summary>
    private void UIControl()
    {
        if (vrifAction.Player.LeftController.ReadValue<Vector2>().y <= -input ||
            vrifAction.Player.LeftController.ReadValue<Vector2>().y >= input) // Down
        {
            if (saveSelect) { saveSelect = false; }
            else { saveSelect = true; }
        }

        if (saveSelect) // 저장 선택 시 
        {
            saveImage.SetActive(true);
            returnImage.SetActive(false);

            if (vrifAction.Player.UI_Click.triggered) { PressSave(); }
        }
        else if (!saveSelect) // 되돌아가기 선택 시 
        {
            saveImage.SetActive(false);
            returnImage.SetActive(true);

            if (vrifAction.Player.UI_Click.triggered) { DeactivateCheckPointUI(); }
        }

    }

    /// <summary>
    /// 저장 버튼 연결
    /// </summary>
    public void PressSave()
    {
        // TODO: 구현된 저장 메소드로 연결 필요
        DeactivateCheckPointUI(); // 임시 연결 
    }

    /// <summary>
    /// 체크포인트 UI 비활성화
    /// </summary>
    public void DeactivateCheckPointUI()
    {
        checkPointUI.SetActive(false);
        vrifStateSystem.ChangeState(VRIFStateSystem.GameState.NORMAL); // 게임 NORMAL 상태로 변경 
    }

}