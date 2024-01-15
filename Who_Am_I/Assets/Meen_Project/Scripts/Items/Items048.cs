using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items048 : ItemsMain
{
    public Items048()
    {
        Init();
    }     // Items048()

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
