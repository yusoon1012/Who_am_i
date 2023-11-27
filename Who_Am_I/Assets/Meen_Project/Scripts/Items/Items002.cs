using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items002 : ItemsMain
{
    public Items002()
    {
        Init();
    }     // Items002()

    public override void Init()
    {
        itemName = "블루 포션";
        itemNumber = 2;
        itemImageNum = 2;
        itemType = ItemType.FOOD;
        itemInfo = "마시면 소량의 MP 가 회복되는 작은 포션입니다.";
        itemStack = 0;
    }     // Init()
}
