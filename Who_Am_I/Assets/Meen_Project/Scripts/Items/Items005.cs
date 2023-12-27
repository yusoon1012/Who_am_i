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
        itemID = 1001;
        itemName = "송이 버섯";
        itemEnglishName = "Matsutake";
        itemInfo = "송이 버섯";
        rarity = 3;
        itemStack = 0;
        itemType = ItemType.STUFF;
        itemImageNum = 6;

        cookType = 1;
        satietyGauge = 0;
        pooGauge = 0;

        collectType = 0;
        hp = 0;
        range = 0f;
        respawn = 0f;

        itemHint = "획득 방법 : ";
    }     // Init()
}
