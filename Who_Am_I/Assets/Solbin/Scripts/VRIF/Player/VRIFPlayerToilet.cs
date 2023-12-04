using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFPlayerToilet : MonoBehaviour
{
    private VRIFAction vrifAction;

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Toilet")) // TODO: 이후 레이어마스크 이용 등 효율적인 방식으로 수정 필요 
        {
            if (vrifAction.Player.Interaction.triggered)
            {
                VisitToilet();
            }
        }
    }

    /// <summary>
    /// 화장실 방문 메소드 
    /// </summary>
    private void VisitToilet()
    {

    }
}


