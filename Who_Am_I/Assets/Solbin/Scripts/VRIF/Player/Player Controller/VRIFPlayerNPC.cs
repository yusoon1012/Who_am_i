using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class VRIFPlayerNPC : MonoBehaviour
{
    // 범위 내 npc가 있는지 확인
    private bool npcCheck = false;

    //WaitForEndOfFrame
    private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    // 시야 각도
    private float viewAngle = 130;
    // 시야 거리
    private float viewDistance = 10;

    // NPC 리스트
    private List<Transform> npcList = new List<Transform>();

    private void Start()
    {
        //VRIFInputSystem.Instance.interaction += AskNPC;
    }

    private void FixedUpdate()
    {
        FieldOfView();
    }

    private Vector3 Angle(float angle_) // TODO: 분석 필요
    {
        angle_ += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle_ * Mathf.Deg2Rad), 0f, Mathf.Cos(angle_ * Mathf.Deg2Rad));
    }

    /// <summary>
    /// NPC 검색 시야 그리기
    /// </summary>
    private void FieldOfView()
    {
        Vector3 leftBoundary = Angle(-viewAngle * 0.5f);  // 시야각 왼쪽 자른거
        Vector3 rightBoundary = Angle(viewAngle * 0.5f);  // 시야각의 오른쪽 자른거

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        //Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        //for (int i = 0; i < _target.Length; i++)
        //{
        //    Transform _targetTf = _target[i].transform;
        //    if (_targetTf.name == "Player")
        //    {
        //        Vector3 _direction = (_targetTf.position - transform.position).normalized;
        //        float _angle = Vector3.Angle(_direction, transform.forward);

        //        if (_angle < viewAngle * 0.5f)
        //        {
        //            RaycastHit _hit;
        //            if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
        //            {
        //                if (_hit.transform.name == "Player")
        //                {
        //                    Debug.Log("플레이어가 돼지 시야 내에 있습니다.");
        //                    Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);

        //                    thePig.Run(_hit.transform.position);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    private void AskNPC(object sender, EventArgs e)
    {

    }
}
