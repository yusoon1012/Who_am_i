using OVR.OpenVR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRIFTool_NerfGun : MonoBehaviour
{
    // VRIF Action
    private VRIFAction vrifAction = default;
    // Test Action
    private TestAction testAction = default;
    // 레이 발사 지점
    [SerializeField] Transform firePos = default;
    // 사정거리
    private float distance = 8.5f;
    // 포인터(크로스 헤어)
    [SerializeField] Transform pointer = default;
    // 너프건 궤적
    [SerializeField] LineRenderer trajectory = default;

    public static event EventHandler slowTimeEvent;

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// 초기세팅
    /// </summary>
    private void Setting()
    {
        trajectory.positionCount = 2; // 궤적의 포인트는 두 개 
    }

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

    private void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * distance, Color.green);

        Aiming();

        ActivateSlowTime();
    }

    /// <summary>
    /// 너프건을 조준, 크로스 헤어를 표시한다. 
    /// </summary>
    private void Aiming()
    {
        trajectory.SetPosition(0, firePos.position);

        Ray ray = new Ray(firePos.position, firePos.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            pointer.position = hit.point;
            trajectory.SetPosition(1, hit.point);

            if (vrifAction.Player.RightTrigger.triggered || testAction.Test.Click.triggered)
            { Shoot(hit.transform.gameObject); }
        }
    }

    /// <summary>
    /// 총을 발사한다. 
    /// </summary>
    private void Shoot(GameObject _prey)
    {
        if (_prey.layer == LayerMask.NameToLayer("Animal"))
        {
            if (_prey.GetComponent<ThisData>()) // 데이터 스크립트가 있는 동물이면
            {
                ThisData thisData = _prey.GetComponent<ThisData>();
                thisData.Hit(1); // 1만큼 데미지 
            }
        }

        // TODO: 레이가 닿은 부분, 총구에 파티클(혹은 다른 효과) 발생
    }

    /// <summary>
    /// 슬로우 타임 
    /// </summary>
    private void ActivateSlowTime()
    {
        if (vrifAction.Player.SlowMode.triggered) // B버튼을 누르면 
        {
            slowTimeEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
