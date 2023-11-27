using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items006 : ItemsMain
{
    public Items006()
    {
        Init();
    }     // Items006()

    public override void Init()
    {
        itemName = "블루 아뮬렛";
        itemNumber = 6;
        itemImageNum = 6;
        itemType = ItemType.FOOD;
        itemInfo = "푸른색 마법빛을 머금은 목걸이 입니다";
        itemStack = 0;
    }     // Init()
}
