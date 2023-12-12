using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Threading;
using UnityEngine;

public class VRIFInputSystem : MonoBehaviour
{
    //인스턴스
    public static VRIFInputSystem Instance { get; private set; }
    // VRIF Action
    private VRIFAction vrifAction = default;

#if UNITY_EDITOR
    // Test Action
    private TestAction testAction = default;
#endif // UNITY_EDITOR

    [Tooltip("키입력 이벤트")]
    // 슬로우 모드
    public event EventHandler slowMode;
    public event EventHandler interaction;
    public event EventHandler uiClick;

    [Tooltip("양쪽 컨트롤러")]
    public Transform lController;
    public Transform rController;

    [Tooltip("컨트롤러 입력값")]
    // 왼쪽 컨트롤러 Velocity
    public Vector3 lVelocity { get; private set; }
    // 오른쪽 컨트롤러 Velocity
    public Vector3 rVelocity { get; private set; }

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();

#if UNITY_EDITOR
        testAction = new TestAction();
        testAction.Enable();
#endif // UNITY_EDITOR
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        VREventSetting();

#if UNITY_EDITOR
        TestEventSetting();
#endif // UNITY_EDITOR
    }

    #region 단발성 입력(버튼) 체크
    private void VREventSetting()
    {
        vrifAction.Player.SlowMode.performed += ctx => SlowMode();
        vrifAction.Player.Interaction.performed += ctx => Interaction();
        vrifAction.Player.UI_Click.performed += ctx => UIClick();
    }

#if UNITY_EDITOR
    private void TestEventSetting()
    {
        testAction.Test.Interaction.performed += ctx => Interaction();
    }
#endif // UNITY_EDITOR

    private void SlowMode() { slowMode?.Invoke(this, EventArgs.Empty); }
    private void Interaction() { interaction?.Invoke(this, EventArgs.Empty); }
    private void UIClick() { uiClick?.Invoke(this, EventArgs.Empty); }
    #endregion

    #region 지속 입력(velocity, 조이스틱) 체크
    private void FixedUpdate()
    {
        DeviceVelocity();
    }

    /// <summary>
    /// 컨트롤러 디바이스의 Velocity 측정 
    /// </summary>
    private void DeviceVelocity()
    {
        lVelocity = vrifAction.Player.LeftVelocity.ReadValue<Vector3>();
        rVelocity = vrifAction.Player.RightVelocity.ReadValue<Vector3>();
    }

    #endregion
}
