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
        craftingName = "블루 아뮬렛";
        craftingLength = 2;
        stuffName[0] = "철";
        stuffName[1] = "사파이어";
        stuffStack[0] = 10;
        stuffStack[1] = 10;
        craftingStack = 1;
    }     // Init()
}
