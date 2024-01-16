using UnityEngine;

public class ItemsMain
{
    #region 재료 아이템 정보 설정

    // ########## 재료 아이템 기본 정보 ##########
    // 아이템 고유 번호
    public int itemID = default;
    // 아이템 이름
    public string itemName = default;
    // 채집 아이템 종류
    // 0 : 습득 버튼 획득
    // 1 : 잡아당기기
    // 2 : 낚시
    // 3 : 수렵
    // 4 : 해당 없음(제작 결과물. 인벤토리 내 상호작용)
    public int getType = default;
    // 채집 대상 체력 (상호작용 횟수) 획득에 필요 특수 상호작용 횟수(당기기)
    public int hp = default;
    // PC 와 상호 작용 가능한 거리
    public float rangeRec = default;
    // 리스폰 시간
    public float respawn = default;
    // 오브젝트 레어도
    public int rarity = default;
    // 아이템 설명
    public string itemInfo = default;
    // 아이템을 획득 할 수 있는 힌트
    public string getMap = default;
    // PC 행동에 참고 해야 할 특기사항
    public string note = default;
    // 아이템 아이콘 이미지 고유 번호
    public int itemImageNum = default;
    // 아이템 영어 이름
    public string itemEnglishName = default;

    #endregion 재료 아이템 정보 설정

    #region 소비 아이템 정보 설정

    // ########## 소비 아이템 기본 정보 ##########
    // 요리의 종류
    // 0 : 기본 효과 요리
    // 1 : 현재 응가 게이지 감소
    // 2 : 등산 점프력 상승
    // 3 : 현재 응가 게이지 감소 + 등산 점프력 상승
    public int cookType = default;
    // 포만감 게이지 증가량
    public int satietyGauge = default;
    // 응가 게이지 증가량
    public int pooGauge = default;
    // 특수 효과의 지속 시간
    public float durationTime = default;
    // 다시 섭취하기 위한 시간 (모든 음식 동시 쿨타임 적용)
    public float coolTime = default;
    // 특수 효과 유무를 판단하기 위한 데이터
    public int utile = default;

    #endregion 소비 아이템 정보 설정

    #region 도구 아이템 정보 설정

    // ########## 도구 아이템 기본 정보 ##########
    // 영구 사용 타입
    public bool infinity = false;
    // 획득 정보 설명
    public string itemHint = default;

    #endregion 도구 아이템 정보 설정

    // 아이템 중첩 수
    public int itemStack = default;
    // 아이템 타입
    public ItemType itemType;

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
    STUFF,
    // 컬렉션
    COLLECT
}     // enum ItemType

////////// 아이템 목록 //////////

///// 도구 목록 /////


///// 음식 목록 /////
// 002 : 딸기
// 003 : 우유
// 004 : 딸기 우유
// 006 : 송이 불고기

///// 재료 목록 /////
// 001 : 고기
// 005 : 송이 버섯


