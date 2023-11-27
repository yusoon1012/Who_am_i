using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region 변수 설정

    // 인벤토리에 출력될 아이템 슬롯 오브젝트 목록
    public GameObject[] itemSlot = new GameObject[30];
    // 인벤토리에 출력될 아이템 아이콘 목록
    public Image[] itemSlotImages = new Image[30];
    // 인벤토리에 페이지 이미지
    public Image[] pageImages = new Image[3];
    // 아이템 아이콘마다 출력될 아이템 중첩 수
    public Text[] itemStakcsText = new Text[30];
    // 아이템 상세 창에서 아이템 큰 이미지
    public Image largeImage;
    // 아이템 상세 창에서 아이템 이름
    public Text itemNameText;
    // 아이템 클릭 시 설명 창
    public GameObject itemInfo;
    // 아이템 클릭 시 설명 글자
    public Text itemInfoTextUI;
    // 장착 / 사용 버튼 UI 텍스트
    public Text usingInfoText;
    // 장착 / 사용 버튼 UI 오브젝트
    public GameObject usingInfoObj;
    // 퀵슬롯 버튼 UI 오브젝트
    public GameObject quickSlotObj;
    // 인벤토리 상세 정보창의 버튼 오브젝트들
    public GameObject[] detailInfoSlot = new GameObject[4];
    // 인벤토리 UI 에 있는 아이템 수량 페이지 화살표 오브젝트
    public GameObject[] itemGroupPageObj = new GameObject[2];
    // 인벤토리 창 오브젝트
    public GameObject inventory;

    // 인벤토리를 열고있는 상태인지 체크
    public bool lookInventory = false;
    // 인벤토리 상세 정보에 들어와 있는 상태인지 체크
    public bool lookItemDetailInfo = false;
    // Test : 인벤토리의 1개의 아이템 그룹 페이지당 아이템 아이콘 갯수
    public int maxItemSlot = default;

    // 인벤토리 창에서 현재 아이템 선택 색
    private Color currentColor = default;
    // 인벤토리 창에서 새로운 아이템 선택 색
    private Color newColor = default;
    // Test : 도감 전용 테스트 색
    private Color testColor = default;

    // 인벤토리에서 아이템 아이콘 선택 색 변경 이미지
    private Image[] itemSlotColor = new Image[30];
    // 아이템 상세 정보에서의 버튼 색 변경 이미지
    private Image[] detailSlotColor = new Image[4];
    // 플레이어 트랜스폼
    private Transform playerTf = default;

    // 인벤토리에 등록된 아이템들의 이름 목록
    private string[] itemNames = new string[240];
    // 인벤토리에 등록된 아이템들의 중첩 수 목록
    private int[] itemStacks = new int[240];
    // 인벤토리에 등록된 아이템들의 아이콘 이미지 값 목록
    private int[] imageNums = new int[240];
    // 인벤토리에 등록된 아이템들의 총 갯수
    private int itemCount = default;
    // 인벤토리 아이템 그룹 페이지의 수
    private int itemGroupCount = default;
    // 참조되는 아이템의 중첩 수
    private int stack = default;
    // 참조되는 아이템의 아이콘 이미지 값
    private int imageNum = default;
    // 인벤토리 페이지 현재 값
    private int inventoryPage = default;
    // 인벤토리 아이템 그룹 페이지 현재 값
    private int itemGroupPage = default;
    // 아이템을 클릭해 설명칸이 출력된 상태인지 체크
    private bool lookItemInfo = false;
    // 아이템 목록에서의 현재 아이템 선택 확인 값
    private int order = default;
    // 아이템 상세 목록에서의 현재 선택한 버튼 확인 값
    private int detailOrder = default;
    // 인벤토리에 아이템들을 표시할 때 몇칸까지 표시할지 체크
    private int countItemSlot = default;
    // 이전, 다음 아이템 수량 페이지가 존재 하는지 체크
    private bool[] itemGroupPageCheck = new bool[2];

    // Test : 데이터 베이스 연동 테스트 딕셔너리
    Dictionary<string, int> test = new Dictionary<string, int>();

    #endregion 변수 설정

    void Awake()
    {
        itemCount = 0;
        stack = 0;
        inventoryPage = 0;
        order = 0;
        detailOrder = 0;
        itemGroupPage = 0;
        itemGroupCount = 0;
        countItemSlot = 0;
        maxItemSlot = 5;

    }     // Awake()

    void Start()
    {
        playerTf = GetComponent<Transform>().transform;

        // 인벤토리에서 아이템 아이콘 선택 색 변경 이미지 참조
        for (int i = 0; i < 24; i++)
        {
            itemSlotColor[i] = itemSlot[i].GetComponent<Image>();
        }

        // 인벤토리 상세 정보창에서 버튼 선택 색 변경 이미지 참조
        for (int j = 0; j < 4; j++)
        {
            detailSlotColor[j] = detailInfoSlot[j].GetComponent<Image>();
        }
    }     // Start()

    void Update()
    {
        //* Test : 해당 키를 누르면 인벤토리에 아이템 추가 기능
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddInventory("레드 포션", 3);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            AddInventory("블루 포션", 4);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AddInventory("롱 소드", 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            AddInventory("철", 2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AddInventory("사파이어", 2);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Test();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            ReadTest();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            DictionaryTest(Color.black);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            DictionaryTest(Color.white);
        }

        //* End Test : 해당 키를 누르면 인벤토리에 아이템 추가 기능
    }     // Update()

    // Test : 도감 미획득 아이템 이미지 색 변화
    public void DictionaryTest(Color color)
    {
        testColor = color;
        itemSlotImages[0].color = testColor;
    }

    #region 아이템 획득 기능

    // 아이템 매니저의 인벤토리에 아이템이 없을 때 새로운 아이템을 추가하고, 아이템이 있을 시 갯수를 중첩시키는 함수
    public void AddInventory(string itemName, int num)
    {
        ItemsMain itemInfo = new ItemsMain();
        ItemManager.instance.InventoryAdd(itemName, num, out itemInfo);

        SendItemData(itemName, num);

        if (lookInventory == true)
        {
            CleanInventory();

            ControlInventory();
        }
    }     // AddInventory()

    #endregion 아이템 획득 기능

    // 아이템 정보가 변경될 때 마다 데이터 베이스에 정보를 보내주는 함수
    // 추가 : 타입, 이름, 갯수
    // 삭제 : 타입, 이름
    public void SendItemData(string itemName, int stack)
    {
        string itemTypeStr = string.Empty;

        ItemManager.instance.ItemTypeCheck(itemName, out int itemType);

        if (itemType == 0)
        {
            itemTypeStr = "Equipment";
        }
        else if (itemType == 1)
        {
            itemTypeStr = "Food";
        }
        else if (itemType == 2)
        {
            itemTypeStr = "Stuff";
        }

        ItemManager.instance.SendDataTest(itemTypeStr, itemName, stack);
    }     // SendItemData()

    public void Test()
    {
        test = ItemManager.instance.testDic;
    }

    public void ReadTest()
    {
        foreach (KeyValuePair<string, int> pair in test)
        {
            string name = pair.Key;
            int stack = pair.Value;

            AddInventory(name, stack);
        }
    }

    #region 인벤토리 아이템 타입 창 이동 기능

    // 아이템 수량 그룹 페이지의 값을 변경하는 함수
    public void ChangeItemGroupPage(int keyType)
    {
        if (keyType == 0 && itemGroupPageCheck[0] == true)
        {
            itemGroupPage -= 1;

            CleanInventory();

            ControlInventory();
        }
        else if (keyType == 1 && itemGroupPageCheck[1] == true)
        {
            itemGroupPage += 1;

            CleanInventory();

            ControlInventory();
        }
    }     // ChangeItemGroupPage()

    public void ChangePage(int arrowType)
    {
        if (arrowType == 2)
        {
            PageUp();
        }
        else if (arrowType == 3)
        {
            PageDown();
        }
    }

    // 인벤토리 이전 타입 창 이동 함수
    public void PageUp()
    {
        // 인벤토리 페이지 변경
        if (inventoryPage == 0)
        {
            inventoryPage = 2;
        }
        else
        {
            inventoryPage -= 1;
        }

        // 페이지 버튼 알파값 변경
        for (int i = 0; i < 3; i++)
        {
            Color color = pageImages[i].color;

            if (inventoryPage == i)
            {
                color.a = 1f;
                pageImages[i].color = color;
            }
            else
            {
                color.a = 0.2f;
                pageImages[i].color = color;
            }
        }

        CleanInventory();

        ControlInventory();
    }     // PageUp()

    // 인벤토리 다음 타입 창 이동 함수
    public void PageDown()
    {
        // 인벤토리 페이지 변경
        if (inventoryPage == 2)
        {
            inventoryPage = 0;
        }
        else
        {
            inventoryPage += 1;
        }

        // 페이지 버튼 알파값 변경
        for (int i = 0; i < 3; i++)
        {
            Color color = pageImages[i].color;

            if (inventoryPage == i)
            {
                color.a = 1f;
                pageImages[i].color = color;
            }
            else
            {
                color.a = 0.2f;
                pageImages[i].color = color;
            }
        }

        CleanInventory();

        ControlInventory();
    }     // PageDown()

    #endregion 인벤토리 아이템 타입 창 이동 기능

    #region 인벤토리 아이템 창 기능

    // 방향키를 누를 때 마다 아이템 아이콘 선택 값 변경 함수
    public void OrderCheck(int keyType)
    {
        // 이전에 선택한 아이템 창 색을 짙은 색으로 변경
        currentColor = new Color32(155, 155, 155, 255);
        itemSlotColor[order].color = currentColor;

        if (keyType == 2)
        {
            if (order == 0)
            {
                order = 23;
            }
            else
            {
                order -= 1;
            }
        }
        else if (keyType == 3)
        {
            if (order == 23)
            {
                order = 0;
            }
            else
            {
                order += 1;
            }
        }
        else if (keyType == 0)
        {
            if (order < 6)
            {
                order += 18;
            }
            else
            {
                order -= 6;
            }
        }
        else if (keyType == 1)
        {
            if (order > 17)
            {
                order -= 18;
            }
            else
            {
                order += 6;
            }
        }

        ShowOrder();
    }     // OrderCheck()

    // 방향키를 눌러 새롭게 선택되는 아이템 아이콘의 정보를 표시하는 함수
    public void ShowOrder()
    {
        // 새롭게 선택되는 아이템 창 색을 밝게 변경
        newColor = new Color32(255, 255, 255, 255);
        itemSlotColor[order].color = newColor;

        itemSlot[order].GetComponent<SaveItemInfo>().LoadNameInfo(out string itemName);
        OnItemInfo(itemName);
    }     // ShowOrder()

    // 아이템 매니저의 인벤토리 안에 총 아이템 갯수를 확인하고, 모든 아이템의 이름과 중첩 갯수를 순차적으로 불러오게 제어하는 함수
    public void ControlInventory()
    {
        if (lookInventory == false)
        {
            inventory.SetActive(true);
        }

        lookInventory = true;

        ResetVariable();
        CountInventory(inventoryPage);
        CountGroupInventory();
        TotalInventory(itemCount, inventoryPage);
        StackInventory(itemCount, inventoryPage);
        ImageItems(itemCount, inventoryPage);
        ResultInventory(inventoryPage);
        ShowOrder();
    }     // ControlInventory()

    // 인벤토리의 모든 아이템 값들을 초기화 시키는 함수
    public void CleanInventory()
    {
        if (lookItemInfo == true)
        {
            lookItemInfo = false;
            itemInfoTextUI.text = string.Format(" ");
            itemInfo.gameObject.SetActive(false);
        }

        for (int i = 0; i < itemCount; i++)
        {
            itemSlot[i].GetComponent<SaveItemInfo>().CleanSlot();
            itemSlotImages[i].sprite = ItemManager.instance.itemImages[0].sprite;
            itemStakcsText[i].text = string.Format(" ");
            itemStacks[i] = 0;
        }

        itemCount = 0;
    }     // CleanInventory()

    // 인벤토리를 종료할 때 비활성화 하는 함수
    public void ExitInventory()
    {
        if (lookItemInfo == true)
        {
            itemInfo.gameObject.SetActive(false);
        }

        lookItemInfo = false;

        // 인벤토리를 닫기 전에 선택한 아이템 창 색을 짙은 색으로 변경
        currentColor = new Color32(155, 155, 155, 255);
        itemSlotColor[order].color = currentColor;

        CleanInventory();

        order = 0;
        detailOrder = 0;
        itemGroupCount = 0;
        itemGroupPage = 0;
        lookInventory = false;

        inventory.SetActive(false);
    }     // ExitInventory()

    // 아이템 매니저의 인벤토리 출력을 실행하기 전에 변수를 초기화 하는 함수
    public void ResetVariable()
    {
        itemCount = 0;
        stack = 0;
    }     // ResetVariable()

    // 아이템 매니저의 인벤토리 안에 총 아이템 갯수를 확인하는 함수
    public void CountInventory(int page)
    {
        ItemManager.instance.InventoryCountCheck(0, page, out itemCount);
    }     // CountInventory()

    // 아이템 총 갯수에 따른 아이템 그룹 페이지의 수를 체크하는 함수
    public void CountGroupInventory()
    {
        itemGroupCount = itemCount / maxItemSlot;
    }     // CountGroupInventory()

    // 아이템 매니저의 인벤토리 안에 모든 아이템의 이름을 순차적으로 불러오는 함수
    public void TotalInventory(int count, int page)
    {
        ItemManager.instance.InventoryTotal(count, page, out itemNames);
    }     // TotalInventory()

    // 아이템 매니저의 인벤토리 안에 모든 아이템의 중첩 갯수를 불러오는 함수
    public void StackInventory(int count, int page)
    {
        for (int i = 0; i < count; i++)
        {
            ItemManager.instance.InventoryStack(itemNames[i], page, out stack);
            itemStacks[i] = stack;
        }
    }     // StackInventory()

    // 아이템 매니저의 인벤토리 안에 모든 아이템들의 이미지 넘버를 불러오는 함수
    public void ImageItems(int count, int page)
    {
        for (int i = 0; i < count; i++)
        {
            ItemManager.instance.ItemImage(itemNames[i], page, out imageNum);
            imageNums[i] = imageNum;
        }
    }     // ImageItems()

    // 인벤토리 창을 활성화 할 때 인벤토리 안에 모든 아이템 정보들이 순차적으로 나열되는 함수
    public void ResultInventory(int page)
    {
        if (itemCount == 0) { return; }

        // 몇번째 아이템 그룹 페이지인지 체크하고 인벤토리 배열에 아이템 그룹 페이지만큼의 아이템을 출력
        int checkCount = itemGroupPage * maxItemSlot;
        // 마지작 아이템 그룹 페이지를 체크하고 for 문의 마지막 정수를 표시
        int maxCheckCount = (itemGroupPage + 1) * maxItemSlot;

        // 인벤토리의 총 아이템 갯수가 마지막 아이템 그룹 페이지보다 작거나 같은지 체크
        if (itemCount < maxCheckCount)
        {
            // 아이템 총 갯수가 마지막 그룹 페이지보다 작거나 같으면 for 문의 마지막 정수 값을 계산
            int count = maxCheckCount - itemCount;
            int result = maxItemSlot - count;

            countItemSlot = count + 1;

            itemGroupPageObj[1].SetActive(false);
            itemGroupPageCheck[1] = false;
        }
        else if (itemCount == maxCheckCount)
        {
            countItemSlot = maxItemSlot;

            itemGroupPageObj[1].SetActive(false);
            itemGroupPageCheck[1] = false;
        }
        else
        {
            // 아이템 총 갯수가 마지막 그룹 페이지보다 크면 for 문의 마지막 정수를 최대값으로 지정
            countItemSlot = maxItemSlot;
            itemGroupPageObj[1].SetActive(true);
            itemGroupPageCheck[1] = true;
        }

        // 현재 아이템 수량 페이지가 0 이상이면 이전 페이지로 가는 화살표 오브젝트를 활성화
        if (itemGroupPage > 0)
        {
            itemGroupPageObj[0].SetActive(true);
            itemGroupPageCheck[0] = true;
        }
        // 현재 아이템 수량 페이지가 0 이면 이전 페이지로 가는 화살표 오브젝트를 비활성화
        else
        {
            itemGroupPageObj[0].SetActive(false);
            itemGroupPageCheck[0] = false;
        }

        // 현재 아이템 수량 페이지에 최대 출력값 만큼 for 문으로 아이템들을 표시
        for (int i = 0; i < countItemSlot; i++)
        {
            itemSlot[i].GetComponent<SaveItemInfo>().SaveInfo(itemNames[i + checkCount], itemStacks[i + checkCount]);

            // 아이템 아이콘의 이미지를 순차적으로 저장
            if (imageNums[i + checkCount] != 0)
            {
                itemSlotImages[i].sprite = ItemManager.instance.itemImages[imageNums[i + checkCount]].sprite;
            }
            else
            {
                itemSlotImages[i].sprite = ItemManager.instance.itemImages[0].sprite;
            }

            if (page != 0)
            {
                // 아이템의 중첩 수를 순차적으로 저장
                if (itemStacks[i + checkCount] != 0)
                {
                    itemStakcsText[i].text = string.Format("{0}", itemStacks[i + checkCount]);
                }
                else
                {
                    itemStakcsText[i].text = string.Format(" ");
                }
            }
            else
            {
                itemStakcsText[i].text = string.Format(" ");
            }
        }
    }     // ResultInventory()

    #endregion 인벤토리 아이템 창 기능

    #region 인벤토리 상세 정보 창 기능

    // 아이템 상세 정보창으로 들어가는 함수
    public void OnItemDetailInfo()
    {
        if (lookItemInfo == false) { return; }

        lookItemDetailInfo = true;
        playerTf.GetComponent<UIController>().uiController = 3;

        // 새롭게 선택되는 버튼 창 색을 밝게 변경
        newColor = new Color32(255, 255, 255, 255);
        detailSlotColor[detailOrder].color = newColor;
    }     // OnItemDetailInfo()

    // 아이템 상세 정보창에서 나오는 함수
    public void OffItemDetailInfo()
    {
        lookItemDetailInfo = false;
        playerTf.GetComponent<UIController>().uiController = 2;

        // 이전에 선택한 버튼 창 색을 짙은 색으로 변경
        currentColor = new Color32(155, 155, 155, 255);
        detailSlotColor[detailOrder].color = currentColor;

        detailOrder = 0;
    }     // OffItemDetailInfo()

    // 아이템 상세 정보창에서 현재 선택되는 버튼을 지정해주는 함수
    public void ShowDetailOrder()
    {
        // 새롭게 선택되는 아이템 창 색을 밝게 변경
        newColor = new Color32(255, 255, 255, 255);
        detailSlotColor[detailOrder].color = newColor;
    }     // ShowDetailOrder()

    // 아이템 상세 창에서 방향키를 누를 때 마다 버튼 UI 선택 값 변경 함수
    public void DetailOrderCheck(int keyType)
    {
        // 이전에 선택한 아이템 창 색을 짙은 색으로 변경
        currentColor = new Color32(155, 155, 155, 255);
        detailSlotColor[detailOrder].color = currentColor;

        if (keyType == 2)
        {
            // 인벤토리 재료 페이지면 사용 / 장착 버튼, 퀵슬롯 버튼 UI 순서를 스킵
            if (inventoryPage == 2 && detailOrder == 3)
            {
                detailOrder = 0;
            }
            else
            {
                if (detailOrder == 0)
                {
                    detailOrder = 3;
                }
                else
                {
                    detailOrder -= 1;
                }
            }
        }
        else if (keyType == 3)
        {
            // 인벤토리 재료 페이지면 사용 / 장착 버튼, 퀵슬롯 버튼 UI 순서를 스킵
            if (inventoryPage == 2 && detailOrder == 0)
            {
                detailOrder = 3;
            }
            else
            {
                if (detailOrder == 3)
                {
                    detailOrder = 0;
                }
                else
                {
                    detailOrder += 1;
                }
            }
        }
        else if (keyType == 0)
        {
            // 인벤토리 재료 페이지면 사용 / 장착 버튼, 퀵슬롯 버튼 UI 순서를 스킵
            if (inventoryPage == 2 && detailOrder == 3)
            {
                detailOrder = 0;
            }
            else
            {
                if (detailOrder == 0)
                {
                    detailOrder = 3;
                }
                else
                {
                    detailOrder -= 1;
                }
            }
        }
        else if (keyType == 1)
        {
            // 인벤토리 재료 페이지면 사용 / 장착 버튼, 퀵슬롯 버튼 UI 순서를 스킵
            if (inventoryPage == 2 && detailOrder == 0)
            {
                detailOrder = 3;
            }
            else
            {
                if (detailOrder == 3)
                {
                    detailOrder = 0;
                }
                else
                {
                    detailOrder += 1;
                }
            }
        }

        ShowDetailOrder();
    }     // DetailOrderCheck()

    // 아이템 아이콘을 선택할 때 아이템 정보를 출력하는 함수
    public void OnItemInfo(string itemName)
    {
        if (itemSlot[order].GetComponent<SaveItemInfo>().activeSlotCheck == true)
        {
            if (lookItemInfo == false)
            {
                lookItemInfo = true;
                itemInfo.gameObject.SetActive(true);
            }

            // 인벤토리 페이지 값에 따라 버튼을 장착 / 사용 / 빈칸 으로 설정
            // 인벤토리 페이지 값에 따라 퀵슬롯 버튼을 활성, 비활성으로 설정
            if (inventoryPage == 0)
            {
                usingInfoObj.SetActive(true);
                usingInfoText.text = string.Format("장 착");
                quickSlotObj.SetActive(true);
            }
            else if (inventoryPage == 1)
            {
                usingInfoObj.SetActive(true);
                usingInfoText.text = string.Format("사 용");
                quickSlotObj.SetActive(true);
            }
            else
            {
                usingInfoText.text = string.Format(" ");
                usingInfoObj.SetActive(false);
                quickSlotObj.SetActive(false);
            }

            itemNameText.text = string.Format("{0}", itemName);
            ItemManager.instance.ItemImage(itemName, 3, out int largeImageNum);
            largeImage.sprite = ItemManager.instance.itemImages[largeImageNum].sprite;
        }
        else
        {
            if (lookItemInfo == true)
            {
                lookItemInfo = false;
                itemInfo.gameObject.SetActive(false);
            }
        }
    }     // OnItemInfo()

    #endregion 인벤토리 상세 정보 창 기능

    // 인벤토리에 있는 빈 아이콘을 클릭하면 아이템 정보창 초기화 함수
    public void EmptyInfo()
    {
        if (lookItemInfo == true)
        {
            lookItemInfo = false;
            itemInfoTextUI.text = string.Format(" ");
            itemInfo.gameObject.SetActive(false);
        }
    }     // EmptyInfo()
}