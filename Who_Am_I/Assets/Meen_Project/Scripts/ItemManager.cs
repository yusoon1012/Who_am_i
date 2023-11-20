using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    #region 변수 설정

    // 인스턴스로 생성
    public static ItemManager instance;

    // 아이템마다 등록된 아이콘 이미지 목록
    public Image[] itemImages = new Image[30];

    // 인벤토리에 저장되어 있는 모든 아이템 수
    private int itemCount = default;
    // 인벤토리에 저장되어 있는 모든 아이템들의 키 값 (아이템 이름)
    private string[] itemKeys = new string[30];

    // 원격 아이템 데이터 서버에서 불러올 아이템 딕셔너리 목록
    public Dictionary<string, int> testDic = new Dictionary<string, int>();

    // 모든 아이템 데이터 베이스
    public Dictionary<string, ItemsMain> itemDataBase = new Dictionary<string, ItemsMain>();
    // 장비 아이템 타입 인벤토리 데이터
    Dictionary<string, ItemsMain> equipments = new Dictionary<string, ItemsMain>();
    // 음식 아이템 타입 인벤토리 데이터
    Dictionary<string, ItemsMain> foods = new Dictionary<string, ItemsMain>();
    // 재료 아이템 타입 인벤토리 데이터
    Dictionary<string, ItemsMain> stuffs = new Dictionary<string, ItemsMain>();
    // 제작 아이템 데이터 베이스
    Dictionary<string, CraftingMain> crafting = new Dictionary<string, CraftingMain>();

    #endregion 변수 설정

    void Awake()
    {
        if (instance == null || instance == default) { instance = this; DontDestroyOnLoad(instance.gameObject); }
        else { Destroy(gameObject); }

        itemCount = 0;
    }     // Awake()

    void Start()
    {
        // 게임 이어하기 클릭 시 저장된 아이템 로드 테스트
        testDic.Add("레드 포션", 11);
        testDic.Add("블루 포션", 15);
        testDic.Add("롱 소드", 1);
        testDic.Add("철", 4);
        testDic.Add("사파이어", 10);
        testDic.Add("블루 아뮬렛", 1);
        testDic.Add("고기", 6);
        testDic.Add("빨간 사과", 8);

        //* 아이템 데이터 베이스에 아이템 추가
        itemDataBase.Add("레드 포션", new Items001());
        itemDataBase.Add("블루 포션", new Items002());
        itemDataBase.Add("롱 소드", new Items003());
        itemDataBase.Add("철", new Items004());
        itemDataBase.Add("사파이어", new Items005());
        itemDataBase.Add("블루 아뮬렛", new Items006());
        itemDataBase.Add("빨간 사과", new Items007());
        itemDataBase.Add("고기", new Items008());
        //* 아이템 데이터 베이스에 아이템 추가

        //* 제작 데이터 베이스에 아이템 추가
        crafting.Add("블루 아뮬렛", new Crafting001());
        //* 제작 데이터 베이스에 아이템 추가
    }     // Start()

    #region 아이템 타입 체크 기능

    // 아이템의 타입을 찾아 내보내는 함수
    public int ItemTypeCheck(string itemName, out int itemType)
    {
        ItemsMain itemInfo = new ItemsMain();

        if (itemDataBase.ContainsKey(itemName))
        {
            itemInfo = itemDataBase[itemName];

            if (itemInfo.itemType == ItemType.EQUIPMENT)
            {
                itemType = 0;
            }
            else if (itemInfo.itemType == ItemType.FOOD)
            {
                itemType = 1;
            }
            else if (itemInfo.itemType == ItemType.STUFF)
            {
                itemType = 2;
            }
            else
            {
                itemType = 3;
            }
        }
        else
        {
            itemType = 3;
        }

        return itemType;
    }     // ItemTypeCheck()

    #endregion 아이템 타입 체크 기능

    #region 인벤토리 기능

    // 인벤토리에 아이템이 없을 때 새로운 아이템을 추가하고, 아이템이 있을 시 갯수를 중첩시키는 함수
    public ItemsMain InventoryAdd(string itemName, int num, out ItemsMain itemInfo)
    {
        ItemTypeCheck(itemName, out int itemTypeNum);

        if (itemTypeNum == 0)
        {
            if (equipments.ContainsKey(itemName))
            {
                itemInfo = equipments[itemName];
                itemInfo.itemStack += num;
                Debug.LogFormat("장비 인벤토리에 {0} 아이템 {1} 개를 중첩시켰습니다. (총 아이템 갯수 : {2})", itemInfo.itemName, num,
                    itemInfo.itemStack);
            }
            else
            {
                itemInfo = itemDataBase[itemName];
                equipments.Add(itemInfo.itemName, itemInfo);
                itemInfo.itemStack += num;
                Debug.LogFormat("장비 인벤토리에 {0} 아이템 {1} 개를 인벤토리에 넣었습니다. (총 아이템 갯수 : {2})", itemInfo.itemName, num,
                    itemInfo.itemStack);
            }
        }
        else if (itemTypeNum == 1)
        {
            if (foods.ContainsKey(itemName))
            {
                itemInfo = foods[itemName];
                itemInfo.itemStack += num;
                Debug.LogFormat("음식 인벤토리에 {0} 아이템 {1} 개를 중첩시켰습니다. (총 아이템 갯수 : {2})", itemInfo.itemName, num,
                    itemInfo.itemStack);
            }
            else
            {
                itemInfo = itemDataBase[itemName];
                foods.Add(itemInfo.itemName, itemInfo);
                itemInfo.itemStack += num;
                Debug.LogFormat("음식 인벤토리에 {0} 아이템 {1} 개를 인벤토리에 넣었습니다. (총 아이템 갯수 : {2})", itemInfo.itemName, num,
                    itemInfo.itemStack);
            }
        }
        else if (itemTypeNum == 2)
        {
            if (stuffs.ContainsKey(itemName))
            {
                itemInfo = stuffs[itemName];
                itemInfo.itemStack += num;
                Debug.LogFormat("재료 인벤토리에 {0} 아이템 {1} 개를 중첩시켰습니다. (총 아이템 갯수 : {2})", itemInfo.itemName, num,
                    itemInfo.itemStack);
            }
            else
            {
                itemInfo = itemDataBase[itemName];
                stuffs.Add(itemInfo.itemName, itemInfo);
                itemInfo.itemStack += num;
                Debug.LogFormat("재료 인벤토리에 {0} 아이템 {1} 개를 인벤토리에 넣었습니다. (총 아이템 갯수 : {2})", itemInfo.itemName, num,
                    itemInfo.itemStack);
            }
        }
        else
        {
            itemInfo = null;
        }

        return itemInfo;
    }     // InventoryAdd()

    // 인벤토리 안에 총 아이템 갯수를 확인하는 함수
    public int InventoryCountCheck(int num, int page, out int checkNum)
    {
        itemCount = 0;

        switch (page)
        {
            case 0:
                itemCount = equipments.Count;
                break;
            case 1:
                itemCount = foods.Count;
                break;
            case 2:
                itemCount = stuffs.Count;
                break;
            default:
                itemCount = 0;
                break;
        }

        checkNum = itemCount;

        return checkNum;
    }     // InventoryCountCheck()

    // 인벤토리 안에 모든 아이템의 이름을 순차적으로 불러오는 함수
    public string[] InventoryTotal(int num, int page, out string[] itemNames)
    {
        itemCount = 0;
        itemNames = new string[num + 1];     // ???

        switch (page)
        {
            case 0:
                foreach (KeyValuePair<string, ItemsMain> inventoryItems in equipments)
                {
                    itemNames[itemCount] = inventoryItems.Key;
                    itemCount += 1;
                }
                break;
            case 1:
                foreach (KeyValuePair<string, ItemsMain> inventoryItems in foods)
                {
                    itemNames[itemCount] = inventoryItems.Key;
                    itemCount += 1;
                }
                break;
            case 2:
                foreach (KeyValuePair<string, ItemsMain> inventoryItems in stuffs)
                {
                    itemNames[itemCount] = inventoryItems.Key;
                    itemCount += 1;
                }
                break;
            default:
                itemNames = default;
                break;
        }

        return itemNames;
    }     // InventoryTotal()

    // 인벤토리 안에 모든 아이템의 중첩 갯수를 순차적으로 불러오는 함수
    public int InventoryStack(string itemName, int page, out int stack)
    {
        ItemsMain itemInfo = new ItemsMain();

        switch (page)
        {
            case 0:
                if (equipments.ContainsKey(itemName))
                {
                    itemInfo = equipments[itemName];
                    stack = itemInfo.itemStack;
                }
                else
                {
                    stack = 0;
                }
                break;
            case 1:
                if (foods.ContainsKey(itemName))
                {
                    itemInfo = foods[itemName];
                    stack = itemInfo.itemStack;
                }
                else
                {
                    stack = 0;
                }
                break;
            case 2:
                if (stuffs.ContainsKey(itemName))
                {
                    itemInfo = stuffs[itemName];
                    stack = itemInfo.itemStack;
                }
                else
                {
                    stack = 0;
                }
                break;
            default:
                stack = 0;
                break;
        }
        
        return stack;
    }     // InventoryStack()

    // 아이템 매니저의 인벤토리 안에 모든 아이템들의 이미지 넘버를 불러오는 함수
    public int ItemImage(string itemName, int page, out int imageNum)
    {
        switch (page)
        {
            case 0:
                if (equipments.ContainsKey(itemName))
                {
                    imageNum = equipments[itemName].itemImageNum;
                }
                else
                {
                    imageNum = 0;
                }
                break;
            case 1:
                if (foods.ContainsKey(itemName))
                {
                    imageNum = foods[itemName].itemImageNum;
                }
                else
                {
                    imageNum = 0;
                }
                break;
            case 2:
                if (stuffs.ContainsKey(itemName))
                {
                    imageNum = stuffs[itemName].itemImageNum;
                }
                else
                {
                    imageNum = 0;
                }
                break;
                // 3 의 아이템 타입은 소지중인 아이템이 아닌 아이템 전체로 아이템 이미지 넘버를 불러옴
            case 3:
                if (itemDataBase.ContainsKey(itemName))
                {
                    imageNum = itemDataBase[itemName].itemImageNum;
                }
                else
                {
                    imageNum = 0;
                }
                break;
            default:
                imageNum = 0;
                break;
        }
        
        return imageNum;
    }     // ItemImage()

    // 인벤토리에서 아이템 아이콘 클릭 시 인벤토리에서 해당 아이템의 설명 정보를 return 시키는 함수
    public string LoadItemInfoText(string itemName, int page, out string itemInfoText)
    {
        switch (page)
        {
            case 0:
                if (equipments.ContainsKey(itemName))
                {
                    itemInfoText = equipments[itemName].itemInfo;
                }
                else
                {
                    itemInfoText = string.Empty;
                }
                break;
            case 1:
                if (foods.ContainsKey(itemName))
                {
                    itemInfoText = foods[itemName].itemInfo;
                }
                else
                {
                    itemInfoText = string.Empty;
                }
                break;
            case 2:
                if (stuffs.ContainsKey(itemName))
                {
                    itemInfoText = stuffs[itemName].itemInfo;
                }
                else
                {
                    itemInfoText = string.Empty;
                }
                break;
            // 3 의 아이템 타입은 소지중인 아이템이 아닌 아이템 전체로 아이템 이미지 넘버를 불러옴
            case 3:
                if (itemDataBase.ContainsKey(itemName))
                {
                    itemInfoText = itemDataBase[itemName].itemInfo;
                }
                else
                {
                    itemInfoText = string.Empty;
                }
                break;
            default:
                itemInfoText = string.Empty;
                break;
        }

        return itemInfoText;
    }     // LoadItemInfoText()

    // 인벤토리에서 아이템 중첩 수를 빼고, 중첩 수가 0 이하면 해당 아이템을 삭제하는 함수
    public ItemsMain InventoryRemove(string itemName, int stack, int type, out ItemsMain itemInfo)
    {
        switch (type)
        {
            case 0:
                if (equipments.ContainsKey(itemName))
                {
                    itemInfo = equipments[itemName];
                    itemInfo.itemStack -= stack;
                    if (itemInfo.itemStack <= 0)
                    {
                        equipments.Remove(itemName);
                    }
                }
                else
                {
                    itemInfo = null;
                }
                break;
            case 1:
                if (foods.ContainsKey(itemName))
                {
                    itemInfo = foods[itemName];
                    itemInfo.itemStack -= stack;
                    if (itemInfo.itemStack <= 0)
                    {
                        foods.Remove(itemName);
                    }
                }
                else
                {
                    itemInfo = null;
                }
                break;
            case 2:
                if (stuffs.ContainsKey(itemName))
                {
                    itemInfo = stuffs[itemName];
                    itemInfo.itemStack -= stack;
                    if (itemInfo.itemStack <= 0)
                    {
                        stuffs.Remove(itemName);
                    }
                }
                else
                {
                    itemInfo = null;
                }
                break;
            default:
                itemInfo = null;
                break;
        }

        return itemInfo;
    }     // InventoryRemove()

    #endregion 인벤토리 기능

    #region 크래프팅 기능

    // 아이템 제작 시 재료 아이템의 갯수를 불러오는 함수
    public int CraftingLength(string craftingName, out int length)
    {
        if (crafting.ContainsKey(craftingName))
        {
            length = crafting[craftingName].craftingLength;
        }
        else
        {
            length = 0;
        }

        return length;
    }     // CraftingLength()

    // 아이템 제작 시 재료 아이템들의 이름을 불러오는 함수
    public string CraftingStuffName(string craftingName, int count, out string stuffName)
    {
        if (crafting.ContainsKey(craftingName))
        {
            stuffName = crafting[craftingName].stuffName[count];
        }
        else
        {
            stuffName = null;
        }

        return stuffName;
    }     // CraftingStuffName()

    // 아이템 제작 시 재료 아이템들의 조건 갯수를 불러오는 함수
    public int CraftingStuffStack(string craftingName, int count, out int stuffStack)
    {
        if (crafting.ContainsKey(craftingName))
        {
            stuffStack = crafting[craftingName].stuffStack[count];
        }
        else
        {
            stuffStack = 0;
        }

        return stuffStack;
    }     // CraftingStuffStack()

    // 제작 완료 시 제작 될 아이템을 얻을 때 중첩 값을 불러오는 함수
    public int CraftingStack(string craftingName, out int craftingStack)
    {
        if (crafting.ContainsKey(craftingName))
        {
            craftingStack = crafting[craftingName].craftingStack;
        }
        else
        {
            craftingStack = 0;
        }

        return craftingStack;
    }     // CraftingStack()

    #endregion 크래프팅 기능

    // Fix : 데이터 베이스에 추가할 예정
    public void SendDataTest(string itemType, string itemName, int stack)
    {
        // 데이터 베이스 함수에 추가할 예정
    }
}