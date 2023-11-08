using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    #region 필드
    public static Player_Ray instance;

    //오른쪽 컨트롤러
    [SerializeField] private Transform rightController = default;
    // VR 포인터 위치
    [SerializeField] protected Transform pointer = default;
    // 땅 레이어
    private int groundLayer = default;
    // Ray가 땅을 가르키는지 확인
    protected bool itGround = false;
    #endregion

    private void Awake() { instance = this; }

    private void Start() { groundLayer = 1 << LayerMask.NameToLayer("Ground"); } // CHECK: 추후 체크

    private void DrawRay()
    {
        if (rightController.gameObject.activeSelf) // 우측 컨트롤러 활성화 상태
        {
            Vector3 rightControllerPos = rightController.position;

            Ray ray = new Ray(rightControllerPos, rightController.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                pointer.position = hit.point; // 포인터 나타내기

                if (hit.transform.gameObject.layer == groundLayer) // 땅 레이어와 충돌
                {
                    itGround = true;
                }
                else itGround = false;

                // TODO: UI와 충돌 확인
            }
        }
    }

    /// <summary>
    /// 포인터는 플레이어를 바라본다
    /// </summary>
    private void PointerLook()
    {
        pointer.LookAt(transform.position);
    }

    private void Update()
    {
        DrawRay();
        PointerLook();
    }
}
