using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsMain
{
    #region 변수 설정

    // ########## 기본 정보 ##########
    // 아이템 고유 번호
    public int itemID = default;
    // 아이템 이름
    public string itemName = default;
    // 아이템 영어 이름
    public string itemEnglishName = default;
    // 아이템 설명
    public string itemInfo = default;
    // 레어도
    // 1. 노말
    // 2. 레어
    // 3. 유니크
    public int rarity = default;
    // 아이템 중첩 수
    public int itemStack = default;
    // 아이템 타입
    public ItemType itemType;
    // 아이템 아이콘 이미지 고유 번호
    public int itemImageNum = default;

    // ########## 재료 아이템 정보 ##########
    // 채집 아이템 종류
    // 1. 습득 버튼 획득
    // 2. 특수 상호 작용 획득
    public int collectType = default;
    // 채집 대상 체력 (상호작용 횟수)
    public int hp = default;
    // 동작 범위 (PC 와 상호작용 거리)
    public float range = default;
    // 리스폰 시간
    public float respawn = default;

    // ########## 음식 아이템 정보 ##########
    // 요리 종류
    // 1. 기본 효과 요리
    // 2. 특수 효과 요리
    public int cookType = default;
    // 포만감 게이지 증가량
    public int satietyGauge = default;
    // 응가 게이지 증가량
    public int pooGauge = default;

    // ########## 도감 전용 아이템 정보 ##########
    // 아이템 획득 정보
    public string itemHint = default;

    #endregion 변수 설정

    public virtual void Init()
    {
        /* Empty */
    }     // Init()
}

// 아이템 타입 설정
public enum ItemType
{
    // 장비
    EQUIPMENT,
    // 음식
    FOOD,
    // 재료
    STUFF
}     // enum ItemType
