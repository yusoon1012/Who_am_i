using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing_FixGrabbable : MonoBehaviour
{
    private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    private OVRGrabbable grabbable;

    private void Awake()
    {
        defaultRotation = transform.rotation;
        defaultPosition = transform.position;

        grabbable = GetComponent<OVRGrabbable>();
    }

    private void LateUpdate()
    {
        transform.rotation = defaultRotation;
        transform.position = defaultPosition;

        // TODO: 왜 왼손이 물체를 잡았을 때와 오른손이 물체를 잡았을 때 고정 정도가 다른지 알아내서 수정해야 한다. 
        // => Physics 적용 vs. 스크립트 조정 방식 고려해보기
        if (grabbable.isGrabbed)
        {
            Debug.Log("잡았다.");
        }
    }
}
