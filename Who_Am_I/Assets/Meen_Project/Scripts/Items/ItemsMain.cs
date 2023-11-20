using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsMain
{
    #region 변수 설정

    // 아이템 이름
    public string itemName = default;
    // 아이템 고유 번호
    public int itemNumber = default;
    // 아이템 아이콘 이미지 값
    public int itemImageNum = default;
    // 아이템 타입
    public ItemType itemType;
    // 아이템 정보
    public string itemInfo = default;
    // 아이템 중첩 수
    public int itemStack = default;

    #endregion 변수 설정

    public virtual void Init()
    {
        /* Empty */
    }     // Init()
}

// 아이템 타입 설정
public enum ItemType
{
    // 장비
    EQUIPMENT,
    // 음식
    FOOD,
    // 재료
    STUFF
}     // enum ItemType
