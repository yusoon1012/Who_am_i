using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI만을 비추는 UICamera가 CenterEyeAnchor(메인 카메라)의 Position, Rotation을 쫓도록 한다.
/// </summary>
public class VRIFSubCamera : MonoBehaviour
{
    [Header("자식: CenterEyeAnchor")]
    [SerializeField] private Transform centerEyeAnchor = default;

    /// <Point> LateUpdate()가 아니라면 Jitter 발생 
    private void LateUpdate()
    {
        transform.position = centerEyeAnchor.position;
        transform.rotation = centerEyeAnchor.rotation;
    }
}
