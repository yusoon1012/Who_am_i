using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Unity.VisualScripting;
using System;

public class VRIFTool_FishingBobber : MonoBehaviour
{
    // 낚시찌의 Rigidbody
    private Rigidbody bobberRigid = default;
    // 낚시찌가 물 속에 있는지 판단
    private bool inWater = false;

    // 물고기 입질 이벤트
    public static event EventHandler fishNibble;

    // 오브젝트 풀
    private Vector3 poolPos = new Vector3(0, -10, 0);

    // 낚시대
    private Transform fishingRod = default;

    private void Start()
    {
        bobberRigid = transform.GetComponent<Rigidbody>();
        fishingRod = FindAnyObjectByType<VRIFTool_FishingRod>().transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Water")) // TODO: 추후 레이어 등으로 변경.
        {
            StopCoroutine("CheckDistance");

            inWater = true;

            if (!VRIFTool_FishingRod.Instance.isFishing)
            {
                StartCoroutine(StopBobber());
            }
        }
        else
        {
            StartCoroutine("CheckDistance");
        }
    }

    private IEnumerator CheckDistance()
    {
        while (Vector3.Distance(fishingRod.position, transform.position) < 30)
        {
            yield return null;
        }

        bobberRigid.useGravity = false; // 중력 해제 
        bobberRigid.velocity = Vector3.zero; // 낚시찌 정지
        transform.position = poolPos; // 낚시찌 원상 복구 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Water")) // 물 속에서 나간다면
        {
            inWater = false;
        }
    }

    #region 입질 체크 
    private IEnumerator StopBobber()
    {
        yield return new WaitForSeconds(0.1f);

        bobberRigid.velocity = Vector3.zero;
        bobberRigid.useGravity = false;

        StartCoroutine(CheckNibble());
    }

    /// <summary>
    /// 물고기 입질 체크
    /// </summary>
    private IEnumerator CheckNibble()
    {
        int percent = default;

        while(inWater) // 낚시찌가 물 속에 있는 것이 확인되었다면
        {
            yield return new WaitForSeconds(3); // 3초 대기 

            percent = UnityEngine.Random.Range(1, 2); // 50%의 확률로 입질 체크

            if (percent == 1) { fishNibble?.Invoke(this, EventArgs.Empty); } // 조건 만족시 입질 이벤트 발생
        }
    }
    #endregion
}
