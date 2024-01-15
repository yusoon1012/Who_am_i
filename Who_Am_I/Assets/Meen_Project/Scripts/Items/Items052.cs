using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items052 : ItemsMain
{
    public Items052()
    {
        Init();
    }     // Items052()

    public override void Init()
    {
        itemID = 0;
        itemName = " ";
        itemInfo = " ";
        getMap = " ";
        infinity = false;
        itemHint = " ";
        itemEnglishName = " ";
        itemImageNum = 0;

        itemType = ItemType.EQUIPMENT;
        itemStack = 0;
    }     // Init()
}
