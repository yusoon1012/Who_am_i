using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items008 : ItemsMain
{
    public Items008()
    {
        Init();
    }     // Items008()

    public override void Init()
    {
        itemName = "고기";
        itemNumber = 8;
        itemImageNum = 8;
        itemType = ItemType.FOOD;
        itemInfo = "먹음직 스러운 고기, 익혀 먹어야 할것만 같다";
        itemStack = 0;
    }     // Init()
}
