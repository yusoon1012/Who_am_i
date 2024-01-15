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
        if (other.gameObject.layer == dragonflyNet.getherLayer)
        {
            dragonflyNet.dirCheck = true;

            StartCoroutine(Clear());
        }
    }

    private IEnumerator Clear()
    {
        yield return new WaitForSeconds(0.7f);
        dragonflyNet.dirCheck = false;
    }
}
