using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 앞서 정면 콜라이더를 통해 이 스크립트가 부착된 망 부분에 닿았다면 채집 성공 처리
/// </summary>
public class VRIFTool_DragonflyOrigin : MonoBehaviour
{
    private VRIFTool_DragonflyNet dragonflyNet = default;

    private void Start()
    {
        dragonflyNet = transform.parent.GetComponent<VRIFTool_DragonflyNet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == dragonflyNet.getherLayer))
        {
            dragonflyNet.Gotha(other.transform);
        }
    }
}
