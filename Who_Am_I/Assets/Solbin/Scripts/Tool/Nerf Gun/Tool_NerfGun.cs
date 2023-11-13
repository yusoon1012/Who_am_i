using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Tool_NerfGun : MonoBehaviour
{
    // 너프건을 쥐었는지 여부를 위해
    OVRGrabbable ovrGrabbable = default;
    // 레이 발사 지점
    [SerializeField] Transform firePos = default;
    // 사정거리
    private float distance = 8.5f;
    // 포인터(크로스 헤어)
    [SerializeField] Transform pointer = default;
    // 수렵 가능 여부(동물 조준)
    private bool aimingPrey = false;
    // 너프건 궤적
    [SerializeField] LineRenderer trajectory = default;

    private void Start()
    {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        trajectory.positionCount = 2; // 궤적의 포인트는 두 개 
    }

    private void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * distance, Color.green);

        if (ovrGrabbable.isGrabbed) // 만약 너프건이 쥐어진 상태일때
        {
            Aiming(); // 너프건을 조준한다

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) { Shoot(); }
        }
        else { Restore(); }
    }

    /// <summary>
    /// 너프건을 조준, 크로스 헤어를 표시한다. 
    /// </summary>
    private void Aiming()
    {
        trajectory.SetPosition(0, firePos.position);

        Ray ray = new Ray(firePos.position, firePos.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            pointer.position = hit.point;
            trajectory.SetPosition(1, hit.point);

            // 임시 주석
            //if (hit.transform.CompareTag("Animal")) // TODO: 후에 동물의 태그에 따라 수정
            //{
            //    aimingPrey = true; // 동물 조준 O
            //}
            //else
            //{
            //    aimingPrey = false; // 동물 조준 X
            //}
        }
    }

    /// <summary>
    /// 총을 발사한다. 
    /// </summary>
    private void Shoot()
    {
        Debug.Log("발사!");
        // TODO: 레이가 닿은 부분, 총구에 파티클(혹은 다른 효과) 발생
    }

    /// <summary>
    /// 너프건을 조준한 상태가 아닐 때 재세팅
    /// </summary>
    private void Restore()
    {
        trajectory.SetPosition(0, PlayerSystem.poolPos);
        trajectory.SetPosition(1, PlayerSystem.poolPos);
    }
}
