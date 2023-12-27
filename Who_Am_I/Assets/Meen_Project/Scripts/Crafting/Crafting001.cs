using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting001 : CraftingMain
{
    public Crafting001()
    {
        Init();
    }     // Crafting001()

    public override void Init()
    {
        craftingName = "딸기 우유";
        craftingLength = 2;
        stuffName[0] = "딸기";
        stuffName[1] = "우유";
        stuffStack[0] = 1;
        stuffStack[1] = 1;
        craftingStack = 1;
        disposableType = false;
        disposable = false;
    }     // Init()
}
