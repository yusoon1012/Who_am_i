using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTool_DragonflyNet : MonoBehaviour
{
    // Dir Pannel에 닿았는지 체크
    public bool dirCheck = false;
    // Restricted Net에 닿았는지 체크 (닿았다면 획득 불가)
    public bool restricted = false;

    public void Gotha(GameObject _target)
    {
        if (!restricted)
        {
            Debug.LogFormat("{0}를 잡았다!", _target);
            Destroy(_target); // 테스트용

            // TODO: 이펙트 출력하기
        }
    }
}
