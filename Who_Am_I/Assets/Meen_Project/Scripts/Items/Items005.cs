using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items005 : ItemsMain
{
    public Items005()
    {
        Init();
    }     // Items005()

    public override void Init()
    {
        itemName = "사파이어";
        itemNumber = 5;
        itemImageNum = 5;
        itemType = ItemType.FOOD;
        itemInfo = "은은한 푸른빛을 띄는 보석입니다";
        itemStack = 0;
    }     // Init()
}
