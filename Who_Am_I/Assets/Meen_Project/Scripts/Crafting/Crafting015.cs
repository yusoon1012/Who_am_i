using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting015 : CraftingMain
{
    public Crafting015()
    {
        Init();
    }     // Crafting015()

    public override void Init()
    {
        craftingID = 0;
        craftingName = " ";
        coolTime = 0f;
        stuffName[0] = " ";
        stuffName[1] = " ";
        disposableType = false;
        utile = 0;

        effectInfo = " ";
        craftingLength = 0;
        stuffStack[0] = 0;
        stuffStack[1] = 0;
        craftingStack = 0;
        disposable = false;
    }     // Init()
}
