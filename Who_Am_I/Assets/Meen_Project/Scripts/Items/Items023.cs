using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items023 : ItemsMain
{
    public Items023()
    {
        Init();
    }     // Items023()

    public override void Init()
    {
        itemID = 0;
        itemName = " ";
        cookType = 0;
        satietyGauge = 0;
        pooGauge = 0;
        rarity = 0;
        durationTime = 0f;
        itemInfo = " ";
        getMap = " ";
        note = " ";
        itemImageNum = 0;
        utile = 0;
        itemEnglishName = " ";

        itemType = ItemType.FOOD;
        itemStack = 0;
    }     // Init()
}
