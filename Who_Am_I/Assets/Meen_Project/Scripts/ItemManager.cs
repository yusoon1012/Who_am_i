using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    #region 변수 설정

    // 인스턴스로 생성
    public static ItemManager instance;
    // 퀵슬롯 트랜스폼
    public Transform quickSlotTf;
    // 컬렉션 정보 트랜스폼
    public Transform collectionInfoTf;

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
    // 아이템 도감에 저장되는 아이템 데이터
    public Dictionary<string, ItemsMain> Encyclopedia = new Dictionary<string, ItemsMain>();

    // 장비 아이템 타입 인벤토리 데이터
    Dictionary<string, ItemsMain> equipments = new Dictionary<string, ItemsMain>();
    // 음식 아이템 타입 인벤토리 데이터
    Dictionary<string, ItemsMain> foods = new Dictionary<string, ItemsMain>();
    // 재료 아이템 타입 인벤토리 데이터
    Dictionary<string, ItemsMain> stuffs = new Dictionary<string, ItemsMain>();
    // 제작 아이템 데이터 베이스
    Dictionary<string, CraftingMain> crafting = new Dictionary<string, CraftingMain>();
    // 도감에 추가할 아이템 그룹 정보 데이터
    Dictionary<string, int> collectionItems = new Dictionary<string, int>();

    #endregion 변수 설정

    void Awake()
    {
        if (instance == null || instance == default)
        {
            instance = this; DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        Debug.Log(auth.CurrentUser);
        itemCount = 0;
    }     // Awake()

    void Start()
    {
        //List<Dictionary<string, object>> itemCsvTest = LGM_CSVReader.Read("test");

        //for (int i = 0; i < itemCsvTest.Count; i++)
        //{
        //    Debug.LogFormat("{0}", itemCsvTest[i]["Name"].ToString());
        //}

        // 게임 이어하기 클릭 시 저장된 아이템 로드 테스트
        testDic.Add("고기", 1);
        testDic.Add("딸기", 1);
        testDic.Add("우유", 1);

        //* 아이템 데이터 베이스에 아이템 추가
        itemDataBase.Add("고기", new Items001());
        itemDataBase.Add("딸기", new Items002());
        itemDataBase.Add("우유", new Items003());
        itemDataBase.Add("딸기 우유", new Items004());
        itemDataBase.Add("송이 버섯", new Items005());
        itemDataBase.Add("송이 불고기", new Items006());
        itemDataBase.Add("너프건", new Items007());
        //* 아이템 데이터 베이스에 아이템 추가

        //* 제작 데이터 베이스에 아이템 추가
        crafting.Add("딸기 우유", new Crafting001());
        crafting.Add("송이 불고기", new Crafting002());
        //* 제작 데이터 베이스에 아이템 추가

        //* 컬렉션 그룹 전용 데이터 베이스에 아이템 추가
        collectionItems.Add("고기", 0);
        collectionItems.Add("딸기", 0);
        collectionItems.Add("우유", 0);
        //* 컬렉션 그룹 전용 데이터 베이스에 아이템 추가
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

            //ItemDatabase.Instance.WriteItemInfo("Equipment", itemName, itemInfo.itemStack);
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

            //ItemDatabase.Instance.WriteItemInfo("Food", itemName, itemInfo.itemStack);
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

            //ItemDatabase.Instance.WriteItemInfo("Stuff", itemName, itemInfo.itemStack);
        }
        else
        {
            itemInfo = null;
        }

        // 인벤토리에 아이템을 추가할 때 도감에 등록 가능 여부를 체크하는 함수를 실행함
        AddDictionary(itemName, itemInfo);

        return itemInfo;
    }     // InventoryAdd()

    // 인벤토리에 아이템을 추가할 때 도감에 등록 가능 여부를 체크하는 함수
    private void AddDictionary(string itemName, ItemsMain itemInfo)
    {
        // 도감 목록에 추가된 아이템 정보가 없을 때 실행
        if (!Encyclopedia.ContainsKey(itemName))
        {
            // 도감 목록에 아이템 정보를 추가함
            Encyclopedia.Add(itemName, itemInfo);

            // 컬렉션 목록에 아이템 정보가 존재할 때 실행
            if (collectionItems.ContainsKey(itemName))
            {
                // 컬렉션 그룹 번호 값을 저장함
                int collectionNum = collectionItems[itemName];
                // 컬렉션 정보를 저장하는 클래스로 아이템 정보를 전달함
                collectionInfoTf.GetComponent<SaveCollections>().CheckCollection(itemName, collectionNum);
            }
        }
    }     // AddDictionary()

    // 아이템 데이터 베이스의 정보를 내보내는 함수
    public ItemsMain ReturnItemInfomation(string itemName, int itemType, out ItemsMain itemInfomation)
    {
        switch (itemType)
        {
            // 모든 아이템 정보 데이터에 대한 아이템 정보를 확인하고 내보냄
            case 0:
                if (itemDataBase.ContainsKey(itemName))
                {
                    itemInfomation = itemDataBase[itemName];
                }
                else
                {
                    itemInfomation = null;
                }
                break;
            // 인벤토리에 저장된 아이템 데이터에 대한 아이템 정보를 확인하고 내보냄
            case 1:
                if (equipments.ContainsKey(itemName))
                {
                    itemInfomation = equipments[itemName];
                }
                else if (foods.ContainsKey(itemName))
                {
                    itemInfomation = foods[itemName];
                }
                else if (stuffs.ContainsKey(itemName))
                {
                    itemInfomation = stuffs[itemName];
                }
                else
                {
                    itemInfomation = null;
                }
                break;
            default:
                itemInfomation = null;
                break;
        }

        return itemInfomation;
    }     // ReturnItemInfomation()

    // 인벤토리에 저장된 아이템 정보를 삭제하는 함수
    public void DeleteItem(string itemName)
    {
        // 도구 인벤토리에 아이템이 존재하면
        if (equipments.ContainsKey(itemName))
        {
            // 도구 인벤토리에서 아이템 정보 삭제
            equipments.Remove(itemName);
        }
        // 음식 인벤토리에 아이템이 존재하면
        else if (foods.ContainsKey(itemName))
        {
            // 음식 인벤토리에서 아이템 정보 삭제
            foods.Remove(itemName);
        }
        // 재료 인벤토리에 아이템이 존재하면
        else if (stuffs.ContainsKey(itemName))
        {
            // 재료 인벤토리에서 아이템 정보 삭제
            stuffs.Remove(itemName);
        }
    }

    // 퀵슬롯 칸마다 아이템 정보를 확인해 return 하는 함수
    public ItemsMain QuickSlotItemInfo(string itemName, int itemType, out ItemsMain itemInfo)
    {
        // 현재 퀵슬롯이 도구 페이지면
        if (itemType == 0)
        {
            if (equipments.ContainsKey(itemName))
            {
                itemInfo = equipments[itemName];
            }
            else
            {
                itemInfo = null;
            }
        }
        // 현재 퀵슬롯이 음식 페이지면
        else if (itemType == 1)
        {
            if (foods.ContainsKey(itemName))
            {
                itemInfo = foods[itemName];
            }
            else
            {
                itemInfo = null;
            }
        }
        else
        {
            itemInfo = null;
        }

        return itemInfo;
    }

    // 도감에서 현재 페이지의 아이템들의 이미지값들을 내보내는 함수
    public ItemsMain DictionaryItemImage(string itemName, out ItemsMain items)
    {
        if (itemDataBase.ContainsKey(itemName))
        {
            items = itemDataBase[itemName];
        }
        else
        {
            items = null;
        }

        return items;
    }     // DictionaryItemImage()

    // 도감에서 현재 페이지의 아이템들의 획득, 미획득 상태 값들을 내보내는 함수
    public bool DictionaryCheck(int type, string itemName, out bool itemCheck)
    {
        if (Encyclopedia.ContainsKey(itemName))
        {
            itemCheck = true;
        }
        else
        {
            itemCheck = false;
        }

        return itemCheck;
    }     // DictionaryCheck()

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
        itemNames = new string[30];

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

    // 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수를 실행함
    public bool LoadCheckDisposableCrafting(string itemName, out bool checkImpossible)
    {
        CraftingMain craftingInfo = new CraftingMain();

        // 제작 레시피에서 해당 레시피를 찾음
        if (crafting.ContainsKey(itemName))
        {
            craftingInfo = crafting[itemName];

            // 일회성 제작 레시피인지 체크
            bool checkImpossibleCraft = craftingInfo.disposableType;

            // 일회성 제작 레시피면
            if (checkImpossibleCraft == true)
            {
                // 일회성 제작을 완료한 상태인지 체크
                checkImpossible = craftingInfo.disposable;
            }
            else
            {
                checkImpossible = false;
            }
        }
        else
        {
            checkImpossible = false;
        }

        return checkImpossible;
    }     // LoadCheckDisposableCrafting()

    // 일회성 제작 레시피인지 체크하는 함수
    public bool LoadCheckDisposableType(string itemName, out bool checkItem)
    {
        CraftingMain craftingInfo = new CraftingMain();

        // 제작 레시피에서 해당 레시피를 찾음
        if (crafting.ContainsKey(itemName))
        {
            craftingInfo = crafting[itemName];
            // 일회성 제작 레시피인지 체크
            checkItem = craftingInfo.disposableType;
        }
        else
        {
            checkItem = false;
        }

        return checkItem;
    }     // LoadCheckDisposableItem()

    // 일회성 제작 레시피를 통해 제작을 완료한 후 일회성 제작 완료 상태로 변경하는 함수
    public void ChangeDiaposableCrafting(string itemName)
    {
        CraftingMain craftingInfo = new CraftingMain();

        // 제작 레시피에서 해당 레시피를 찾음
        if (crafting.ContainsKey(itemName))
        {
            craftingInfo = crafting[itemName];
            // 일회성 제작 완료 상태로 변경
            craftingInfo.disposable = true;
        }
    }     // ChangeDiaposableCrafting()

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

    // 아이템 데이터 베이스에서 아이템 버리기 기능
    public string RemoveItem(string itemName, int itemType, out string dropItemName)
    {
        ItemsMain itemInfo = new ItemsMain();

        switch (itemType)
        {
            case 0:
                if (equipments.ContainsKey(itemName))
                {
                    itemInfo = equipments[itemName];
                    itemInfo.itemStack = 0;
                    equipments.Remove(itemName);
                    dropItemName = itemName;
                }
                else
                {
                    dropItemName = null;
                }
                break;
            case 1:
                if (foods.ContainsKey(itemName))
                {
                    itemInfo = foods[itemName];
                    itemInfo.itemStack = 0;
                    foods.Remove(itemName);
                    dropItemName = itemName;
                }
                else
                {
                    dropItemName = null;
                }
                break;
            case 2:
                if (stuffs.ContainsKey(itemName))
                {
                    itemInfo = stuffs[itemName];
                    itemInfo.itemStack = 0;
                    stuffs.Remove(itemName);
                    dropItemName = itemName;
                }
                else
                {
                    dropItemName = null;
                }
                break;
            default:
                dropItemName = null;
                break;
        }

        return dropItemName;
    }     // RemoveItem()

    // 인벤토리에서 아이템 중첩 수를 빼고, 중첩 수가 0 이하면 해당 아이템을 삭제하는 함수
    public ItemsMain RemoveCraftingItem(string itemName, int stack, int type, out ItemsMain itemInfo)
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
                        itemInfo.itemStack = 0;
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
                        quickSlotTf.GetComponent<QuickSlot>().UseFoodsCheck(itemName);
                        foods.Remove(itemName);
                        itemInfo.itemStack = 0;
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
                        itemInfo.itemStack = 0;
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

    // 아이템 제작 시 효과 타입 값을 내보내는 함수
    public int CraftingEffectType(string craftingName, out int effectType)
    {
        CraftingMain craftingInfo = new CraftingMain();

        if (crafting.ContainsKey(craftingName))
        {
            effectType = crafting[craftingName].utile;
        }
        else
        {
            effectType = 0;
        }

        return effectType;
    }     // CraftingEffectType()

    // 아이템 제작 시 일반 효과 수치들의 값을 내보내는 함수
    public int CraftingEffectCount(string craftingName, int count, out int effectCount)
    {
        ItemsMain itemInfo = new ItemsMain();

        if (itemDataBase.ContainsKey(craftingName))
        {
            switch (count)
            {
                case 0:
                    effectCount = itemDataBase[craftingName].satietyGauge;
                    break;
                case 1:
                    effectCount = itemDataBase[craftingName].pooGauge;
                    break;
                default:
                    effectCount = 0;
                    break;
            }
        }
        else
        {
            effectCount = 0;
        }

        return effectCount;
    }     // CraftingEffectCount()

    // 아이템 제작 시 특수 효과 값을 내보내는 함수
    public string CraftingEffectInfo(string craftingName, out string effectInfo)
    {
        CraftingMain craftingInfo = new CraftingMain();

        if (crafting.ContainsKey(craftingName))
        {
            effectInfo = crafting[craftingName].effectInfo;
        }
        else
        {
            effectInfo = null;
        }

        return effectInfo;
    }     // CraftingEffectInfo()

    // 아이템 제작 시 재료들의 정보를 내보내는 함수
    public string CraftingStuffInfo(string stuffName, out string stuffInfo)
    {
        ItemsMain itemInfo = new ItemsMain();

        if (itemDataBase.ContainsKey(stuffName))
        {
            stuffInfo = itemDataBase[stuffName].itemInfo;
        }
        else
        {
            stuffInfo = null;
        }

        return stuffInfo;
    }     // CraftingStuffInfo()

    #endregion 크래프팅 기능

    // Fix : 데이터 베이스에 추가할 예정
    public void SendDataTest(string itemType, string itemName, int stack)
    {
        // 데이터 베이스 함수에 추가할 예정
    }

    public bool QuestLootItemCheck(string itemName, int itemType, int lootCount, out bool lootCheck)
    {
        ItemsMain itemInfo = new ItemsMain();

        switch (itemType)
        {
            case 0:
                if (equipments.ContainsKey(itemName))
                {
                    itemInfo = equipments[itemName];
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
                }
                else
                {
                    itemInfo = null;
                }
                break;
            default:
                break;
        }

        if (itemInfo != null)
        {
            if (itemInfo.itemStack >= lootCount)
            {
                lootCheck = true;
            }
            else
            {
                lootCheck = false;
            }
        }
        else
        {
            lootCheck = false;
        }

        return lootCheck;
    }
}