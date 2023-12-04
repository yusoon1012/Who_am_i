using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCraftingInfo : MonoBehaviour
{
    // 저장되는 제작 아이템 이름
    public string craftingName { get; set; } = default;
    // 현재 활성화된 제작 리스트인지 확인
    public bool activeSlotCheck { get; set; } = false;

    // 칸마다 제작 아이템 이름을 저장하는 함수
    public void SaveInfo(string name)
    {
        craftingName = name;

        activeSlotCheck = true;
    }     // SaveInfo()

    // 칸마다 제작 아이템 이름을 불러오는 함수
    public string LoadInfo(out string name)
    {
        name = craftingName;

        return name;
    }     // LoadInfo()
}
