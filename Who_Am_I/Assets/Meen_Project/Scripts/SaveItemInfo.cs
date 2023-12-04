using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveItemInfo : MonoBehaviour
{
    #region 변수 설정

    // 아이템 이름을 인벤토리 칸마다 저장
    public string itemName { get; set; } = default;
    // 아이템 중첩 수를 인벤토리 칸마다 저장
    public int itemStack { get; set; } = default;
    // 인벤토리 슬롯에 아이템이 존재하는지 체크
    public bool activeSlotCheck { get; set; } = false;

    #endregion 변수 설정

    // 인벤토리 칸마다 아이템 이름과 아이템 중첩 수를 저장
    public void SaveInfo(string name, int stack)
    {
        itemName = name;
        itemStack = stack;
        activeSlotCheck = true;
    }     // SaveInfo()

    // 해당 인벤토리 칸에서 아이템 이름을 출력
    public string LoadNameInfo(out string name)
    {
        name = itemName;

        return name;
    }     // LoadNameInfo()

    // 해당 인벤토리 칸에서 아이템 중첩 수를 출력
    public int LoadStackInfo(out int stack)
    {
        stack = itemStack;

        return stack;
    }     // LoadStackInfo()

    // 해당 인벤토리 칸을 초기화 시켜줌
    public void CleanSlot()
    {
        itemName = string.Empty;
        itemStack = 0;
        activeSlotCheck = false;
    }     // CleanSlot()
}
