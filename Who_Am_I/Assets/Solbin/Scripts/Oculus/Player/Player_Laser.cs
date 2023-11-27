using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Laser : MonoBehaviour
{
    #region 필드
    [Header("Reference")]
    // 플레이어
    private Transform player = default;
    //오른손 위치(컨트롤러 & 손모델)
    [SerializeField] private Transform rightAnchor = default;
    // VR 포인터 위치
    [SerializeField] protected Transform pointer = default;
    // Input System
    PlayerAction playerAction;

    [Header("Essential")]
    // 땅 레이어
    private int groundLayer = default;
    // UI 레이어
    private int UILayer = default;
    // 라인 렌더러 컴포넌트
    private LineRenderer lineRenderer = default;
    // 레이저 최대 거리(텔레포트 최대 거리)
    private float rayDistance = default;
    // 플레이어 시스템(컨트롤러 or 손 사용 여부)
    [SerializeField] private PlayerSystem playerSystem = default;
    #endregion

    #region 초기 세팅
    private void Start() { Setting(); }

    /// <summary>
    /// 레이어마스크, 컴포넌트 설정
    /// </summary>
    private void Setting()
    {
        player = transform.GetChild(0);
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        UILayer = 1 << LayerMask.NameToLayer("UI");
        lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // 점은 두 개
        rayDistance = Player_Status.playerStat.teleportDistance;
    }

    private void OnEnable() 
    {
        playerAction = new PlayerAction();
        playerAction.Enable(); 
    }

    private void OnDisable() { playerAction.Disable(); }
    #endregion

    /// <summary>
    /// 오른손에서 레이 발사
    /// </summary>
    private void ShootRay()
    {
        // 레이 앞으로 발사
        lineRenderer.SetPosition(0, rightAnchor.position);
        Debug.DrawRay(rightAnchor.position, rightAnchor.forward * rayDistance, Color.green);
        Ray ray = new Ray(rightAnchor.position, rightAnchor.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, groundLayer)) // 땅에 레이가 부딪혔을때
        {
            GroundRay(hit);
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, UILayer)) // UI에 레이가 부딪혔을때
        {
            UIRay(hit); // TODO: 인벤토리에서는 비활성화 해야 한다. 
        }
        else
            lineRenderer.enabled = false; // 이외의 상황에 라인 렌더러 비활성화
    }

    /// <summary>
    /// 땅레이어에 충돌
    /// </summary>
    private void GroundRay(RaycastHit _hit)
    {
        if (!playerSystem.handActivate)
        {
            RaycastHit hit = _hit;

            lineRenderer.enabled = true; // 라인 렌더러 활성화

            lineRenderer.SetPosition(1, hit.point);
            pointer.position = hit.point;

            if (playerAction.Player.Click.triggered)
            {
                //TODO: 텔레포트 할 위치임을 알리는 효과 추가
                Vector3 teleportPos = hit.point;
                teleportPos.y += 1;

                player.position = teleportPos;
            }
        }
        else { lineRenderer.enabled = false; }
    }

    /// <summary>
    /// UI레이어에 충돌
    /// </summary>
    private void UIRay(RaycastHit _hit)
    {
        RaycastHit hit = _hit;

        lineRenderer.enabled = true; // 라인 렌더러 활성화

        lineRenderer.SetPosition(1, hit.point);
        pointer.position = hit.point;   

        Debug.Log("UI 충돌 체크");
    }

    private void Update()
    {
        ShootRay();
    }
}
