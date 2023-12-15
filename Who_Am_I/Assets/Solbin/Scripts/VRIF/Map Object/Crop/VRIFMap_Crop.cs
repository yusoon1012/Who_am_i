using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VRIFMap_Crop : MonoBehaviour
{
    [Header("잎")]
    [SerializeField] private GameObject leaf = default;

    // 작물의 HP (몇 번 잡아당겨야 뽑히는가?)
    public int hp = 2;
    // 작물의 Grabbable 스크립트
    private Grabbable grabbable = default;
    // TODO: 인스펙터에서 VRIFMap_CropStretch를 지우면 정상작동한다. 

    // 잡고 있는 손 
    public GameObject hand { get; private set; }

    private void Start()
    {
        grabbable = transform.GetComponent<Grabbable>();

        if (grabbable != null)
        {
            Debug.Log("Grabbable이 있다.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 잎에 닿아있고 장갑을 착용 중 
        if (other.GetComponent<Grabber>() && VRIFItemSystem.Instance.handType == VRIFItemSystem.HandType.GLOVES)
        {
            if (other.CompareTag("Left") && VRIFInputSystem.Instance.lGrab >= 0.7f) // 왼손 충돌 중 그랩이면
            {
                WhatHand(other.gameObject); // 왼손 전달
            }
            else if (other.CompareTag("Right") && VRIFInputSystem.Instance.rGrab >= 0.7f) // 오른손 충돌 중 그랩이면 
            {
                WhatHand(other.gameObject); // 오른손 전달 
            }
        }
    }

    /// <summary>
    /// 잎을 잡고 있는 손이 무엇인지 판단한다.  
    /// </summary>
    private void WhatHand(GameObject _hand)
    {
        if (hand == null) // 양손 모두 잡았을때, 처음 잡은 손만 입력을 받는다. 
        {
            hand = _hand;
        }
    }

    private void FixedUpdate()
    {
        if (hand != null)
        {
            CheckRelease(); // 손을 놓는 것을 체크 
            StretchLeaf();
            Harvesting();
        }

        if (grabbable == null)
        {
            Debug.LogWarning("Grabbable is null!"); // TODO: 왜 계속 grabbable이 null인가?
        }
    }

    #region 손을 놓는 조건 
    /// <summary>
    /// 손을 놓는 것을 체크 (컨트롤러)
    /// </summary>
    private void CheckRelease()
    {
        if (hand.CompareTag("Left") && VRIFInputSystem.Instance.lGrab <= 0.5f)
        {
            //Debug.LogWarning("Release Left Hand");
            hand = null;
        }
        else if (hand.CompareTag("Right") && VRIFInputSystem.Instance.rGrab <= 0.5f)
        {
            //Debug.LogWarning("Release Right Hand");
            hand = null;
        }
    }

    /// <summary>
    /// 손을 놓는 것을 체크 (거리)
    /// </summary>
    protected void CheckDistance() { Debug.LogWarning("Too Far"); hand = null; }

    #endregion

    private void StretchLeaf()
    {

    }

    private void Harvesting()
    {
        if (hp <= 0)
        {
            grabbable.enabled = true;
        }
    }
}

// TODO: 뽑히긴 뽑히는데 작물이 부드럽게 움직이지 않는다. 
