using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items001 : ItemsMain
{
    public Items001()
    {
        Init();
    }     // Items001()

    public override void Init()
    {
        itemID = 1000;
        itemName = "고기";
        itemEnglishName = "Meat";
        itemInfo = "불에 구워 먹으면 맛있어 보이는 고기";
        rarity = 1;
        itemStack = 0;
        itemType = ItemType.STUFF;
        itemImageNum = 1;
        collectType = 1;
        hp = 1;
        range = 30f;
        respawn = 60f;

        cookType = 0;
        satietyGauge = 0;
        pooGauge = 0;

        itemHint = "획득 방법 : ";
    }     // Init()
}
