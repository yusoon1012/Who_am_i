using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items003 : ItemsMain
{
    public Items003()
    {
        Init();
    }     // Items003()

    public override void Init()
    {
        itemName = "롱 소드";
        itemNumber = 3;
        itemImageNum = 3;
        itemType = ItemType.FOOD;
        itemInfo = "날카롭고 긴 철검입니다.";
        itemStack = 0;
    }     // Init()
}
