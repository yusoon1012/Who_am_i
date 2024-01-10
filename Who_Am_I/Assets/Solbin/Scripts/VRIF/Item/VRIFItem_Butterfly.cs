using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VRIFItem_Butterfly : MonoBehaviour
{
    // TODO: 잡은 것이 이것(this gameObject)일때 => 동작 행하기 
    [Header("나비")]
    private Animator animator = default;
    private Rigidbody rigid = default;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        foreach (var grabber in VRIFGameManager.Instance.grabberArray)
        {
            grabber.grabEvent += Catch; // 물체를 잡았을 때의 이벤트 구독
        }
    }

    private void Catch(object sender, EventArgs e)
    {
        foreach (var grabber in VRIFGameManager.Instance.grabberArray)
        {
            if (grabber.HoldingItem) // 물체를 잡은 Grabber가 있다면 
            {
                if (grabber.HeldGrabbable.gameObject == gameObject)
                {
                    Grabber holder = grabber; // 현재 나비를 잡고 있는 손 
                    StartCoroutine(Holding(holder));
                }
            }
        }
    }

    /// <summary>
    /// 나비를 잡고 있는 동안 대기하기 위한 코루틴
    /// </summary>
    /// <param name="holder_">나비를 잡고 있는 Grabber</param>
    /// <returns></returns>
    private IEnumerator Holding(Grabber holder_)
    {
        GetButterfly(); // 나비를 잡았을 때 동작

        while (holder_.HoldingItem) // 나비를 잡고 있는 동안 대기 
        {
            yield return null;
        }

        ReleaseButterfly(); // 나비를 놓았을 때 동작
    }

    /// <summary>
    /// 나비를 잡고 있는 동안 지속될 동작
    /// </summary>
    private void GetButterfly()
    {
        animator.enabled = false;
    }

    /// <summary>
    /// 나비를 놓았을 때 기존 동작으로 원상 복귀
    /// </summary>
    private void ReleaseButterfly()
    {
        transform.rotation = Quaternion.identity;

        rigid.velocity = Vector3.zero; // 선형속도 0으로
        rigid.angularVelocity = Vector3.zero; // 각속도를 0로. 

        animator.enabled = true;
    }
}
