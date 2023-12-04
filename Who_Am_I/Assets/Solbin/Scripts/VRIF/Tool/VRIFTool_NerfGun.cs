using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRIFTool_NerfGun : MonoBehaviour
{
    // VRIF Action
    private VRIFAction vrifAction = default;
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
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
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

            if (vrifAction.Player.RightTrigger.triggered) { Shoot(hit.transform.gameObject); }
        }
    }

    /// <summary>
    /// 총을 발사한다. 
    /// </summary>
    private void Shoot(GameObject _prey)
    {
        if (_prey.layer == LayerMask.NameToLayer("Animal"))
        {
            Debug.LogWarning("Shoot Animal");
        }
        else
        {
            Debug.LogWarning("Not Shoot Animal");
        }

        // TODO: 레이가 닿은 부분, 총구에 파티클(혹은 다른 효과) 발생
    }

    private void ActivateSlowTime()
    {
        if (vrifAction.Player.SlowMode.triggered) // B버튼을 누르면 
        {
            slowTimeEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
