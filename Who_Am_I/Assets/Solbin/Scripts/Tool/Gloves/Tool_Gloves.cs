using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Gloves : MonoBehaviour
{
    // 왼쪽 장갑 (착용시 활성화)
    [SerializeField] Transform leftGlove = default;
    // 오른쪽 장갑 (착용시 활성화)
    [SerializeField] Transform rightGlove = default;
    // 왼쪽 앵커 (착용시 부모)
    [SerializeField] Transform leftAnchor = default;
    // 오른쪽 앵커 (착용시 부모)
    [SerializeField] Transform rightAnchor = default;
    // 왼쪽 장갑 (미착용시 활성화)
    [SerializeField] Transform leftItem = default;
    // 오른쪽 장갑 (미착용시 활성화)
    [SerializeField] Transform rightItem = default;
    // 본 아이템 오브젝트 (미착용시 부모)
    [SerializeField] Transform gloves = default;
    // 플레이어
    [SerializeField] Transform player = default;
    // OVRGrabbable
    OVRGrabbable ovrGrabbable = default;

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// (초기 세팅)
    /// </summary>
    private void Setting()
    {
        leftItem.gameObject.SetActive(true);
        rightItem.gameObject.SetActive(true);

        leftGlove.gameObject.SetActive(false);
        rightGlove.gameObject.SetActive(false);

        ovrGrabbable = GetComponent<OVRGrabbable>();
    }

    /// <summary>
    /// 장갑 착용
    /// </summary>
    private void GetGloves()
    {
        // TODO: 장갑 각도, 위치를 손 모델과 일치시켜야 한다. 

        Debug.Log("장갑 장착함");

        leftGlove.SetParent(leftAnchor); // 왼쪽 장갑 왼쪽 손 앵커 하위로
        rightGlove.SetParent(rightAnchor); // 오른쪽 장갑 오른쪽 손 앵커 하위로

        leftGlove.gameObject.SetActive(true); // 장갑 활성화
        rightGlove.gameObject.SetActive(true);

        leftItem.gameObject.SetActive(false); // 아이템 비활성화
        rightItem.gameObject.SetActive(false);
    }

    /// <summary>
    /// 장갑 벗기
    /// </summary>
    private void TakeOffGloves()
    {
        leftGlove.SetParent(gloves); // 앵커 복귀
        rightGlove.SetParent(gloves);

        leftGlove.gameObject.SetActive(false); // 장갑 비활성화
        rightGlove.gameObject.SetActive(false);

        leftItem.gameObject.SetActive(true); // 아이템 활성화
        rightItem.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(ovrGrabbable.isGrabbed) // 장갑을 착용
        {
            GetGloves();
        }
    }

    // TODO: 장갑을 잡으면 => 형상 아이템 비활성화, OVRGrabbable 비활성화, 착용 아이템 활성화. 
    // TODO: 장갑을 벗는것은? => 기획팀에 문의 
    // TODO: 장갑을 착용한 상태에서 채집을 가능토록 하려면? 착용한 상태에서 OverlapSphere을 검사 => 만약 채집 가능한 것이 근처에
    // 있으며 맞는 키를 눌렀다면...?

    // 채집 가능 여부 
    private void FindForaging()
    {
        int radius = 10;
        Collider[] foraging = Physics.OverlapSphere(player.position, radius); // 반경 내 채집품 검색

        // TODO: 채집품 획득, 가까운 곳부터?
        Debug.Log("채집품 획득!");
    }
}
