using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Gloves : MonoBehaviour
{
    // 왼쪽 장갑
    [SerializeField] Transform leftGlove = default;
    // 오른쪽 장갑
    [SerializeField] Transform rightGlove = default;
    // 왼쪽 앵커
    [SerializeField] Transform leftAnchor = default;
    // 오른쪽 앵커
    [SerializeField] Transform rightAnchor = default;

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// (초기 세팅)
    /// </summary>
    private void Setting()
    {
        leftGlove.gameObject.SetActive(false);
        rightGlove.gameObject.SetActive(false);

        leftGlove.SetParent(leftAnchor); // 왼쪽 장갑 왼쪽 손 앵커 하위로
        rightGlove.SetParent(rightAnchor); // 오른쪽 장갑 오른쪽 손 앵커 하위로
    }

    // TODO: 장갑을 잡으면 => 형상 아이템 비활성화, OVRGrabbable 비활성화, 착용 아이템 활성화. 
    // TODO: 장갑을 벗는것은? => 기획팀에 문의 
}
