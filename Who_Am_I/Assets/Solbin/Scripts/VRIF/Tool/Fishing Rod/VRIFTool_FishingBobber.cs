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

    private void Start()
    {
        bobberRigid = transform.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Water")) // TODO: 추후 레이어 등으로 변경.
        {
            inWater = true;

            if (!VRIFTool_FishingRod.Instance.isFishing)
            {
                StartCoroutine(StopBobber());
            }
        }
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
