using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items006 : ItemsMain
{
    public Items006()
    {
        Init();
    }     // Items006()

    public override void Init()
    {
        itemID = 2003;
        itemName = "송이 불고기";
        itemEnglishName = "Matsutake Bulgogi";
        itemInfo = "송이 버섯으로 요리한 맛있어보이는 송이 불고기";
        rarity = 3;
        itemStack = 0;
        itemType = ItemType.FOOD;
        itemImageNum = 7;

        cookType = 2;
        satietyGauge = 0;
        pooGauge = 0;

        collectType = 0;
        hp = 0;
        range = 0f;
        respawn = 0f;

        itemHint = "획득 방법 : ";
    }     // Init()
}
