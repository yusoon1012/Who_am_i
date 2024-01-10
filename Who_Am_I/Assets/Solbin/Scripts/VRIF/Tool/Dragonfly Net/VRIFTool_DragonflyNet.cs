using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 곤충 채집망의 최상단에 붙는다. 
/// </summary>
public class VRIFTool_DragonflyNet : MonoBehaviour
{
    // Dir Pannel에 닿았는지 체크
    [Tooltip("채집 콜라이더 전의 콜라이더를 말한다.")]
    public bool dirCheck = false;
    [Tooltip("채집 콜라이더를 말한다.")]
    // 채집 가능 레이어
    public int getherLayer = default;

    [Header("Net Material")]
    [Tooltip("곤충 채집망을 휘두름에 따라 그물망 크기 조정")]
    [SerializeField] private MeshRenderer meshRenderer = default;
    // 그물망 머티리얼
    private Material netMaterial = default;

    // 컨트롤러를 흔드는 값
    private float magnitude = default;
    // 컨트롤러를 흔드는 기준이 되는 값
    private float guideVel = default;

    // 현재 NetMeterial의 Move값
    private float currentNet = default;

    private void Start()
    {
        getherLayer = LayerMask.NameToLayer("Gether");
        netMaterial = meshRenderer.material;
        // 적용방법: netMaterial.GetFloat("_Move")
    }

    public void Gotha(Transform targetChild_)
    {
        if (dirCheck)
        {
            GameObject target = targetChild_.transform.parent.gameObject; // 채집할 게임 오브젝트

            Destroy(target); // 테스트용

            // TODO: 이펙트 출력하기
        }
    }

    private void FixedUpdate()
    {
        Stretch();
    }

    private void LateUpdate()
    {
        Shrink();
    }

    private void Stretch()
    {
        magnitude = VRIFInputSystem.Instance.rMagnitude;
        currentNet = netMaterial.GetFloat("_Move");

        if (magnitude >= 0.0001f) // 값이 -0.7을 향해 간다.
        {
            Debug.Log("@1");
            if (currentNet >= -0.7)
            {
                netMaterial.SetFloat("_Move", currentNet -= 0.1f);
            }
        }
        else if (magnitude < 0.0001f)// 값이 0을 향해 간다
        {
            Debug.Log("@2");
            if (currentNet <= 0)
            {
                netMaterial.SetFloat("_Move", currentNet += 0.1f);
            }
        }
    }

    private void Shrink()
    {
        // TODO: 여기에 줄어드는 로직을 집어넣으면 딜레이를 조금 더 확보할 수 있나?
    }
}

// TODO: @1와 @2가 동시에 들어오는 문제 => 네트 크기가 불안정한 이유 
