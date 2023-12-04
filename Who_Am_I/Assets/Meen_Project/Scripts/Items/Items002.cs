using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items002 : ItemsMain
{
    public Items002()
    {
        Init();
    }     // Items002()

    public override void Init()
    {
        itemID = 2000;
        itemName = "딸기";
        itemEnglishName = "Strawberry";
        itemInfo = "빨갛게 잘 익은 딸기";
        rarity = 2;
        itemStack = 0;
        itemType = ItemType.FOOD;
        itemImageNum = 2;

        cookType = 1;
        satietyGauge = 20;
        pooGauge = 3;

        collectType = 0;
        hp = 0;
        range = 0f;
        respawn = 0f;

        itemHint = "획득 방법 : ";
    }     // Init()
}
