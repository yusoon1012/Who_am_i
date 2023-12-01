using Oculus.Platform.Models;
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
        itemID = 2001;
        itemName = "우유";
        itemEnglishName = "Milk";
        itemInfo = "갓 짠 신선한 우유";
        rarity = 1;
        itemStack = 0;
        itemType = ItemType.FOOD;
        itemImageNum = 3;

        cookType = 1;
        satietyGauge = 30;
        pooGauge = 6;

        collectType = 0;
        hp = 0;
        range = 0f;
        respawn = 0f;

        itemHint = "획득 방법 : ";
    }     // Init()
}
