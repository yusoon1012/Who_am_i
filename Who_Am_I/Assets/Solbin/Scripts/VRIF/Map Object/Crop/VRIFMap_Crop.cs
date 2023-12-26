using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VRIFMap_Crop : MonoBehaviour
{
    // 작물의 HP (몇 번 잡아당겨야 뽑히는가?)
    public int hp = 2;
    // 작물의 Grabbable 스크립트
    private Grabbable grabbable = default;
    // 잡고 있는 손 
    public GameObject hand { get; private set; }

    [Header("잎의 Bone 위치")]
    [Tooltip("잎이 늘어나게 되는 Armature 위치")]
    [SerializeField] private Transform leafBone = default;
    // 위 Bone의 원래 위치 
    private Vector3 leafOriginPos = default;

    // 본체 콜라이더
    [SerializeField] private Collider radishCollider = default;
    // 무 본체의 Rigidbody
    private Rigidbody radishRigid = default;

    // 당김 정도를 나타내는 bool값 (1회/2회...)
    private bool firstPull = false;
    private bool secondPull = false;

    [Header("자식: Item Collider")]
    [Tooltip("아이템 인식범위")]
    [SerializeField] private Collider itemCollider = default;

    private void Start()
    {
        grabbable = transform.GetComponent<Grabbable>();
        grabbable.enabled = false; // 작물을 뽑기 전엔 그랩 불가

        leafOriginPos = leafBone.localPosition;
        radishRigid = GetComponent<Rigidbody>();

        itemCollider.enabled = false; // 뿌리 작물을 뽑은 이후부터 활성화
    }

    private void OnTriggerStay(Collider other) // 캡슐 콜라이더가 잎 콜라이더가 된다. 
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
        if (hand == null) // 양손 모두 뿌리 작물을 잡았을때, 처음 잡은 손만 입력을 받는다. 
        {
            hand = _hand;
        }
    }

    private void FixedUpdate()
    {
        if (hand != null)
        {
            CheckRelease(); // 손을 놓는 것을 체크 
            StretchLeaf(); // 잎을 늘린다. 
            Harvesting();
        }
        else
        {
            ResetLeaf();
        }
    }

    #region 손을 놓는 조건 
    /// <summary>
    /// 손을 놓는 것을 체크 (컨트롤러)
    /// </summary>
    private void CheckRelease()
    {
        if ((hand.CompareTag("Left") && VRIFInputSystem.Instance.lGrab <= 0.5f) ||
            (hand.CompareTag("Right") && VRIFInputSystem.Instance.rGrab <= 0.5f))
        {
            hand = null;
        }
    }

    /// <summary>
    /// 손을 놓는 것을 체크 (거리)
    /// </summary>
    protected void CheckDistance() { hand = null; }

    #endregion

    /// <summary>
    /// 잎을 잡아당기면 늘어난다. 
    /// </summary>
    private void StretchLeaf()
    {
        if (hand != null && !grabbable.enabled)
        {
            leafBone.position = hand.transform.position;
        }
    }

    /// <summary>
    /// 잎을 잡아당기지 않을 때 원상 복귀
    /// </summary>
    public void ResetLeaf() { leafBone.localPosition = leafOriginPos; }

    private void Harvesting()
    {
        if (hp == 1 && !firstPull)
        {
            firstPull = true;
            radishRigid.AddForce(Vector3.up * 1.2f, ForceMode.Impulse);

            Invoke("ResetVelocity", 0.1f); // 작물이 어느정도 빠져나오다 멈추도록 
        }
        else if (hp <= 0 && !secondPull)
        {
            secondPull = true;
            radishRigid.AddForce(Vector3.up * 1.2f, ForceMode.Impulse);

            grabbable.enabled = true;
            radishCollider.enabled = true;
            Invoke("ActivateGravity", 0.1f); // 땅에서 탈출할 시간 이후 중력 활성화

            itemCollider.enabled = true; // 아이템 획득 가능 

            // TODO: 풀로 돌아간 후 재세팅이 필요하다. 
        }
    }

    // 중력 활성화
    private void ActivateGravity() { radishRigid.useGravity = true; }

    // velocity zero
    private void ResetVelocity() { radishRigid.velocity = Vector3.zero; }
}

