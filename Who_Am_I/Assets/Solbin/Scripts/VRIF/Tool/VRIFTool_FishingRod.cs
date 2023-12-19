using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTool_FishingRod : MonoBehaviour
{
    [Header("[PlayerRange] VRIFPlayerFishing")]
    [SerializeField] private VRIFPlayerFishing vrifPlayerFishing = default;
    [Header("[PlayerRange] Fishing Hit Range")]
    [SerializeField] Collider fishingHitRange = default;

    private void OnTriggerExit(Collider other)
    {
        if (vrifPlayerFishing.activateFishing && other == fishingHitRange) // 물가에 접촉해있고 낚시대를 위로 들어올렸을 때 
        {
            StartCoroutine(CheckSwing());
        }
    }

    /// <summary>
    /// 1초간 아래로 휘두르는 동작을 확인
    /// </summary>
    private IEnumerator CheckSwing()
    {
        float time = 0f;

        while (time < 1) // 뒤로 젖힌 후로부터 1초 간 
        {
            time += Time.deltaTime;

            if (VRIFInputSystem.Instance.rVelocity.y <= -0.7f) // 아래로 휘두르는 것이 확인되면
            {
                Debug.LogWarning("StartFishing!!");
            }

            yield return null;
        }
    }

}
