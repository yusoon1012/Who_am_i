using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    #region 변수 설정

    // 퀵슬롯 UI 오브젝트
    public GameObject quickSlotObj;
    // 퀵슬롯 칸 이미지 목록
    public Image[] quickSlotImage = new Image[6];
    // 퀵슬롯 아이템 이름 목록
    public Text[] quickSlotName = new Text[6];
    // 퀵슬롯 아이템 이미지 목록
    public Image[] quickSlotItemImage = new Image[6];
    // 퀵슬롯 아이템 중첩 수 표시 목록
    public Text[] quickSlotStack = new Text[3];
    // 플레이어 트랜스폼
    public Transform playerTf;

    // 퀵슬롯 커서를 움직일 때 변경되는 색
    private Color changeColor = default;

    // 퀵슬롯 트랜스폼
    private Transform thisQuickSlotObj = default;

    // 퀵슬롯에 저장된 아이템 이름 목록
    private string[] quickSlotSaveName = new string[6];
    // 퀵슬롯으로 보낸 현재 선택중인 아이템 이름
    private string connectItemName = default;

    // 퀵슬롯에서 움직이는 UI 표적 값
    private int order = default;
    // 퀵슬롯으로 보낸 현재 선택중인 아이템의 타입
    private int itemType = default;

    #endregion 변수 설정

    void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            quickSlotSaveName[i] = "None";
        }

        connectItemName = "None";

        order = 0;
        itemType = 0;
    }     // Awake()

    void Start()
    {
        thisQuickSlotObj = GetComponent<Transform>().transform;
    }     // Start()

    // 퀵슬롯 목록들을 다시 재설정하는 함수
    public void ResetQuickSlot()
    {
        ItemsMain items = new ItemsMain();

        for (int i = 0; i < 6; i++)
        {
            // 퀵슬롯에 저장된 아이템 이름이 "None" 이 아니면 실행
            if (quickSlotSaveName[i] != "None")
            {
                // 도구 타입의 퀵슬롯이 선택중이면 실행
                if (i < 3)
                {
                    ItemManager.instance.QuickSlotItemInfo(quickSlotSaveName[i], 0, out items);
                }
                // 음식 타입의 퀵슬롯이 선택중이면 실행
                else if (i >= 3)
                {
                    ItemManager.instance.QuickSlotItemInfo(quickSlotSaveName[i], 1, out items);
                }

                quickSlotName[i].text = string.Format("{0}", items.itemName);
                quickSlotItemImage[i].sprite = ItemManager.instance.itemImages[items.itemImageNum].sprite;

                if (i >= 3)
                {
                    quickSlotStack[i - 3].text = string.Format("{0}", items.itemStack);
                }
            }
            // 퀵슬롯에 저장된 아이템 이름이 "None" 이면 실행
            else
            {
                quickSlotName[i].text = string.Format(" ");
                quickSlotItemImage[i].sprite = ItemManager.instance.itemImages[0].sprite;

                if (i >= 3)
                {
                    quickSlotStack[i - 3].text = string.Format(" ");
                }
            }
        }
    }     // ResetQuickSlot()

    // 인벤토리에서 선택된 아이템 정보를 초기 설정하는 함수
    public void TakeItemName(string itemName, int itemTypeNum)
    {
        connectItemName = itemName;
        itemType = itemTypeNum;

        quickSlotObj.SetActive(true);

        if (itemType == 0 || itemType == 2)
        {
            order = 0;
        }
        else if (itemType == 1)
        {
            order = 3;
        }

        changeColor = new Color32(255, 255, 255, 255);
        quickSlotImage[order].color = changeColor;
    }     // TakeItemName()

    // 퀵슬롯 UI 에서 인벤토리 UI 로 넘어가는 함수
    public void ConnectInventory()
    {
        changeColor = new Color32(155, 155, 155, 255);
        quickSlotImage[order].color = changeColor;

        connectItemName = string.Empty;

        quickSlotObj.SetActive(false);
        playerTf.GetComponent<MainMenu>().mainMenu.SetActive(true);
        playerTf.GetComponent<Inventory>().inventory.SetActive(true);
        playerTf.GetComponent<Inventory>().itemInfo.SetActive(true);
    }     // ConnectInventory()

    // 퀵슬롯에서 각 칸을 이동하는 기능 함수
    public void DirectionControl(int arrowType)
    {
        changeColor = new Color32(155, 155, 155, 255);
        quickSlotImage[order].color = changeColor;

        switch (itemType)
        {
            case 0:
                if (arrowType == 0)
                {
                    if (order == 0)
                    {
                        order = 2;
                    }
                    else
                    {
                        order -= 1;
                    }
                }
                else if (arrowType == 1)
                {
                    if (order == 2)
                    {
                        order = 0;
                    }
                    else
                    {
                        order += 1;
                    }
                }
                break;
            case 1:
                if (arrowType == 0)
                {
                    if (order == 3)
                    {
                        order = 5;
                    }
                    else
                    {
                        order -= 1;
                    }
                }
                else if (arrowType == 1)
                {
                    if (order == 5)
                    {
                        order = 3;
                    }
                    else
                    {
                        order += 1;
                    }
                }
                break;
            case 2:
                if (arrowType == 0)
                {
                    if (order == 0)
                    {
                        order = 2;
                    }
                    else if (order == 3)
                    {
                        order = 5;
                    }
                    else
                    {
                        order -= 1;
                    }
                }
                else if (arrowType == 1)
                {
                    if (order == 2)
                    {
                        order = 0;
                    }
                    else if (order == 5)
                    {
                        order = 3;
                    }
                    else
                    {
                        order += 1;
                    }
                }
                else if (arrowType == 2)
                {
                    if (order < 3)
                    {
                        order += 3;
                    }
                    else if (order >= 3)
                    {
                        order -= 3;
                    }
                }
                else if (arrowType == 3)
                {
                    if (order < 3)
                    {
                        order += 3;
                    }
                    else if (order >= 3)
                    {
                        order -= 3;
                    }
                }
                break;
        }

        changeColor = new Color32(255, 255, 255, 255);
        quickSlotImage[order].color = changeColor;
    }     // DirectionControl()

    // 퀵슬롯에서 선택중인 칸을 눌렀을때 기능하는 함수
    public void SelectSlot()
    {
        // 현재 선택중인 아이템 이름이 "None" 이 아닐때 (인벤토리에서 퀵슬롯 UI 를 열었을 경우)
        if (connectItemName != "None")
        {
            // 퀵슬롯에 저장된 아이템 이름이 현재 선택중인 아이템 이름과 다를 때
            if (quickSlotSaveName[order] != connectItemName)
            {
                OverlapCheck();

                quickSlotSaveName[order] = connectItemName;
            }
            // 퀵슬롯에 저장된 아이템 이름이 현재 선택중인 아이템 이름과 같을 때
            else
            {
                quickSlotSaveName[order] = "None";
            }
        }
        // 현재 선택중인 아이템 이름이 "None" 일 때 (바로 퀵슬롯 UI 를 연 경우)
        else
        {
            //* Feat : 도구 장착, 음식 사용 기능 추가 예정
        }

        ResetQuickSlot();
    }     // SelectSlot()

    // 퀵슬롯에 등록 할 아이템 정보가 이미 퀵슬롯에 저장되어있는 아이템 정보인지 체크하는 함수
    public void OverlapCheck()
    {
        for (int i = 0; i < 6; i++)
        {
            if (order != i)
            {
                if (quickSlotSaveName[i] == connectItemName)
                {
                    quickSlotSaveName[i] = "None";
                }
            }
        }
    }     // OverlapCheck()
}
