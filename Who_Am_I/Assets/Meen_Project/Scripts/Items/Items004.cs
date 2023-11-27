using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items004 : ItemsMain
{
    public Items004()
    {
        Init();
    }     // Items004()

    public override void Init()
    {
        itemName = "철";
        itemNumber = 4;
        itemImageNum = 4;
        itemType = ItemType.FOOD;
        itemInfo = "단단한 광물입니다.";
        itemStack = 0;
    }     // Init()
}
