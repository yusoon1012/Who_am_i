using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCrafting : MonoBehaviour
{
    #region 변수 설정

    // 메뉴 UI 의 백그라운드 이미지
    public GameObject mainScreen;
    // 제작 창 오브젝트
    public GameObject crafting;
    // 제작 창의 제작 가능한 슬롯 목록
    public GameObject[] craftingSlot = new GameObject[5];
    // 제작 가능한 슬롯의 아이템 이미지 값
    public Image[] itemImages = new Image[5];
    // 제작 가능한 슬롯의 아이템 이름 값
    public Text[] itemTexts = new Text[5];
    // 제작 상세 창의 제작 아이템 이미지 목록
    public Image[] craftingImages = new Image[3];
    // 제작 상세 창의 제작 아이템 갯수 확인 이미지 목록
    public Text[] craftingStacks = new Text[3];
    // 제작 상세 창 오브젝트
    public GameObject craftingInfoObj;
    // 제작 창의 페이지 이미지
    public Image[] pageImages = new Image[2];
    // 제작 상세 정보에서 선택한 아이템 이름 텍스트
    public Text itemNameText;
    // 제작 리스트 목록 이미지 오브젝트
    public GameObject[] craftingSlotObj = new GameObject[5];
    // 제작 상세 창의 버튼 오브젝트
    public GameObject[] detailSlotObj = new GameObject[4];

    // 제작 창을 보고있는 상태인지 체크
    public bool lookCrafting { get; set; } = false;
    // 제작 상세 창을 보고있는 상태인지 체크
    public bool lookCraftingInfo { get; set; } = false;

    // 제작 창에서 현재 리스트 선택 색
    private Color currentColor = default;
    // 제작 창에서 새로운 리스트 선택 색
    private Color newColor = default;
    // 제작 창에서 리스트 선택 색 변경 이미지
    private Image[] craftingSlotColor = new Image[5];
    // 제작 상세 정보에서의 버튼 색 변경 이미지
    private Image[] detailSlotColor = new Image[4];

    // 제작되는 아이템의 재료 아이템의 종류 수
    private int craftingLength = default;
    // 제작되는 아이템의 이름
    private string craftingName = default;
    // 제작되는 아이템의 아이콘 이미지 값
    private int craftingImageNum = default;
    // 제작되는 아이템의 설명 텍스트
    private string craftingInfo = default;
    // 제작되는 아이템의 타입
    private int completeItemType = default;
    // 재료 아이템들의 이름
    private string[] stuffName = new string[5];
    // 재료 아이템들의 필요 중첩 수
    private int[] stuffStack = new int[5];
    // 재료 아이템들의 현재 소지중인 중첩 수
    private int[] stuffNowStack = new int[5];
    // 재료 아이템들의 아이콘 이미지 값
    private int[] stuffImageNum = new int[5];
    // 재료 아이템들의 중첩 수 조건 최종 체크
    private int resultStuffCount = default;
    // 재료 아이템들의 타입
    private int[] stuffItemTypes = new int[5];
    // 제작 창의 페이지 값
    private int page = default;
    // 제작 목록에서의 현재 리스트 선택 확인 값
    private int order = default;
    // 제작 상세 목록에서의 현재 선택한 버튼 확인 값
    private int detailOrder = default;
    // 제작 가능 목록의 아이템 이름 값
    private string[] craftingStr = new string[5];
    // 제작 가능 목록의 아이템 이미지 값
    private int[] craftingSlotImageNum = new int[5];
    // 제작 상세 창의 제작 후 완성되는 아이템의 제작 완료 갯수
    private int craftingStack = default;
    // 제작을 진행할 때 만들어지는 아이템 수량 계산값
    private int craftingStackNum = default;
    // 제작을 진행할 때 필요한 재료 아이템 수량 계산값
    private int[] stuffStackNum = new int[5];
    // 제작을 진행할 때 제작 조건을 모두 완료 상태인지 체크
    private bool resultStuffCheck = false;

    #endregion 변수 설정

    void Awake()
    {
        craftingLength = 0;
        resultStuffCount = 0;
        completeItemType = 0;
        page = 0;
        craftingStack = 0;
        order = 0;
        detailOrder = 0;
        craftingStackNum = 0;
    }     // Awake()

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            craftingSlotColor[i] = craftingSlotObj[i].GetComponent<Image>();
        }

        for (int j = 0; j < 4; j++)
        {
            detailSlotColor[j] = detailSlotObj[j].GetComponent<Image>();
        }
    }     // Start()

    #region 페이지 변경 기능

    // 우측 타입의 방향키 입력 시 키 값 별로 전달하는 함수
    public void ControlDetailOrder(int arrowType)
    {
        if (arrowType == 2 || arrowType == 3)
        {
            PageChange();
        }
    }     // ControlDetailOrder()

    // 제작 타입 페이지 변경 키 누를 시 페이지 변경 함수
    public void PageChange()
    {
        // 크래프팅 페이지 변경
        if (page == 0)
        {
            page = 1;
        }
        else
        {
            page = 0;
        }

        // 페이지 버튼 알파값 변경
        for (int i = 0; i < 2; i++)
        {
            Color color = pageImages[i].color;

            if (page == i)
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

        CheckPage();
    }     // PageChange()

    // 제작 페이지 초기값을 설정
    public void CheckPage()
    {
        CleanSlot();

        // 크래프팅에 현재 선택되어 있는 슬롯의 색을 밝게 변경
        newColor = new Color32(255, 255, 255, 255);
        craftingSlotColor[order].color = newColor;

        if (page == 0)
        {
            craftingStr[0] = "딸기 우유";

            // Fix : 테스트 이후 5개 슬롯을 for 문을 사용하여 돌릴 예정
            for (int i = 0; i < 1; i++)
            {
                ItemManager.instance.ItemImage(craftingStr[i], 3, out craftingSlotImageNum[i]);
                itemImages[i].sprite = ItemManager.instance.itemImages[craftingSlotImageNum[i]].sprite;
                itemTexts[i].text = string.Format("{0}", craftingStr[i]);
                craftingSlot[i].GetComponent<SaveCraftingInfo>().SaveInfo(craftingStr[i]);
            }

            //* Fix : 테스트 후 완성본
            //for (int i = 0; i < 5; i++)
            //{
            //    itemTexts[i].text = string.Format("{0}", craftingStr[i]);
            //    craftingSlot[i].GetComponent<SaveCraftingInfo>().SaveInfo(craftingStr[i]);
            //}
        }

        ShowOrder();
    }     // CheckPage()

    // 현재 선택중인 제작 리스트의 정보를 출력하는 함수
    public void ShowOrder()
    {
        craftingSlot[order].GetComponent<SaveCraftingInfo>().LoadInfo(out string craftingName);

        CraftingCheck(craftingName);
    }     // ShowOrder()

    #endregion 페이지 변경 기능

    #region 크래프팅 목록 기능

    // 제작 가능 리스트의 모든 값들을 초기화하는 함수
    public void CleanSlot()
    {
        for (int i = 0; i < 5; i++)
        {
            craftingSlot[i].GetComponent<SaveCraftingInfo>().activeSlotCheck = false;
            itemImages[i].sprite = ItemManager.instance.itemImages[0].sprite;
            itemTexts[i].text = string.Format(" ");
        }
    }     // CleanSlot()

    // 제작 창 열기
    public void OnCrafting()
    {
        if (lookCrafting == false)
        {
            mainScreen.SetActive(true);
            crafting.SetActive(true);
        }

        lookCrafting = true;

        CheckPage();
    }     // OnCrafting()

    // 제작 창 닫기
    public void ExitCrafting()
    {
        lookCrafting = false;
        detailOrder = 0;

        CleanSlot();

        craftingInfoObj.SetActive(false);
        crafting.SetActive(false);
        mainScreen.SetActive(false);
    }     // ExitCrafting()

    // 제작 목록에서의 커서 변경 함수
    public void OrderCheck(int keyType)
    {
        // 크래프팅에 이전에 선택되어 있던 슬롯의 색을 어둡게 변경
        currentColor = new Color32(155, 155, 155, 255);
        craftingSlotColor[order].color = currentColor;

        if (keyType == 0)
        {
            if (order == 0)
            {
                order = 4;
            }
            else
            {
                order -= 1;
            }
        }
        else if (keyType == 1)
        {
            if (order == 4)
            {
                order = 0;
            }
            else
            {
                order += 1;
            }
        }
        else if (keyType == 2)
        {
            if (order == 0)
            {
                order = 4;
            }
            else
            {
                order -= 1;
            }
        }
        else if (keyType == 3)
        {
            if (order == 4)
            {
                order = 0;
            }
            else
            {
                order += 1;
            }
        }

        // 크래프팅에 현재 선택되어 있는 슬롯의 색을 밝게 변경
        newColor = new Color32(255, 255, 255, 255);
        craftingSlotColor[order].color = newColor;

        ShowOrder();
    }     // OrderCheck()

    #endregion 크래프팅 목록 기능

    #region 크래프팅 상세 정보 기능

    // 제작 상세 창에서 선택중인 기능 버튼을 확인하는 함수
    public void DetailOrderCheck(int keyType)
    {
        if (keyType == 0)
        {
            // 제작 상세 창에서 이전에 선택되어 있던 버튼의 색을 어둡게 변경
            currentColor = new Color32(155, 155, 155, 255);
            detailSlotColor[detailOrder].color = currentColor;

            if (detailOrder == 0)
            {
                detailOrder = 3;
            }
            else
            {
                detailOrder -= 1;
            }

            // 제작 상세창에서 UI 를 순서대로 선택하는 함수 실행
            DetailOrderUpDown();
        }
        else if (keyType == 1)
        {
            // 제작 상세 창에서 이전에 선택되어 있던 버튼의 색을 어둡게 변경
            currentColor = new Color32(155, 155, 155, 255);
            detailSlotColor[detailOrder].color = currentColor;

            if (detailOrder == 3)
            {
                detailOrder = 0;
            }
            else
            {
                detailOrder += 1;
            }

            // 제작 상세창에서 UI 를 순서대로 선택하는 함수 실행
            DetailOrderUpDown();
        }
        else if (keyType == 2 && detailOrder != 2)
        {
            // 제작 상세 창에서 이전에 선택되어 있던 버튼의 색을 어둡게 변경
            currentColor = new Color32(155, 155, 155, 255);
            detailSlotColor[detailOrder].color = currentColor;

            if (detailOrder == 0)
            {
                detailOrder = 3;
            }
            else
            {
                detailOrder -= 1;
            }

            // 제작 상세창에서 UI 를 순서대로 선택하는 함수 실행
            DetailOrderUpDown();
        }
        else if (keyType == 2 && detailOrder == 2)
        {
            // 증가되는 재료의 갯수를 체크하는 함수 실행
            if (stuffStackNum[0] > 1 && stuffStackNum[1] > 1)
            {
                stuffStackNum[0] -= stuffStack[0];
                stuffStackNum[1] -= stuffStack[1];
                craftingStackNum -= craftingStack;
            }

            // 변경되는 제작 갯수를 표시해주는 함수 실행
            DetailOrderLeftRight();
        }
        else if (keyType == 3 && detailOrder != 2)
        {
            // 제작 상세 창에서 이전에 선택되어 있던 버튼의 색을 어둡게 변경
            currentColor = new Color32(155, 155, 155, 255);
            detailSlotColor[detailOrder].color = currentColor;

            if (detailOrder == 3)
            {
                detailOrder = 0;
            }
            else
            {
                detailOrder += 1;
            }

            // 제작 상세창에서 UI 를 순서대로 선택하는 함수 실행
            DetailOrderUpDown();
        }
        else if (keyType == 3 && detailOrder == 2)
        {
            // 증가되는 재료의 갯수를 체크하는 함수 실행
            StuffStackCheck(out bool stuffStackCheck);

            if (stuffStackCheck == true)
            {
                stuffStackNum[0] += stuffStack[0];
                stuffStackNum[1] += stuffStack[1];
                craftingStackNum += craftingStack;
            }

            // 변경되는 제작 갯수를 표시해주는 함수 실행
            DetailOrderLeftRight();
        }
    }     // DetailOrderCheck()

    // 제작 상세 창에서 상세 UI 를 순서에 따라 선택하는 함수
    private void DetailOrderUpDown()
    {
        // 상단에 아이템 이름 정보값을 출력
        if (detailOrder == 0)
        {
            itemNameText.text = string.Format("{0}", stuffName[0]);
        }
        else if (detailOrder == 1)
        {
            itemNameText.text = string.Format("{0}", stuffName[1]);
        }
        else if (detailOrder == 2)
        {
            itemNameText.text = string.Format("{0}", craftingName);
        }
        else
        {
            itemNameText.text = string.Format(" ");
        }

        // 제작 상세 창에서 새롭게 선택된 버튼의 색을 밝게 변경
        newColor = new Color32(255, 255, 255, 255);
        detailSlotColor[detailOrder].color = newColor;
    }     // DetailOrderUpDown()

    // 제작 상세 창에서 제작할 갯수를 변경하는 함수
    private void DetailOrderLeftRight()
    {
        resultStuffCount = 0;
        resultStuffCheck = false;

        // 제작 갯수를 키 입력값에 따라 변화시킴
        for (int j = 0; j < craftingLength + 1; j++)
        {
            if (j != 2)
            {
                // 0, 1 번째 순서에는 재료 아이템 정보
                craftingStacks[j].text = string.Format("{0} / {1}", stuffStackNum[j], stuffNowStack[j]);
            }
            else
            {
                // 2 번째 순서에는 제작 아이템 정보
                craftingStacks[j].text = string.Format("{0}", craftingStackNum);
            }
        }

        // 제작 가능한 수량을 체크
        for (int i = 0; i < craftingLength; i++)
        {
            if (stuffNowStack[i] >= stuffStackNum[i])
            {
                resultStuffCount += 1;
            }

            if (resultStuffCount == craftingLength)
            {
                resultStuffCheck = true;
            }
            else
            {
                resultStuffCheck = false;
            }
        }
    }     // DetailOrderLeftRight()

    // 제작 갯수를 증가시킬 때 현재 소지중인 재료의 갯수보다 같거나 작은지 체크하는 함수
    private bool StuffStackCheck(out bool stuffStackCheck)
    {
        int[] stuffCheck = new int[2];

        // 재료 갯수를 미리 증가시킴
        stuffCheck[0] = stuffStackNum[0] + stuffStack[0];
        stuffCheck[1] = stuffStackNum[1] + stuffStack[1];

        // 증가된 재료 갯수가 현재 소지중인 재료의 갯수보다 같거나 작은지 체크
        if (stuffCheck[0] <= stuffNowStack[0] && stuffCheck[1] <= stuffNowStack[1])
        {
            // 현재 소지중인 재료보다 같거나 작다 (true)
            stuffStackCheck = true;
        }
        else
        {
            // 현재 소지중인 재료보다 크다 (false)
            stuffStackCheck = false;
        }

        return stuffStackCheck;
    }     // StuffStackCheck()

    // 크래프팅 상세 정보창 진입 함수
    public void OnDetailCrafting()
    {
        if (craftingSlot[order].GetComponent<SaveCraftingInfo>().activeSlotCheck == true)
        {
            lookCraftingInfo = true;

            // 크래프팅에 현재 선택되어 있는 슬롯의 색을 밝게 변경
            newColor = new Color32(255, 255, 255, 255);
            detailSlotColor[detailOrder].color = newColor;

            itemNameText.text = string.Format("{0}", stuffName[0]);
        }
    }     // OnDetailCrafting()

    // 크래프팅 상세 정보창 나가기 함수
    public void ExitDetailCrafting()
    {
        // 크래프팅에 이전에 선택되어 있던 슬롯의 색을 어둡게 변경
        currentColor = new Color32(155, 155, 155, 255);
        detailSlotColor[detailOrder].color = currentColor;

        itemNameText.text = string.Format(" ");
        detailOrder = 0;
        lookCraftingInfo = false;
    }     // ExitDetailCrafting()

    // 제작 가능한 리스트를 선택했을 때 상세 제작 정보 출력 함수
    public void CraftingCheck(string resultType)
    {
        stuffStackNum[0] = 0;
        stuffStackNum[1] = 0;
        resultStuffCount = 0;
        craftingLength = 0;
        craftingName = resultType;
        completeItemType = 0;
        resultStuffCheck = false;

        // 제작 리스트가 존재할 경우에만 제작 상세 창을 연다
        if (craftingSlot[order].GetComponent<SaveCraftingInfo>().activeSlotCheck == false)
        {
            craftingInfoObj.SetActive(false);

            return;
        }
        else
        {
            craftingInfoObj.SetActive(true);
        }

        //* 제작 될 최종 아이템 정보를 불러옴
        ItemManager.instance.ItemTypeCheck(craftingName, out completeItemType);
        ItemManager.instance.ItemImage(craftingName, 3, out craftingImageNum);
        ItemManager.instance.CraftingLength(craftingName, out craftingLength);
        ItemManager.instance.LoadItemInfoText(craftingName, 3, out craftingInfo);
        ItemManager.instance.CraftingStack(craftingName, out craftingStack);
        craftingStackNum = craftingStack;
        //* 제작 될 최종 아이템 정보를 불러옴

        //* 제작에 필요한 재료들의 정보를 순차적으로 불러옴
        for (int i = 0; i < craftingLength; i++)
        {
            ItemManager.instance.CraftingStuffName(craftingName, i, out stuffName[i]);
            ItemManager.instance.ItemTypeCheck(stuffName[i], out stuffItemTypes[i]);
            ItemManager.instance.CraftingStuffStack(craftingName, i, out stuffStack[i]);
            stuffStackNum[i] = stuffStack[i];
            ItemManager.instance.InventoryStack(stuffName[i], stuffItemTypes[i], out stuffNowStack[i]);
            ItemManager.instance.ItemImage(stuffName[i], 3, out stuffImageNum[i]);
        }
        //* 제작에 필요한 재료들의 정보를 순차적으로 불러옴

        for (int j = 0; j < craftingLength + 1; j++)
        {
            if (j != 2)
            {
                // 0, 1 번째 순서에는 재료 아이템 정보
                craftingImages[j].sprite = ItemManager.instance.itemImages[stuffImageNum[j]].sprite;
                craftingStacks[j].text = string.Format("{0} / {1}", stuffStackNum[j], stuffNowStack[j]);
            }
            else
            {
                // 2 번째 순서에는 제작 아이템 정보
                craftingImages[j].sprite = ItemManager.instance.itemImages[craftingImageNum].sprite;
                craftingStacks[j].text = string.Format("{0}", craftingStack);
            }
        }

        // 제작 가능한 수량을 체크
        for (int y = 0; y < craftingLength; y++)
        {
            if (stuffNowStack[y] >= stuffStackNum[y])
            {
                resultStuffCount += 1;
            }

            if (resultStuffCount == craftingLength)
            {
                resultStuffCheck = true;
            }
            else
            {
                resultStuffCheck = false;
            }
        }
    }     // CraftingCheck()

    // 제작 버튼을 눌렀을 때 제작 조건을 체크하고 제작 실행을 연결하는 함수
    public void ConnectCrafting()
    {
        // 재료 아이템이 충분하지 않으면 실행하지 않음
        if (detailOrder == 3 && resultStuffCheck == true)
        {
            Crafting();
        }
    }     // ConnectCrafting()

    // 제작을 실행하는 함수
    public void Crafting()
    {
        ItemsMain itemInfo = new ItemsMain();

        // 재료 아이템의 갯수만큼 순차적으로 중첩 수를 빼줌
        for (int i = 0; i < craftingLength; i++)
        {
            Debug.LogFormat("{0} : {1} 개 소모", stuffName[i], stuffStackNum[i]);
            ItemManager.instance.RemoveCraftingItem(stuffName[i], stuffStackNum[i], stuffItemTypes[i], out itemInfo);
        }

        // 아이템을 제작 후 인벤토리에 추가
        ItemManager.instance.InventoryAdd(craftingName, craftingStackNum, out itemInfo);

        Debug.LogFormat("{0} : {1} 개 제작 완료", craftingName, craftingStackNum);

        ShowOrder();
    }     // Crafting()

    #endregion 크래프팅 상세 정보 기능
}