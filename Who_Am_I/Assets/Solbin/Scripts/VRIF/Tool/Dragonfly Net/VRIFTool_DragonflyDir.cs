using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTool_DragonflyDir : MonoBehaviour
{
    private VRIFTool_DragonflyNet dragonflyNet = default;

    private void Start()
    {
        dragonflyNet = transform.parent.GetComponent<VRIFTool_DragonflyNet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Butterfly")) // TODO: 후에 곤충 분류로 변경 (레이어)
        {
            dragonflyNet.dirCheck = true;

            StartCoroutine(Clear());
        }
    }

    private IEnumerator Clear()
    {
        yield return new WaitForSeconds(0.1f);
        dragonflyNet.dirCheck = false;
    }
}
