using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 곤충 채집망의 최상단에 붙는다. 
/// </summary>
public class VRIFTool_DragonflyNet : MonoBehaviour
{
    public static VRIFTool_DragonflyNet Instance;

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
    // 그물망의 늘어남 정도
    private float netStretch = default;

    [Tooltip("로컬 각속도를 구하기 위한 변수")]
    // 이전 프레임 Rotation
    private Quaternion previousRotation = default;
    // 각속도 관리 변수
    private Vector3 angularVelocity = default;
    // 각속도의 magnitude
    private float angularMagnitude = default;

    // Audio Source
    private AudioSource audioSource = default;

    // 채집 완료 이벤트
    public event EventHandler gatheringEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        getherLayer = LayerMask.NameToLayer("Gether");
        netMaterial = meshRenderer.material;

        audioSource = GetComponent<AudioSource>();
    }

    public void Gotha(Transform targetChild_)
    {
        if (dirCheck)
        {
            GameObject target = targetChild_.transform.parent.gameObject; // 채집할 게임 오브젝트

            Destroy(target); // 테스트용
            gatheringEvent?.Invoke(this, EventArgs.Empty);

            // TODO: 이펙트 출력하기
        }
    }

    private void Update()
    {
        GetLocalAngularVelocity();
        netStretch = netMaterial.GetFloat("_Move");
    }

    /// <summary>
    /// 1. 컨트롤러 velocity의 magnitude(0.0001 등 소수점 단위 출력)로는 정확한 구현이 어려워 각속도 사용 결정
    /// 2. 로컬 velocity.angularVelocity로 로컬 각속도를 구하는데 어려움이 있어 아래 메소드를 활용해 로컬 각속도 계산
    /// 3. 곤충 채집망을 휘두르는 정도를 나타냄
    /// </summary>
    private void GetLocalAngularVelocity()
    {
        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);

        previousRotation = transform.rotation;

        deltaRotation.ToAngleAxis(out var angle, out var axis);

        //각도에서 라디안으로 변환
        angle *= Mathf.Deg2Rad;

        angularVelocity = (1.0f / Time.deltaTime) * angle * axis;

        //각속도의 Magnitude
        angularMagnitude = angularVelocity.magnitude;
    }

    /// <summary>
    /// 위 계산이 끝난 후 머티리얼 업데이트 진행
    /// </summary>
    private void LateUpdate()
    {
        Stretch();
    }

    /// <summary>
    /// 위 로컬 각속도 계산값을 받아와 채집망을 휘두를 때 그물망이 늘어나는 것을 구현
    /// </summary>
    private void Stretch()
    {
        if (angularMagnitude >= 5) // 그물망이 -0.7을 향해 간다
        {
            if (netStretch > -0.7f) // 한계점 이하가 아닐 때
            {
                netMaterial.SetFloat("_Move", netStretch -= 0.15f);
            }

            if (!audioSource.isPlaying) { audioSource.Play(); } // 채집망 휘두르는 소리
        }
        else // 그물망이 0을 향해 간다 
        {
            if (netStretch < 0) // 한계점 이상이 아닐 때 
            {
                netMaterial.SetFloat("_Move", netStretch += 0.1f);
            }
        }
    }
}