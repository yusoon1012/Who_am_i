using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTool_DragonflyOrigin : MonoBehaviour
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
            if (dragonflyNet.dirCheck)
            {
                dragonflyNet.Gotha(other.gameObject);
            }
        }
    }
}
