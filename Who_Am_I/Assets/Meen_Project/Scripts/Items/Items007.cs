using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items007 : ItemsMain
{
    public Items007()
    {
        Init();
    }     // Items007()

    public override void Init()
    {
        itemID = 3001;
        itemName = "너프건";
        itemEnglishName = "NerfGun";
        itemInfo = "진짜 총을 기대하셨다면 실망하셨겠군요!";
        rarity = 3;
        itemStack = 0;
        itemType = ItemType.EQUIPMENT;
        itemImageNum = 8;

        cookType = 0;
        satietyGauge = 0;
        pooGauge = 0;

        collectType = 0;
        hp = 0;
        range = 0f;
        respawn = 0f;

        itemHint = "획득 방법 : ";
    }     // Init()
}
