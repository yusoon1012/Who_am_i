using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting002 : CraftingMain
{
    public Crafting002()
    {
        Init();
    }     // Crafting002()

    public override void Init()
    {
        craftingName = "송이 불고기";
        effectInfo = "포만감 최대치 + 10";
        craftingLength = 2;
        stuffName[0] = "송이 버섯";
        stuffName[1] = "고기";
        stuffStack[0] = 1;
        stuffStack[1] = 1;
        craftingStack = 1;
        utile = 1;
        disposableType = true;
        disposable = false;
    }     // Init()
}
