using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMain
{
    #region 변수 설정

    // 제작 할 아이템 이름
    public string craftingName = default;
    // 필요한 재료 아이템의 갯수
    public int craftingLength = default;
    // 재료 아이템의 이름
    public string[] stuffName = new string[5];
    // 재료 아이템의 필요한 중첩 수
    public int[] stuffStack = new int[5];
    // 제작되는 아이템의 갯수
    public int craftingStack = default;

    #endregion 변수 설정

    public virtual void Init()
    {
        /* Empty */
    }     // Init()
}
