using Oculus.Platform.Models;
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
        itemID = 0;
        itemName = " ";
        getType = 0;
        hp = 0;
        rangeRec = 0f;
        respawn = 0f;
        rarity = 0;
        itemInfo = " ";
        getMap = " ";
        note = " ";
        itemImageNum = 0;
        itemEnglishName = " ";

        itemType = ItemType.STUFF;
        itemStack = 0;
    }     // Init()
}
