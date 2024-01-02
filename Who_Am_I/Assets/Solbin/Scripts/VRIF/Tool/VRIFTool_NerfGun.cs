using Meta.WitAi;
using OVR.OpenVR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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
    private float distance = 100f;
    // 포인터(크로스 헤어)
    [SerializeField] Transform pointer = default;
    // 너프건 궤적
    [SerializeField] LineRenderer trajectory = default;
    // 오브젝트 풀
    private Vector3 poolPos = new Vector3(0, -10, 0);
    // 궤적 포인트 배열 
    private Vector3[] trajectoryPos = new Vector3[2];

    public static event EventHandler slowTimeEvent;

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

    private void Start()
    {
        trajectory.positionCount = 2; // 궤적의 포인트는 두 개 
        VRIFInputSystem.Instance.slowMode += ActivateSlowTime; 
    }

    private void FixedUpdate() => Aiming();

    private void LateUpdate() { trajectory.SetPositions(trajectoryPos); }

    /// <summary>
    /// 너프건을 조준, 크로스 헤어를 표시한다. 
    /// </summary>
    private void Aiming()
    {
        trajectoryPos[0] = firePos.position;

        Ray ray = new Ray(firePos.position, firePos.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance)) // 총구가 물체를 향하고 있을때
        {
            if (!hit.collider.isTrigger) // 실체를 가지고 있을 때 
            {
                pointer.position = hit.point;
                trajectoryPos[1] = pointer.position;

                if (vrifAction.Player.RightTrigger.triggered || testAction.Test.Click.triggered)
                {
                    GameObject prey = hit.transform.gameObject;
                    Shoot(prey);
                }
            }
            else // 아닐때
            {
                pointer.position = poolPos;
                trajectoryPos[0] = poolPos;
                trajectoryPos[1] = poolPos;
            }
        }
    }

    /// <summary>
    /// 총을 발사한다. 
    /// </summary>
    private void Shoot(GameObject _prey)
    {
        if (_prey.layer == LayerMask.NameToLayer("Animal"))
        {
            if (_prey.GetComponentInChildren<SendDamage>()) // 데이터 스크립트가 있는 동물이면
            {
                Debug.Log(_prey.name);

                SendDamage sendDamage = _prey.GetComponentInChildren<SendDamage>();
                sendDamage.Hit(1); // 1만큼 데미지 
            }
        }

        // TODO: 레이가 닿은 부분, 총구에 파티클(혹은 다른 효과) 발생
    }

    /// <summary>
    /// 슬로우 타임 
    /// </summary>
    private void ActivateSlowTime(object sender, EventArgs e)
    {
        slowTimeEvent?.Invoke(this, EventArgs.Empty);
    }
}
