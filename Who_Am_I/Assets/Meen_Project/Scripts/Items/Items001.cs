using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items001 : ItemsMain
{
    public Items001()
    {
        Init();
    }     // Items001()

    public override void Init()
    {
        itemName = "레드 포션";
        itemNumber = 1;
        itemImageNum = 1;
        itemType = ItemType.FOOD;
        itemInfo = "마시면 소량의 HP 가 회복되는 작은 포션입니다.";
        itemStack = 0;
    }     // Init()
}
