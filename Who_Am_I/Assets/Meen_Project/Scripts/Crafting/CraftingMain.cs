using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMain
{
    #region 변수 설정

    // 제작 레시피 고유 번호
    public int craftingID = default;
    // 제작 할 아이템 이름
    public string craftingName = default;
    // 다시 제작하기 위한 시간 (모든 레시피 동시 쿨타임 적용)
    public float coolTime = default;
    // 재료 아이템의 이름
    public string[] stuffName = new string[2];
    // 한번만 제작 가능한 아이템인지 타입 체크
    public bool disposableType = false;
    // 제작 할 아이템의 효과 타입
    public int utile = default;

    // 한번만 제작 가능한 아이템 제작 여부 체크
    public bool disposable = false;
    // 제작 할 아이템의 특수 효과 정보
    public string effectInfo = default;

    // 필요한 재료 아이템의 갯수
    public int craftingLength = default;
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
