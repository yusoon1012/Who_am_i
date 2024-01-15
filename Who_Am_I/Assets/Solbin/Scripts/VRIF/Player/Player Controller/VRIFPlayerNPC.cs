using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class VRIFPlayerNPC : MonoBehaviour
{
    [Header("Center Eye Anchor")]
    [SerializeField] Transform centerEyeAnchor = default;

    // 시야 각도
    private float viewAngle = 100;
    // 시야 거리
    private float viewDistance = 3;

    private int npcLayer = default;

    // VRIF Action
    private VRIFAction vrifAction = default;

    private void Start()
    {
        npcLayer = LayerMask.NameToLayer("NPC");
    }

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction.Disable();
    }

    private void FixedUpdate()
    {
        FieldOfView();
    }

    private Vector3 Angle(float angle_)
    {
        angle_ += centerEyeAnchor.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle_ * Mathf.Deg2Rad), 0f, Mathf.Cos(angle_ * Mathf.Deg2Rad));
    }

    /// <summary>
    /// NPC 검색 시야 그리기
    /// </summary>
    private void FieldOfView()
    {
        Vector3 leftBoundary = Angle(-viewAngle * 0.5f);  // 시야각 왼쪽 자른거
        Vector3 rightBoundary = Angle(viewAngle * 0.5f);  // 시야각 오른쪽 자른거

        //Debug.DrawRay(centerEyeAnchor.position, leftBoundary, Color.red); // 확인용 코드
        //Debug.DrawRay(centerEyeAnchor.position, rightBoundary, Color.red); // 확인용 코드

        Collider[] npcs = Physics.OverlapSphere(centerEyeAnchor.position, viewDistance);

        for (int i = 0; i < npcs.Length; i++)
        {
            Transform npcTf = npcs[i].transform; // 감지된 개별 npc

            Vector3 direction = (npcTf.position - centerEyeAnchor.position).normalized; // npc - pc 방향
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < viewAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(centerEyeAnchor.position, direction, out hit, viewDistance))
                {
                    if (hit.transform.gameObject.layer == npcLayer)
                    {
                        //Debug.DrawRay(centerEyeAnchor.position, direction, Color.blue); // 확인용 코드
                        if (vrifAction.Player.Interaction.triggered) { AskNPC(hit.transform); }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 시야 내 NPC에게 대화를 거는 메소드
    /// </summary>
    /// <param name="npc_">대화할 NPC</param>
    private void AskNPC(Transform npc_)
    {
        Debug.Log("시야 내 NPC에 대화 시도");
    }
}
