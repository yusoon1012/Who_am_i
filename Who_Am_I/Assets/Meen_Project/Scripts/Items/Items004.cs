using Oculus.Platform.Models;
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
        itemID = 2002;
        itemName = "딸기 우유";
        itemEnglishName = "Strawberry Milk";
        itemInfo = "딸기 맛이 나는 달콤한 우유 [사용 효과 : 포만감 + 55, 응가 게이지 + 5]";
        rarity = 3;
        itemStack = 0;
        itemType = ItemType.FOOD;
        itemImageNum = 4;

        cookType = 1;
        satietyGauge = 55;
        pooGauge = 5;

        collectType = 0;
        hp = 0;
        range = 0f;
        respawn = 0f;

        itemHint = "획득 방법 : ";
    }     // Init()
}
