using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTool_DragonflyRestricted : MonoBehaviour
{
    private VRIFTool_DragonflyNet dragonflyNet = default;

    private void Start()
    {
        dragonflyNet = transform.parent.GetComponent<VRIFTool_DragonflyNet>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Butterfly")) // TODO: 후에 곤충 분류로 변경 (레이어)
        {
            dragonflyNet.restricted = true;

            Invoke("Clear", 2f) ;
        }
    }

    private void Clear() { dragonflyNet.restricted = false; }
}
