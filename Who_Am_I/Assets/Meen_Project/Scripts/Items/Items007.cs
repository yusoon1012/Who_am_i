using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items007 : ItemsMain
{
    public Items007()
    {
        Init();
    }     // Items007()

    public override void Init()
    {
        itemName = "빨간 사과";
        itemNumber = 7;
        itemImageNum = 7;
        itemType = ItemType.FOOD;
        itemInfo = "맛있게 익은 적당한 크기의 사과";
        itemStack = 0;
    }     // Init()
}
