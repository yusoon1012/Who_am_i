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
    // Test Action
    private TestAction testAction = default;
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
    private float input = 0.35f;
    // 키감 조절
    private bool inputDelay = false;

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
            if (vrifAction.Player.Interaction.triggered || testAction.Test.Interaction.triggered)
            {
                ActivateCheckPointUI(); // Activate CheckPoint UI
            }

            if (vrifAction.Player.UI_Click.triggered) { DeactivateCheckPointUI();} // 선택시 UI 비활성화
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
        VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.UI);
    }

    private void Update()
    {
        if (checkPointUI.activeSelf) // 만약 CheckPointUI가 활성화 되어있다면
        {
            UIControl();
        }
    }

    /// <summary>
    /// UI 컨트롤
    /// </summary>
    private void UIControl()
    {
        if (vrifAction.Player.LeftController.ReadValue<Vector2>().y <= -input ||
            vrifAction.Player.LeftController.ReadValue<Vector2>().y >= input) // Down
        {
            PressSave();
        }

        #region 컴퓨터 매핑
        if (testAction.Test.UpArrow.triggered || testAction.Test.DownArrow.triggered) // 테스트
        {
            if (saveSelect) { saveSelect = false; }
            else { saveSelect = true; }
        }

        if (saveSelect) // 저장 선택 시 
        {
            if (testAction.Test.Enter.triggered) { PressSave(); }
        }
        else if (!saveSelect) // 되돌아가기 선택 시 
        {
            if (testAction.Test.Enter.triggered) { DeactivateCheckPointUI(); }
        }
        #endregion
    }


    /// <summary>
    /// 저장 버튼 연결
    /// </summary>
    public void PressSave()
    {
        if (!inputDelay)
        {
            inputDelay = true;

            if (saveSelect) // 저장 선택시
            {
                saveImage.SetActive(true);
                returnImage.SetActive(false);

                saveSelect = false; // 다른 선택지로 변경
            }
            else // 되돌아가기 선택시
            {
                saveImage.SetActive(false);
                returnImage.SetActive(true);

                saveSelect = true; // 다른 선택지로 변경
            }

            // TODO: 구현된 저장 메소드로 연결 필요
 
            Invoke("ClearInputDelay", 0.5f);
        }
    }

    /// <summary>
    /// 체크포인트 UI 비활성화
    /// </summary>
    public void DeactivateCheckPointUI()
    {
        checkPointUI.SetActive(false);

        Invoke("ClearInputDelay", 0.5f);
        Invoke("ReturnNORMALState", 0.5f);
    }

    /// <summary>
    /// 키감 조절값 초기화
    /// </summary>
    private void ClearInputDelay() { inputDelay = false; }

    /// <summary>
    /// 바로 퀵슬롯이 입력을 받아 켜지는 것을 막기 위함
    /// </summary>
    private void ReturnNORMALState() { VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.NORMAL); }

    // TODO: 체크포인트 UI가 켜진 상태에서 a키를 누르면 UI가 꺼진 후 CLIMBING 입력을 받아 점프를 진행한다. 
}