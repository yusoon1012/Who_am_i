using System.Collections;
using System.Collections.Generic;
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
    // 도구 페이지 장착 중 텍스트 목록
    public Text[] usingText = new Text[30];
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
    // 퀵슬롯 UI 트랜스폼
    public Transform quickSlotTf;
    // 아이템 버리기 안내 창 오브젝트
    public GameObject dropItemInfo;
    // 아이템 버리기 선택 버튼 UI
    public Image[] dropItemInfoImage = new Image[2];

    // 인벤토리를 열고있는 상태인지 체크
    public bool lookInventory { get; set; } = false;
    // 인벤토리 상세 정보에 들어와 있는 상태인지 체크
    public bool lookItemDetailInfo { get; set; } = false;
    // Test : 인벤토리의 1개의 아이템 그룹 페이지당 아이템 아이콘 갯수
    public int maxItemSlot { get; set; } = default;
    // 도구 페이지의 장착 중 텍스트가 표시된 아이템의 이름
    public string useEquipStr { get; set; } = default;

    // 인벤토리 창에서 현재 아이템 선택 색
    private Color currentColor = default;
    // 인벤토리 창에서 새로운 아이템 선택 색
    private Color newColor = default;

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
    // 0. 도구 페이지
    // 1. 음식 페이지
    // 2. 재료 페이지
    private int inventoryPage = default;
    // 인벤토리 아이템 그룹 페이지 현재 값
    private int itemGroupPage = default;
    // 아이템을 클릭해 설명칸이 출력된 상태인지 체크
    private bool lookItemInfo = false;
    // 아이템 목록에서의 현재 아이템 선택 확인 값
    private int order = default;
    // 아이템 상세 목록에서의 현재 선택한 버튼 확인 값
    private int detailOrder = default;
    // 아이템 버리기 UI 에서 현재 표시하고 있는 버튼 값 체크
    private int dropItemOrder = default;
    // 인벤토리에 아이템들을 표시할 때 몇칸까지 표시할지 체크
    private int countItemSlot = default;
    // 도구 페이지의 장착 중 텍스트의 표시된 위치
    private int useEquipNum = default;
    // 도구 페이지의 장착 중 텍스트가 표시된 상태인지 체크
    private bool useEquipCheck = false;
    // 현재 출력된 인벤토리가 몇번째 페이지인지 중첩 슬롯을 확인하는 값
    private int checkCount = default;
    // 이전, 다음 아이템 수량 페이지가 존재 하는지 체크
    private bool[] itemGroupPageCheck = new bool[2];
    // 인벤토리 상세 정보창에 들어간 상태인지 체크
    private bool itemInfoCheck = false;

    // 도구, 음식 아이템을 장착, 사용 또는 해제하는 스크립트
    VRIFItemSystem equipmentScript = new VRIFItemSystem();
    VRIFStatusSystem playerStatusScript = new VRIFStatusSystem();

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
        checkCount = 0;
        useEquipNum = 0;
        dropItemOrder = 0;

        maxItemSlot = 24;
        useEquipStr = "None";
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

    #region 아이템 획득 기능

    // 아이템 매니저의 인벤토리에 아이템이 없을 때 새로운 아이템을 추가하고, 아이템이 있을 시 갯수를 중첩시키는 함수
    public void AddInventory(string itemName, int num)
    {
        ItemsMain itemInfo = new ItemsMain();

        ItemManager.instance.InventoryAdd(itemName, num, out itemInfo);

        if (lookInventory == true)
        {
            CleanInventory();

            ControlInventory();
        }
    }     // AddInventory()

    #endregion 아이템 획득 기능

    //* Feat : 아이템 파이어 데이터 베이스에서 아이템 모두 가져오기 기능 추가 예정
    //public void Test()
    //{
    //    test = ItemManager.instance.testDic;
    //}

    //public void ReadTest()
    //{
    //    foreach (KeyValuePair<string, int> pair in test)
    //    {
    //        string name = pair.Key;
    //        int stack = pair.Value;

    //        AddInventory(name, stack);
    //    }
    //}

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

    // 아이템 타입 페이지를 변경시켜주는 함수를 연결시켜주는 함수
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
    }     // ChangePage()

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

    // 아이템 버리기 안내창에서 커서를 움직이는 함수
    public void DropItemOrderCheck(int arrowType)
    {
        currentColor = new Color32(155, 155, 155, 255);
        dropItemInfoImage[dropItemOrder].color = currentColor;

        if (arrowType == 0 || arrowType == 2)
        {
            if (dropItemOrder == 0)
            {
                dropItemOrder = 1;
            }
            else if (dropItemOrder == 1)
            {
                dropItemOrder = 0;
            }
        }
        else if (arrowType == 1 || arrowType == 3)
        {
            if (dropItemOrder == 0)
            {
                dropItemOrder = 1;
            }
            else if (dropItemOrder == 1)
            {
                dropItemOrder = 0;
            }
        }

        newColor = new Color32(255, 255, 255, 255);
        dropItemInfoImage[dropItemOrder].color = newColor;
    }     // DropItemOrderCheck()

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

        // 현재 <장착중> 표시가 인벤토리에 표시 되어있을 경우에는 해당 표시를 지워줌
        if (useEquipCheck == true)
        {
            useEquipCheck = false;
            usingText[useEquipNum].gameObject.SetActive(false);
            useEquipNum = 0;
        }

        for (int i = 0; i < itemCount; i++)
        {
            itemSlot[i].GetComponent<SaveItemInfo>().CleanSlot();
            itemSlotImages[i].sprite = ItemManager.instance.itemImages[0].sprite;
            itemStakcsText[i].text = string.Format(" ");
            itemStacks[i] = 0;
        }

        itemCount = 0;
        checkCount = 0;
        countItemSlot = 0;
        itemGroupCount = 0;
    }     // CleanInventory()

    // 인벤토리를 종료할 때 비활성화 하는 함수
    public void ExitInventory()
    {
        if (lookItemInfo == true)
        {
            itemInfo.SetActive(false);
            largeImage.gameObject.SetActive(true);
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

        checkCount = 0;
        countItemSlot = 0;

        for (int j = 0; j < itemCount; j++)
        {
            if (itemNames[j] == null)
            {
                itemCount -= 1;
            }
        }

        // 몇번째 아이템 그룹 페이지인지 체크하고 인벤토리 배열에 아이템 그룹 페이지만큼의 아이템을 출력
        checkCount = itemGroupPage * maxItemSlot;

        // 마지작 아이템 그룹 페이지를 체크하고 for 문의 마지막 정수를 표시
        int maxCheckCount = (itemGroupPage + 1) * maxItemSlot;

        // 인벤토리의 총 아이템 갯수가 마지막 아이템 그룹 페이지보다 작거나 같은지 체크
        if (itemCount < maxCheckCount)
        {
            countItemSlot = itemCount;

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
            if (itemNames[i + checkCount] == null) { continue; }

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

            // 인벤토리 현재 페이지의 아이템들을 출력할 때 페이지가 도구 페이지이고, 현재 장착 상태인 아이템이 있을때 실행
            if (inventoryPage == 0 && useEquipStr != "None")
            {
                // 해당 인벤토리 칸의 아이템이 장착 중인 아이템과 같을때 <장착중> 글을 표시
                if (itemNames[i + checkCount] == useEquipStr)
                {
                    useEquipCheck = true;
                    useEquipNum = i;
                    usingText[i].gameObject.SetActive(true);
                }
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
        if (itemInfoCheck == true)
        {
            itemInfoCheck = false;
            itemInfoTextUI.gameObject.SetActive(false);
            largeImage.gameObject.SetActive(true);
        }

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
            if (inventoryPage == 1)
            {
                usingInfoObj.SetActive(true);
                usingInfoText.text = string.Format("사 용");
                quickSlotObj.SetActive(true);
            }
            else if (inventoryPage == 0)
            {
                // 아이템을 선택할 때 해당 아이템이 현재 장착중인 아이템일 경우에는 장착 해제를 표시
                if (useEquipCheck == true && itemNames[order + checkCount] == useEquipStr)
                {
                    usingInfoObj.SetActive(true);
                    usingInfoText.text = string.Format("장착 해제");
                    quickSlotObj.SetActive(true);
                }
                // 아이템을 선택할 때 해당 아이템이 장착중인 아이템이 아니면 장착을 표시
                else
                {
                    usingInfoObj.SetActive(true);
                    usingInfoText.text = string.Format("장 착");
                    quickSlotObj.SetActive(true);
                }
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
                itemInfo.SetActive(false);
            }
        }
    }     // OnItemInfo()

    // 아이템 상세 정보창에서 각각의 메뉴를 실행하는 함수
    public void SelectInfo()
    {
        // 아이템 큰 이미지를 누르면 아이템 정보가 표시됨 (반대도 가능)
        if (detailOrder == 0) { ChangeItemInfo(); }
        // 아이템 장착, 사용, 장착 해제를 누르면 해당 기능의 함수 실행
        else if (detailOrder == 1) { UseItem(0, itemNames[order + checkCount]); }
        // 퀵슬롯 버튼을 누르면 실행
        else if (detailOrder == 2)
        {
            quickSlotTf.GetComponent<QuickSlot>().TakeItemName(itemNames[order + checkCount], inventoryPage);
            playerTf.GetComponent<UIController>().uiController = 6;
            ConnectQuickSlot();
        }
        // 아이템 버리기 버튼을 누르면 실행
        else if (detailOrder == 3)
        {
            playerTf.GetComponent<UIController>().uiController = 7;
            dropItemInfo.SetActive(true);
            dropItemOrder = 1;
            newColor = new Color32(255, 255, 255, 255);
            dropItemInfoImage[dropItemOrder].color = newColor;
        }
    }     // SelectInfo()

    // 아이템 버리기 안내창에서 확인을 눌렀을 때 기능 함수
    public void FunctionDropItem()
    {
        // 아이템 버리기 안내창에서 확인을 눌렀을 때
        if (dropItemOrder == 0)
        {
            // 아이템 매니저의 아이템 삭제 함수를 실행
            ItemManager.instance.RemoveItem(itemNames[order + checkCount], inventoryPage, out string dropItemName);
            playerTf.GetComponent<UIController>().uiController = 2;
            Debug.LogFormat("{0} 아이템을 버렸습니다.", dropItemName);

            // 인벤토리 UI 를 초기화 하고 다시 아이템 정렬
            ExitDropItem(0);
            OffItemDetailInfo();
            CleanInventory();
            ControlInventory();
        }
        // 아이템 버리기 안내창에서 취소를 눌렀을 때
        else if (dropItemOrder == 1)
        {
            ExitDropItem(1);
        }
    }     // FunctionDropItem()

    // 아이템 버리기 안내창에서 취소를 눌렀을 때 기능 함수
    public void ExitDropItem(int exitType)
    {
        currentColor = new Color32(155, 155, 155, 255);
        dropItemInfoImage[dropItemOrder].color = currentColor;
        dropItemOrder = 1;
        dropItemInfo.SetActive(false);
        
        // 아이템 버리기 안내창에서 확인 키를 누르고 UI 가 비활성화 되지않고 취소를 누르고 UI 가 비활성화 될 때
        // 아이템을 버리고 난 뒤에는 아이템 상세 창이 비활성화 됨
        if (exitType == 1)
        {
            playerTf.GetComponent<UIController>().uiController = 3;
        }
    }     // ExitDropItem()

    // 인벤토리 UI 에서 퀵슬롯 UI 로 이동하는 함수
    public void ConnectQuickSlot()
    {
        inventory.SetActive(false);
        itemInfo.SetActive(false);
        playerTf.GetComponent<MainMenu>().mainMenu.SetActive(false);
        quickSlotTf.GetComponent<QuickSlot>().quickSlotObj.SetActive(true);
    }     // ConnectQuickSlot()

    // 아이템 장착, 사용, 장착 해제 기능 함수
    public void UseItem(int usingType, string itemName)
    {
        // 인벤토리에서 아이템을 사용, 장착, 장착해제 기능을 사용하였는지, 퀵슬롯에서 사용하였는지 체크하는 값
        switch (usingType)
        {
            // 인벤토리에서 아이템에 대한 사용, 장착, 장착 해제 버튼을 눌렀을 경우
            case 0:
                // 도구 아이템 타입일 경우
                if (inventoryPage == 0)
                {
                    // 현재 장착 아이템이 없으면 장착을 누른 아이템을 장착
                    if (useEquipStr == "None")
                    {
                        // 장착한 아이템 이름을 저장함
                        useEquipStr = itemName;

                        CleanInventory();
                        ControlInventory();
                        OnItemInfo(itemName);

                        // 아이템 영문 이름으로 변환하는 함수를 실행
                        ConversionEquipmentName(useEquipStr, 0);
                    }
                    // 현재 장착중인 다른 아이템이 있으면 장착을 누른 아이템으로 장착 교체
                    else if (useEquipStr != "None" && itemName != useEquipStr)
                    {
                        // 장착한 아이템 이름을 저장함
                        useEquipStr = itemName;

                        CleanInventory();
                        ControlInventory();
                        OnItemInfo(itemName);

                        // 아이템 영문 이름으로 변환하는 함수를 실행
                        ConversionEquipmentName(useEquipStr, 0);
                    }
                    // 현재 장착중인 아이템을 장착 해제를 누르면 장착 해제
                    else if (useEquipStr != "None" && itemName == useEquipStr)
                    {
                        // 아이템 영문 이름으로 변환하는 함수를 실행
                        ConversionEquipmentName(useEquipStr, 1);

                        // 장착한 아이템 이름을 None 으로 저장함
                        useEquipStr = "None";

                        CleanInventory();
                        ControlInventory();
                        OnItemInfo(itemName);
                    }
                }
                // 음식 아이템 타입일 경우
                else if (inventoryPage == 1)
                {
                    // 아이템을 사용하는 기능의 함수로 실행 타입과 사용하는 아이템 이름값으로 실행함
                    UseFoods(0, itemName);
                }
                break;
            // 퀵슬롯에서 아이템에 대한 사용, 장착, 장착 해제 버튼을 눌렀을 경우
            case 1:
                if (useEquipStr == "None")
                {
                    useEquipStr = itemName;

                    // 아이템 영문 이름으로 변환하는 함수를 실행
                    ConversionEquipmentName(useEquipStr, 0);
                }
                else if (useEquipStr == itemName)
                {
                    // 아이템 영문 이름으로 변환하는 함수를 실행
                    ConversionEquipmentName(useEquipStr, 1);

                    useEquipStr = "None";
                }
                else if (useEquipStr != "None" && useEquipStr != itemName)
                {
                    useEquipStr = itemName;

                    // 아이템 영문 이름으로 변환하는 함수를 실행
                    ConversionEquipmentName(useEquipStr, 0);
                }
                break;
            default:
                break;
        }
    }     // UseItem()

    // 음식 아이템을 사용하는 기능의 함수
    public void UseFoods(int usingType, string itemName)
    {
        ItemsMain itemInfomation = new ItemsMain();

        // 인벤토리에서 아이템을 사용하였는지, 퀵슬롯에서 아이템을 사용하였는지 구분함
        switch (usingType)
        {
            // 인벤토리에서 아이템을 사용했을 경우
            case 0:
                // 아이템 매니저에서 아이템 저장 정보를 가져옴
                ItemManager.instance.ReturnItemInfomation(itemName, 1, out itemInfomation);
                // 아이템 중첩 수를 감소
                itemInfomation.itemStack -= 1;

                // 아이템 중첩 수가 0 이하일 경우
                if (itemInfomation.itemStack <= 0)
                {
                    // 퀵슬롯에 저장된 아이템과 중첩값을 동기화
                    quickSlotTf.GetComponent<QuickSlot>().UseFoodsCheck(itemName);
                    // 아이템 매니저에서 인벤토리에 저장된 아이템을 삭제하는 함수를 실행
                    ItemManager.instance.DeleteItem(itemName);

                    OffItemDetailInfo();
                    CleanInventory();
                    ControlInventory();
                }
                else
                {
                    CleanInventory();
                    ControlInventory();
                }
                break;
            case 1:
                // 아이템 매니저에서 아이템 저장 정보를 가져옴
                ItemManager.instance.ReturnItemInfomation(itemName, 1, out itemInfomation);
                // 아이템 중첩 수를 감소
                itemInfomation.itemStack -= 1;

                // 아이템 중첩 수가 0 이하일 경우
                if (itemInfomation.itemStack <= 0)
                {
                    // 퀵슬롯에 저장된 아이템과 중첩값을 동기화
                    quickSlotTf.GetComponent<QuickSlot>().UseFoodsCheck(itemName);
                    // 아이템 매니저에서 인벤토리에 저장된 아이템을 삭제하는 함수를 실행
                    ItemManager.instance.DeleteItem(itemName);
                }
                
                break;
            default:
                break;
        }

        // 가져온 음식 정보 메서드값이 null 이 아닐 경우 실행
        if (itemInfomation != null)
        {
            //* Feat : 음식 아이템을 사용한 후 효과 타입을 체크하고, 음식 효과를 VR 플레이어 스크립트 함수에 보내는 기능 추가 예정
            // 사용한 음식의 효과 타입
            int foodEffectType = itemInfomation.cookType;
            // 사용한 음식의 포만감 게이지 증가량
            int foodSatietyGauge = itemInfomation.satietyGauge;
            // 사용한 음식의 응가 게이지 증가량
            int foodPooGauge = itemInfomation.pooGauge;

            switch (foodEffectType)
            {
                // 음식 효과 타입이 1 이면
                case 1:
                    playerStatusScript.GetFood(foodSatietyGauge, foodPooGauge);
                    break;
                // 음식 효과 타입이 2 이면
                case 2:
                    //* 음식 효과 타입이 2 면 기능 추가 예정
                    break;
                default:
                    break;
            }
        }
    }     // UseFoods()

    // 도구 장착, 음식 사용 아이템 이름을 영문 이름으로 변환해서 장착 정보를 보내는 함수
    public void ConversionEquipmentName(string itemName, int itemType)
    {
        ItemsMain itemInfomation = new ItemsMain();

        // 아이템 매니저에서 해당 아이템의 정보를 가져옴
        ItemManager.instance.ReturnItemInfomation(itemName, 0, out itemInfomation);

        // 아이템 타입이 도구 타입이면
        if (itemType == 0)
        {
            // 도구 장착 기능의 스크립트로 영문 아이템 이름으로 정보를 보냄
            //equipmentScript.MountingItem(itemInfomation.itemEnglishName);
        }
        // 아이템 타입이 도구이고 도구를 해제하는 상태면
        else if (itemType == 1)
        {
            //equipmentScript.ReleaseItem(itemInfomation.itemEnglishName);
        }
    }     // ConversionEquipmentName()

    // 아이템 큰 이미지를 누르면 아이템 정보가 표시되는 함수 (반대도 가능)
    public void ChangeItemInfo()
    {
        // 아이템 큰 이미지가 표시된 상태
        if (itemInfoCheck == false)
        {
            // 아이템 큰 이미지를 비활성화하고 아이템 정보를 표시
            itemInfoCheck = true;
            largeImage.gameObject.SetActive(false);
            itemInfoTextUI.gameObject.SetActive(true);

            ItemsMain items = new ItemsMain();

            // 아이템 매니저에서 아이템 정보를 가져와 아이템 정보를 출력
            ItemManager.instance.DictionaryItemImage(itemNames[order + checkCount], out items);

            itemInfoTextUI.text = string.Format("{0}", items.itemInfo);
        }
        // 아이템 정보가 표시된 상태
        else
        {
            // 아이템 정보를 비활성화하고 큰 이미지를 표시
            itemInfoCheck = false;
            itemInfoTextUI.gameObject.SetActive(false);
            largeImage.gameObject.SetActive(true);
        }
    }     // ChangeItemInfo()

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