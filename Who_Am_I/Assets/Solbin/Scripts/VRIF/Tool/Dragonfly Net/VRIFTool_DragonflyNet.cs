using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 곤충 채집망의 최상단에 붙는다. 
/// </summary>
public class VRIFTool_DragonflyNet : MonoBehaviour
{
    // Dir Pannel에 닿았는지 체크
    [Tooltip("채집 부분(정면) 콜라이더를 말한다.")]
    public bool dirCheck = false;
    [Tooltip("뒷부분 콜라이더를 말한다.")]
    // Restricted Net에 닿았는지 체크 (닿았다면 획득 불가)
    public bool restricted = false;

    public void Gotha(GameObject _target)
    {
        if (!restricted)
        {
            Destroy(_target); // 테스트용

            // TODO: 이펙트 출력하기
        }
    }
}
