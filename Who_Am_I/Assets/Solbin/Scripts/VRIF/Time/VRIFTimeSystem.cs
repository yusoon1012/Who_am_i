using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTimeSystem : MonoBehaviour
{
    // SlowTime Scale 
    private float slowTimeScale = 0.35f;
    // VRIFTool_NerfGun
    //private VRIFTool_NerfGun vRIFTool_NerfGun = default;

    private void Start()
    {
        Setting();
    }

    private void Setting()
    {
        VRIFTool_NerfGun.slowTimeEvent += SlowTimer; // VRIFTool_NerfGun.cs의 이벤트
    }

    #region (도구 적용) 시간 제한이 있는 Slow Time
    private void SlowTimer(object sender, EventArgs e)
    {
        Time.timeScale = slowTimeScale; // 슬로우 타임 적용 
        float quotient = 1 / slowTimeScale;

        StartCoroutine(SlowTimerCoroutine(quotient)); // 슬로우 타임 타이머 시작 
    }

    private IEnumerator SlowTimerCoroutine(float _quotient)
    {
        float waitTime = 3f; // 현실 시간으로 슬로우 타임 지속시간 
        waitTime /= _quotient;

        yield return new WaitForSeconds(waitTime);

        Time.timeScale = 1; // 정상 시간으로 복귀 
    }
    #endregion

    #region (퀵슬롯 등 UI 적용) 시간 제한이 없는 Slow Time
    public void OnSlowTime() { Time.timeScale = slowTimeScale; }

    public void OffSlowTime() { Time.timeScale = 1; } // TODO: 후에 퀵슬롯 추가 시 슬로우 타임 적용 
    #endregion
}
