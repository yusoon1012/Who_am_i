using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region 변수 설정

    public Transform quickSlotTf;

    // 보고있는 UI 창 타입 설정
    // 0 : UI 창 모두 꺼짐
    // 1 : 메인 메뉴 창
    // 2 : 인벤토리 창
    // 3 : 인벤토리 세부 창
    // 4 : 제작 창
    // 5 : 도감 창
    // 6 : 퀵슬롯 창
    // 7 : 아이템 버리기 창
    // 8 : 제작 세부 창
    public int uiController = default;

    // 플레이어 트랜스폼
    private Transform playerTf = default;

    #endregion 변수 설정

    void Awake()
    {
        uiController = 0;
    }     // Awake()

    void Start()
    {
        playerTf = GetComponent<Transform>().transform;
    }     // Start()

    // 모든 UI 에서 방향키 입력을 받아 전달하는 함수
    public void DirectionControl(int arrowType)
    {
        if (uiController == 1)
        {
            playerTf.GetComponent<MainMenu>().ChangeOrder(arrowType);
        }
        else if (uiController == 2)
        {
            playerTf.GetComponent<Inventory>().OrderCheck(arrowType);
        }
        else if (uiController == 3)
        {
            playerTf.GetComponent<Inventory>().DetailOrderCheck(arrowType);
        }
        else if (uiController == 4)
        {
            playerTf.GetComponent<ItemCrafting>().OrderCheck(arrowType);
        }
        else if (uiController == 5)
        {
            playerTf.GetComponent<Dictionary>().ChangeOrder(arrowType);
        }
        else if (uiController == 6)
        {
            quickSlotTf.GetComponent<QuickSlot>().DirectionControl(arrowType);
        }
        else if (uiController == 7)
        {
            playerTf.GetComponent<Inventory>().DropItemOrderCheck(arrowType);
        }
        else if (uiController == 8)
        {
            playerTf.GetComponent<ItemCrafting>().DetailOrderCheck(arrowType);
        }
    }     // DirectionControl()

    // 모든 UI 에서 다른 방향키 입력을 받아 전달하는 함수
    public void RightDirectionControl(int arrowType)
    {
        if (uiController == 2)
        {
            if (arrowType == 2 || arrowType == 3)
            {
                playerTf.GetComponent<Inventory>().ChangePage(arrowType);
            }
            else if (arrowType == 0 || arrowType == 1)
            {
                playerTf.GetComponent<Inventory>().ChangeItemGroupPage(arrowType);
            }
        }
        else if (uiController == 4)
        {
            playerTf.GetComponent<ItemCrafting>().ControlDetailOrder(arrowType);
        }
        else if (uiController == 5)
        {
            playerTf.GetComponent<Dictionary>().OtherChangeOrder(arrowType);
        }
    }     // RightDirectionControl()

    // 모든 UI 에서 진입, 뒤로가기 입력을 받아 전달하는 함수
    public void OnOffControl(int keyType)
    {
        // 진입 키 입력 값 전달
        if (keyType == 0)
        {
            switch (uiController)
            {
                case 0:
                    uiController = 1;
                    playerTf.GetComponent<MainMenu>().OnMainMenu();
                    break;
                case 1:
                    playerTf.GetComponent<MainMenu>().ConnectMenu();
                    break;
                case 2:
                    playerTf.GetComponent<Inventory>().OnItemDetailInfo();
                    break;
                case 3:
                    playerTf.GetComponent<Inventory>().SelectInfo();
                    break;
                case 4:
                    uiController = 8;
                    playerTf.GetComponent<ItemCrafting>().OnDetailCrafting();
                    break;
                case 6:
                    quickSlotTf.GetComponent<QuickSlot>().SelectSlot();
                    break;
                case 7:
                    playerTf.GetComponent<Inventory>().FunctionDropItem();
                    break;
                case 8:
                    playerTf.GetComponent<ItemCrafting>().Crafting();
                    break;
                default:
                    break;
            }
        }
        // 뒤로가기 키 입력 값 전달
        else if (keyType == 1)
        {
            switch (uiController)
            {
                case 1:
                    uiController = 0;
                    playerTf.GetComponent<MainMenu>().OffMainMenu();
                    break;
                case 2:
                    uiController = 1;
                    playerTf.GetComponent<Inventory>().ExitInventory();
                    playerTf.GetComponent<MainMenu>().DisconnectMenu();
                    break;
                case 3:
                    uiController = 2;
                    playerTf.GetComponent<Inventory>().OffItemDetailInfo();
                    break;
                case 4:
                    uiController = 0;
                    playerTf.GetComponent<ItemCrafting>().ExitCrafting();
                    break;
                case 5:
                    uiController = 1;
                    playerTf.GetComponent<Dictionary>().OffDictionary();
                    playerTf.GetComponent<MainMenu>().DisconnectMenu();
                    break;
                case 6:
                    uiController = 3;
                    quickSlotTf.GetComponent<QuickSlot>().ConnectInventory();
                    break;
                case 7:
                    uiController = 3;
                    playerTf.GetComponent<Inventory>().ExitDropItem(1);
                    break;
                case 8:
                    uiController = 4;
                    playerTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
                    break;
                default:
                    break;
            }
        }
    }     // OnOffControl()

    #region Input 키 입력 값 모음

    void Update()
    {
        // 모든 상, 하, 좌, 우 기본 키보드 키 입력 값
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DirectionControl(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DirectionControl(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DirectionControl(2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DirectionControl(3);
        }
        // 모든 진입 키 입력 값
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            OnOffControl(0);
        }
        // 모든 뒤로가기 키 입력 값
        else if (Input.GetKeyDown(KeyCode.X))
        {
            OnOffControl(1);
        }
        // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 값
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            RightDirectionControl(0);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            RightDirectionControl(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            RightDirectionControl(2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            RightDirectionControl(3);
        }
        else if (Input.GetKeyDown(KeyCode.P) && uiController == 0)
        {
            uiController = 4;
            playerTf.GetComponent<ItemCrafting>().OnCrafting();
        }


        #region LAGACY
        //* LEGACY CODE
        //// 인벤토리 창을 열고있는 상태에서만 실행
        //if (playerTf.GetComponent<Inventory>().lookInventory == true)
        //{
        //    // 아이템 상세 정보창을 보고있지 않을때 실행
        //    if (playerTf.GetComponent<Inventory>().lookItemDetailInfo == false)
        //    {
        //        // 상, 하, 좌, 우 키를 눌렀을 때
        //        if (Input.GetKeyDown(KeyCode.LeftArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().OrderCheck(0);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.RightArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().OrderCheck(1);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.UpArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().OrderCheck(2);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.DownArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().OrderCheck(3);
        //        }
        //        // 확인 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.Z))
        //        {
        //            playerTf.GetComponent<Inventory>().OnItemDetailInfo();
        //        }
        //        // 뒤로 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.X))
        //        {
        //            playerTf.GetComponent<Inventory>().ExitInventory();
        //        }
        //        // 아이템 타입 페이지 업 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.Keypad4))
        //        {
        //            playerTf.GetComponent<Inventory>().PageUp();
        //        }
        //        // 아이템 타입 페이지 다운 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.Keypad6))
        //        {
        //            playerTf.GetComponent<Inventory>().PageDown();
        //        }
        //        // 아이템 수량 페이지 업 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.Keypad8))
        //        {
        //            playerTf.GetComponent<Inventory>().ChangeItemGroupPage(0);
        //        }
        //        // 아이템 수량 페이지 다운 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.Keypad2))
        //        {
        //            playerTf.GetComponent<Inventory>().ChangeItemGroupPage(1);
        //        }
        //    }
        //    // 아이템 상세 정보창을 보고있을 때 실행
        //    else
        //    {
        //        // 상, 하, 좌, 우 키를 눌렀을 때
        //        if (Input.GetKeyDown(KeyCode.LeftArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().DetailOrderCheck(0);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.RightArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().DetailOrderCheck(1);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.UpArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().DetailOrderCheck(2);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.DownArrow))
        //        {
        //            playerTf.GetComponent<Inventory>().DetailOrderCheck(3);
        //        }
        //        // 뒤로 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.X))
        //        {
        //            playerTf.GetComponent<Inventory>().OffItemDetailInfo();
        //        }
        //    }
        //}
        //else if (playerTf.GetComponent<ItemCrafting>().lookCrafting == true)
        //{
        //    // 제작 상세 정보창을 보고있지 않을때 실행
        //    if (playerTf.GetComponent<ItemCrafting>().lookCraftingInfo == false)
        //    {
        //        // 상, 하, 좌, 우 키를 눌렀을 때
        //        if (Input.GetKeyDown(KeyCode.LeftArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().OrderCheck(0);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.RightArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().OrderCheck(1);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.UpArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().OrderCheck(2);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.DownArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().OrderCheck(3);
        //        }
        //        // 확인 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.Z))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().OnDetailCrafting();
        //        }
        //        // 뒤로 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.X))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().ExitCrafting();
        //        }
        //        // 제작 아이템 타입 페이지 변경 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().PageChange();
        //        }
        //    }
        //    // 제작 상세 정보창을 보고있을 때 실행
        //    else
        //    {
        //        // 상, 하, 좌, 우 키를 눌렀을 때
        //        if (Input.GetKeyDown(KeyCode.LeftArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().DetailOrderCheck(0);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.RightArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().DetailOrderCheck(1);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.UpArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().DetailOrderCheck(2);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.DownArrow))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().DetailOrderCheck(3);
        //        }
        //        // 뒤로 키를 눌렀을 때
        //        else if (Input.GetKeyDown(KeyCode.X))
        //        {
        //            playerTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
        //        }
        //    }
        //}
        //// 아무 UI 창도 열지 않았을 때 실행
        //else
        //{
        //    // 인벤토리 열기 키를 눌렀을 때
        //    if (Input.GetKeyDown(KeyCode.N))
        //    {
        //        playerTf.GetComponent<Inventory>().ControlInventory();
        //    }
        //    // 크래프팅 열기 키를 눌렀을 때
        //    else if (Input.GetKeyDown(KeyCode.M))
        //    {
        //        playerTf.GetComponent<ItemCrafting>().OnCrafting();
        //    }
        //}

        //// Legacy : 인벤토리 아이콘을 클릭했을 때 실행되는 함수
        //if (Input.GetMouseButtonDown(0))
        //{
        //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        //    {
        //        // "ItemImg" 태그로 아이콘을 구분함
        //        if (hitInfo.transform.tag == "ItemImg")
        //        {
        //            hitItemSlot = hitInfo.transform.gameObject;

        //            // 클릭한 인벤토리 아이콘의 활성화 값이 true 면 실행
        //            if (hitItemSlot.GetComponent<SaveItemInfo>().activeSlotCheck == true)
        //            {
        //                hitItemSlot.GetComponent<SaveItemInfo>().LoadNameInfo(out string itemName);
        //                playerTf.GetComponent<Inventory>().OnItemInfo(itemName);
        //            }
        //            // 클릭한 인벤토리 아이콘의 활성화 값이 false 면 실행
        //            else
        //            {
        //                playerTf.GetComponent<Inventory>().EmptyInfo();
        //            }
        //        }
        //        // "CraftingImg" 태그로 아이콘을 구분함
        //        else if (hitInfo.transform.tag == "CraftingImg")
        //        {
        //            hitItemSlot = hitInfo.transform.gameObject;

        //            hitItemSlot.GetComponent<SaveCraftingInfo>().LoadInfo(out string slotName);
        //            playerTf.GetComponent<ItemCrafting>().CraftingCheck(slotName);
        //        }
        //    }
        //}
    }     // Update()

    #endregion LAGACY

    #endregion Input 키 입력 값 모음
}