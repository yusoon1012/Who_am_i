using UnityEngine;
using BNG;

public class UIController : MonoBehaviour
{
    #region 변수 설정

    // 퀵슬롯 트랜스폼
    public Transform quickSlotTf;
    // 지도 카메라 트랜스폼
    public Transform mapCameraTf;
    // 맵 컨트롤러 트랜스폼
    public Transform mapControllerTf;
    // NPC 컨트롤러 트랜스폼
    public Transform npcControllerTf;

    // 활성화 상태 UI 창 타입 구분값
    // 0 : UI 창 모두 꺼짐
    // 1 : 메인 메뉴 창
    // 2 : 인벤토리 창
    // 3 : 인벤토리 세부 창
    // 4 : 제작 창
    // 5 : 도감 창
    // 6 : 인벤토리 퀵슬롯 창
    // 7 : 아이템 버리기 창
    // 8 : 제작 세부 창
    // 9 : 사용 퀵슬롯 창
    // 10 : 지도 창
    // 11 : 지도 선택 창
    // 12 : NPC 대화 상태
    // 13 : 제작 완료 안내 창
    public int uiController { get; set; } = default;

    // 아이템 데이터 그룹 메인 오브젝트 트랜스폼
    private Transform mainObjTf = default;

    #endregion 변수 설정

    // <Solbin> VRIF Action
    private VRIFAction vrifAction = default;
    // <Solbin> VRIF State System
    [SerializeField] private VRIFStateSystem vrifStateSystem = default;
    // <Solbin> VRIF Time System
    [SerializeField] private VRIFTimeSystem vrifTimeSystem = default;
    // <Solbin> UI 조이스틱 입력 기준값
    private float joystickInput = 0.95f;
    // <Solbin> Input Delay를 위한 bool 값
    private bool inputDelay = false;

    void Awake()
    {
        uiController = 0;
    }     // Awake()

    void Start()
    {
        mainObjTf = GetComponent<Transform>().transform;

        mainObjTf.GetComponent<Inventory>().AddInventory("너프건", 1);
    }     // Start()

    void Update()
    {
        if (inputDelay) { return; } // <Solbin> 입력 딜레이 중 입력 금지 

        UpdateFunction();
    }     // Update()

    #region UpdateFunction()

    //private void StopMovePlayer()
    //{
    //    mainObjTf.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //}     // StopMovePlayer()

    // Update 마다 조건에 맞으면 실행되는 함수
    private void UpdateFunction()
    {
        if (uiController == 0)
        {
            mainObjTf.GetComponent<Meen_MovePlayer>().UpdateFunction();

            if (npcControllerTf.GetComponent<NPCController>().onNavigationCheck > 0)
            {
                npcControllerTf.GetComponent<NPCController>().ConnectUpdateFunction();
            }
        }
        else if (uiController == 10) { mapCameraTf.GetComponent<CameraControl>().UpdateFunction(); }

        // 모든 상, 하, 좌, 우 기본 키보드 키 입력 값
        if (Input.GetKeyDown(KeyCode.UpArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().y >= joystickInput)
        {
            // <Solbin> GetKey 같은 느낌이라 너무 예민하다고 느껴진다. 수정 필요
            if (!inputDelay) { DirectionControl(0); }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().y <= -joystickInput)
        {
            if (!inputDelay) { DirectionControl(1); }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().x <= -joystickInput)
        {
            if (!inputDelay) { DirectionControl(2); }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().x >= joystickInput)
        {
            if (!inputDelay) { DirectionControl(3); }
        }
        // 모든 상, 하, 좌, 우 키보드 키 입력 종료 값
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            KeyUpDirectionControl(0);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            KeyUpDirectionControl(1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            KeyUpDirectionControl(2);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            KeyUpDirectionControl(3);
        }
        // <Solbin> 메뉴 진입
        else if (Input.GetKeyDown(KeyCode.M) || vrifAction.Player.UI_Menu.triggered) // <Solbin> Menu Enter
        {
            // <Solbin> 선택 키 감 개선 (키 매핑이 풀리는 문제가 있어 이곳에 삽입)
            vrifAction.Player.UI_Click.performed += ctx => OnOffControl(0);
            // <Solbin> ===

            OnMainMenuControl();
        }
        // 모든 진입 키 입력 값
        else if (Input.GetKeyDown(KeyCode.Z)) // <Solbin> Menu Select
        {
            OnOffControl(0);
        }
        // 모든 뒤로가기 키 입력 값
        else if (Input.GetKeyDown(KeyCode.X) || vrifAction.Player.UI_Exit.triggered) // <Solbin> Exit Menu
        {
            OnOffControl(1);

            //if (vrifStateSystem.gameState == VRIFStateSystem.GameState.UI) // <Solbin> UI 상태일때
            //{
            //    OnOffControl(1);
            //    vrifStateSystem.ChangeState(VRIFStateSystem.GameState.NORMAL); // <Solbin> NORMAL 상태로 전환
            //}
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            npcControllerTf.GetComponent<NPCController>().InputController();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            mapCameraTf.GetComponent<CameraControl>().FunctionCheckPoint();
        }
        // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 값
        else if (Input.GetKeyDown(KeyCode.Keypad8) || vrifAction.Player.RightController.ReadValue<Vector2>().y >= joystickInput)
        {
            if (!inputDelay) { RightDirectionControl(0); }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || vrifAction.Player.RightController.ReadValue<Vector2>().y <= -joystickInput)
        {
            if (!inputDelay) { RightDirectionControl(1); }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || vrifAction.Player.RightController.ReadValue<Vector2>().x <= -joystickInput)
        {
            if (!inputDelay) { RightDirectionControl(2); }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6) || vrifAction.Player.RightController.ReadValue<Vector2>().x >= joystickInput)
        {
            if (!inputDelay) { RightDirectionControl(3); }
        }
        // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 종료 값
        else if (Input.GetKeyUp(KeyCode.Keypad8))
        {
            KeyUpRightDirectionControl(0);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            KeyUpRightDirectionControl(1);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            KeyUpRightDirectionControl(2);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            KeyUpRightDirectionControl(3);
        }
        else if (Input.GetKeyDown(KeyCode.P) && uiController == 0)
        {
            uiController = 4;
            mainObjTf.GetComponent<ItemCrafting>().OnCrafting();
            OpenMenuCheck();
        }
        //* Test : 아래의 해당 키를 누르면 인벤토리에 아이템 추가 기능
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainObjTf.GetComponent<Inventory>().AddInventory("딸기", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mainObjTf.GetComponent<Inventory>().AddInventory("우유", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mainObjTf.GetComponent<Inventory>().AddInventory("고기", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            mainObjTf.GetComponent<Inventory>().AddInventory("송이 버섯", 1);
        }

        // <Solbin> 지나치게 예민한 입력값을 막기 위함
        if (inputDelay) { Invoke("ClearInputDelay", 0.55f); }
        // <Solbin> ===
    }     // UpdateFunction()

    #endregion UpdateFunction()

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
    }

    #region DirectionControl()

    // 모든 UI 에서 방향키 입력을 받아 전달하는 함수
    public void DirectionControl(int arrowType)
    {
        // <Solbin>
        inputDelay = true;
        // <Solbin> ===

        switch (uiController)
        {
            case 1:
                mainObjTf.GetComponent<MainMenu>().ChangeOrder(arrowType);
                break;
            case 2:
                mainObjTf.GetComponent<Inventory>().OrderCheck(arrowType);
                break;
            case 3:
                mainObjTf.GetComponent<Inventory>().DetailOrderCheck(arrowType);
                break;
            case 4:
                mainObjTf.GetComponent<ItemCrafting>().OrderCheck(arrowType);
                break;
            case 5:
                mainObjTf.GetComponent<Dictionary>().ChangeOrder(arrowType);
                break;
            case 6:
                quickSlotTf.GetComponent<QuickSlot>().DirectionControl(arrowType);
                break;
            case 7:
                mainObjTf.GetComponent<Inventory>().DropItemOrderCheck(arrowType);
                break;
            case 8:
                mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(arrowType);
                break;
            case 9:
                quickSlotTf.GetComponent<QuickSlot>().DirectionControl(arrowType);
                break;
            case 10:
                mapCameraTf.GetComponent<CameraControl>().MoveMap(arrowType);
                break;
            case 11:
                mapCameraTf.GetComponent<CameraControl>().ChangeCheckPointButton(arrowType);
                break;
            default:
                break;
        }
    }     // DirectionControl()

    // 모든 상, 하, 좌, 우 키보드 키 입력 종료 값을 받아 전달하는 함수
    public void KeyUpDirectionControl(int arrowType)
    {
        switch (uiController)
        {
            case 10:
                mapCameraTf.GetComponent<CameraControl>().StopMoveMap(arrowType);
                break;
            default:
                break;
        }
    }     // KeyUpDirectionControl()

    // 모든 UI 에서 다른 방향키 입력을 받아 전달하는 함수
    public void RightDirectionControl(int arrowType)
    {
        // <Solbin>
        inputDelay = true;
        // <Solbin> ===

        switch (uiController)
        {
            case 2:
                if (arrowType == 2 || arrowType == 3)
                {
                    mainObjTf.GetComponent<Inventory>().ChangePage(arrowType);
                }
                else if (arrowType == 0 || arrowType == 1)
                {
                    mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(arrowType);
                }
                break;
            case 4:
                mainObjTf.GetComponent<ItemCrafting>().ControlDetailOrder(arrowType);
                break;
            case 5:
                mainObjTf.GetComponent<Dictionary>().OtherChangeOrder(arrowType);
                break;
            case 10:
                if (arrowType == 0 || arrowType == 1) { mapCameraTf.GetComponent<CameraControl>().CheckInCamera(arrowType); }
                break;
            default:
                break;
        }
    }     // RightDirectionControl()

    // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 종료 값을 받아 전달하는 함수
    public void KeyUpRightDirectionControl(int arrowType)
    {
        switch (uiController)
        {
            case 10:
                if (arrowType == 0 || arrowType == 1) { mapCameraTf.GetComponent<CameraControl>().CheckOutCamera(arrowType); }
                break;
            default:
                break;
        }
    }     // KeyUpRightDirectionControl()

    #endregion DirectionControl()

    #region OnOffControl()

    // 모든 UI 에서 진입, 뒤로가기 입력을 받아 전달하는 함수
    public void OnOffControl(int keyType)
    {
        // 진입 키 입력 값 전달
        if (keyType == 0)
        {
            switch (uiController)
            {
                // 아무 UI 도 안켜져 있을 때 A 버튼을 누르면 퀵슬롯이 켜짐
                case 0:
                    //StopMovePlayer();
                    uiController = 9;
                    quickSlotTf.GetComponent<QuickSlot>().SingleOpenQuickSlot();
                    OpenQuickSlot();
                    break;
                case 1:
                    mainObjTf.GetComponent<MainMenu>().ConnectMenu();
                    break;
                case 2:
                    mainObjTf.GetComponent<Inventory>().OnItemDetailInfo();
                    break;
                case 3:
                    mainObjTf.GetComponent<Inventory>().SelectInfo();
                    break;
                case 4:
                    uiController = 8;
                    mainObjTf.GetComponent<ItemCrafting>().OnDetailCrafting();
                    break;
                case 6:
                    quickSlotTf.GetComponent<QuickSlot>().SelectSlot();
                    break;
                case 7:
                    mainObjTf.GetComponent<Inventory>().FunctionDropItem();
                    break;
                case 8:
                    mainObjTf.GetComponent<ItemCrafting>().ConnectCrafting();
                    break;
                case 9:
                    quickSlotTf.GetComponent<QuickSlot>().SelectSlot();
                    break;
                case 10:
                    mapCameraTf.GetComponent<CameraControl>().SelectCheckPoint();
                    break;
                case 11:
                    mapCameraTf.GetComponent<CameraControl>().SelectCheckPointButton();
                    break;
                case 13:
                    uiController = 4;
                    mainObjTf.GetComponent<ItemCrafting>().ExitCompleteCrafting();
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
                    mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                    ExitMenuCheck();
                    break;
                case 2:
                    uiController = 1;
                    mainObjTf.GetComponent<Inventory>().ExitInventory();
                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                    break;
                case 3:
                    uiController = 2;
                    mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
                    break;
                case 4:
                    uiController = 0;
                    mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
                    ExitMenuCheck();
                    break;
                case 5:
                    uiController = 1;
                    mainObjTf.GetComponent<Dictionary>().OffDictionary();
                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                    break;
                case 6:
                    uiController = 3;
                    quickSlotTf.GetComponent<QuickSlot>().ConnectInventory();
                    break;
                case 7:
                    uiController = 3;
                    mainObjTf.GetComponent<Inventory>().ExitDropItem(1);
                    break;
                case 8:
                    uiController = 4;
                    mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
                    break;
                case 9:
                    uiController = 0;
                    quickSlotTf.GetComponent<QuickSlot>().SingleCloseQuickSlot();
                    ExitQuickSlot();
                    break;
                case 10:
                    uiController = 1;
                    mapControllerTf.GetComponent<MapControl>().ExitMap();
                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                    break;
                case 11:
                    mapCameraTf.GetComponent<CameraControl>().ExitCheckPointInfo();
                    break;
                case 13:
                    uiController = 4;
                    mainObjTf.GetComponent<ItemCrafting>().ExitCompleteCrafting();
                    break;
                default:
                    break;
            }
        }
    }     // OnOffControl()

    // 메인 메뉴 버튼을 눌렀을 때 메인 메뉴를 열고, 다른 메뉴들이 열려있으면 모두 닫고 초기화 시키는 함수
    public void OnMainMenuControl()
    {
        switch (uiController)
        {
            // 메뉴가 아무것도 안열려 있으면 메뉴 UI 를 연다
            case 0:
                //StopMovePlayer();
                uiController = 1;
                mainObjTf.GetComponent<MainMenu>().OnMainMenu();
                OpenMenuCheck(); // <Solbin> 메인메뉴 열릴 때 => UI 상태로 전환 
                break;
            // 메뉴가 하나라도 켜져있으면 해당 메뉴를 모두 닫고 초기화 화면으로 돌아감
            case 1:
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 2:
                mainObjTf.GetComponent<Inventory>().ExitInventory();
                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 3:
                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
                mainObjTf.GetComponent<Inventory>().ExitInventory();
                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 4:
                mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 5:
                mainObjTf.GetComponent<Dictionary>().OffDictionary();
                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 6:
                quickSlotTf.GetComponent<QuickSlot>().ConnectInventory();
                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
                mainObjTf.GetComponent<Inventory>().ExitInventory();
                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 7:
                mainObjTf.GetComponent<Inventory>().ExitDropItem(1);
                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
                mainObjTf.GetComponent<Inventory>().ExitInventory();
                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 8:
                mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
                mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 9:
                quickSlotTf.GetComponent<QuickSlot>().SingleCloseQuickSlot();
                ExitQuickSlot();
                uiController = 0;
                break;
            case 10:
                mapControllerTf.GetComponent<MapControl>().ExitMap();
                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 11:
                mapCameraTf.GetComponent<CameraControl>().ExitCheckPointInfo();
                mapControllerTf.GetComponent<MapControl>().ExitMap();
                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
                ExitMenuCheck();
                uiController = 0;
                break;
            case 13:
                mainObjTf.GetComponent<ItemCrafting>().ExitCompleteCrafting();
                mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
                mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
                ExitMenuCheck();
                uiController = 0;
                break;
            default:
                break;
        }
    }     // OnMainMenuControl()

    // 지도에서 워프 기능 사용 후 지도 기능을 비활성화 하는 함수
    public void AfterWarpExitMap()
    {
        mapCameraTf.GetComponent<CameraControl>().ExitCheckPointInfo();
        mapControllerTf.GetComponent<MapControl>().ExitMap();
        mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
        mainObjTf.GetComponent<MainMenu>().OffMainMenu();
        ExitMenuCheck();
        uiController = 0;
    }     // AfterWarpExitMap()

    #endregion OnOffControl()

    #region UI 상태에 따라 게임 슬로우 상태, 정지 상태 변경 기능

    // 단일 퀵슬롯이 열릴때 게임 시간 슬로우 효과
    private void OpenQuickSlot()
    {
        // TODO: 타임 슬로우 효과 연결, UI 상태로 변환
        VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.UI);
    }

    // 단일 퀵슬롯을 닫았을 때 슬로우 효과 해제
    private void ExitQuickSlot()
    {
        // TODO: 타임 슬로우 효과 해제 연결, NORMAL 상태로 변환
        VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.NORMAL);
    }

    // 어떤 메뉴든 열릴 때 게임 시간 정지
    private void OpenMenuCheck()
    {
        // <Solbin> 메뉴를 열 때 NORMAL 상태라면 UI 상태로 전환
        if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.NORMAL)
        {
            VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.UI);
        }// <Solbin> UI 상태로 전환 
    }

    // <Solbin>
    /// <summary>
    /// 조합대 상호작용으로 UI Controller 활성화
    /// </summary>
    public void OpenCraftingUI()
    {
        if (uiController == 0)
        {
            uiController = 4;
            mainObjTf.GetComponent<ItemCrafting>().OnCrafting();
            OpenMenuCheck();
        }
    }
    // <Solbin> ===

    // 어떤 메뉴든 닫았을 때 게임 시간 재개
    public void ExitMenuCheck()
    {
        // <Solbin> 메뉴를 닫을 때 UI 상태라면 NORMAL 상태로 전환
        if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.UI)
        {
            VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.NORMAL);
        }// <Solbin> NORMAL 상태로 전환 
    }

    #endregion UI 상태에 따라 게임 슬로우 상태, 정지 상태 변경 기능

    #region LAGACY
    //* LEGACY CODE
    //// 인벤토리 창을 열고있는 상태에서만 실행
    //if (mainObjTf.GetComponent<Inventory>().lookInventory == true)
    //{
    //    // 아이템 상세 정보창을 보고있지 않을때 실행
    //    if (mainObjTf.GetComponent<Inventory>().lookItemDetailInfo == false)
    //    {
    //        // 상, 하, 좌, 우 키를 눌렀을 때
    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().OrderCheck(0);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().OrderCheck(1);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().OrderCheck(2);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().OrderCheck(3);
    //        }
    //        // 확인 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.Z))
    //        {
    //            mainObjTf.GetComponent<Inventory>().OnItemDetailInfo();
    //        }
    //        // 뒤로 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            mainObjTf.GetComponent<Inventory>().ExitInventory();
    //        }
    //        // 아이템 타입 페이지 업 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.Keypad4))
    //        {
    //            mainObjTf.GetComponent<Inventory>().PageUp();
    //        }
    //        // 아이템 타입 페이지 다운 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.Keypad6))
    //        {
    //            mainObjTf.GetComponent<Inventory>().PageDown();
    //        }
    //        // 아이템 수량 페이지 업 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.Keypad8))
    //        {
    //            mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(0);
    //        }
    //        // 아이템 수량 페이지 다운 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.Keypad2))
    //        {
    //            mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(1);
    //        }
    //    }
    //    // 아이템 상세 정보창을 보고있을 때 실행
    //    else
    //    {
    //        // 상, 하, 좌, 우 키를 눌렀을 때
    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(0);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(1);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(2);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
    //        {
    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(3);
    //        }
    //        // 뒤로 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
    //        }
    //    }
    //}
    //else if (mainObjTf.GetComponent<ItemCrafting>().lookCrafting == true)
    //{
    //    // 제작 상세 정보창을 보고있지 않을때 실행
    //    if (mainObjTf.GetComponent<ItemCrafting>().lookCraftingInfo == false)
    //    {
    //        // 상, 하, 좌, 우 키를 눌렀을 때
    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(0);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(1);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(2);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(3);
    //        }
    //        // 확인 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.Z))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().OnDetailCrafting();
    //        }
    //        // 뒤로 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
    //        }
    //        // 제작 아이템 타입 페이지 변경 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().PageChange();
    //        }
    //    }
    //    // 제작 상세 정보창을 보고있을 때 실행
    //    else
    //    {
    //        // 상, 하, 좌, 우 키를 눌렀을 때
    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(0);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(1);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(2);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(3);
    //        }
    //        // 뒤로 키를 눌렀을 때
    //        else if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
    //        }
    //    }
    //}
    //// 아무 UI 창도 열지 않았을 때 실행
    //else
    //{
    //    // 인벤토리 열기 키를 눌렀을 때
    //    if (Input.GetKeyDown(KeyCode.N))
    //    {
    //        mainObjTf.GetComponent<Inventory>().ControlInventory();
    //    }
    //    // 크래프팅 열기 키를 눌렀을 때
    //    else if (Input.GetKeyDown(KeyCode.M))
    //    {
    //        mainObjTf.GetComponent<ItemCrafting>().OnCrafting();
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
    //                mainObjTf.GetComponent<Inventory>().OnItemInfo(itemName);
    //            }
    //            // 클릭한 인벤토리 아이콘의 활성화 값이 false 면 실행
    //            else
    //            {
    //                mainObjTf.GetComponent<Inventory>().EmptyInfo();
    //            }
    //        }
    //        // "CraftingImg" 태그로 아이콘을 구분함
    //        else if (hitInfo.transform.tag == "CraftingImg")
    //        {
    //            hitItemSlot = hitInfo.transform.gameObject;

    //            hitItemSlot.GetComponent<SaveCraftingInfo>().LoadInfo(out string slotName);
    //            mainObjTf.GetComponent<ItemCrafting>().CraftingCheck(slotName);
    //        }
    //    }
    //}
    #endregion LAGACY

    // <Solbin>
    private void ClearInputDelay() { inputDelay = false; }
    // <Solbin> ===

}     // Update()


#region Lagacy Function

// -------------------------- Super Real Lagacy --------------------------------//

//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using BNG;
//using System;

//public class UIController : MonoBehaviour
//{
//    #region 변수 설정

//    public Transform quickSlotTf;

//    // 보고있는 UI 창 타입 설정
//    // 0 : UI 창 모두 꺼짐
//    // 1 : 메인 메뉴 창
//    // 2 : 인벤토리 창
//    // 3 : 인벤토리 세부 창
//    // 4 : 제작 창
//    // 5 : 도감 창
//    // 6 : 인벤토리 퀵슬롯 창
//    // 7 : 아이템 버리기 창
//    // 8 : 제작 세부 창
//    // 9 : 사용 퀵슬롯 창
//    public int uiController { get; set; } = default;

//    public Transform 

//    // 플레이어 트랜스폼
//    private Transform mainObjTf = default;

//    #endregion 변수 설정

//    // <Solbin> VRIF Action
//    private VRIFAction vrifAction = default;
//    // <Solbin> VRIF State System
//    [SerializeField] private VRIFStateSystem vrifStateSystem = default;
//    // <Solbin> VRIF Time System
//    [SerializeField] private VRIFTimeSystem vrifTimeSystem = default;
//    // <Solbin> UI 조이스틱 입력 기준값
//    private float joystickInput = 0.55f;
//    // <Solbin> Input Delay를 위한 bool 값
//    private bool inputDelay = false;


//    void Awake()
//    {
//        uiController = 0;
//    }     // Awake()

//    void Start()
//    {
//        mainObjTf = GetComponent<Transform>().transform;
//    }     // Start()

//    void Update()
//    {
//        if (inputDelay) { return; } // <Solbin> 입력 딜레이 중 입력 금지

//        // 모든 상, 하, 좌, 우 기본 키보드 키 입력 값
//        if (Input.GetKeyDown(KeyCode.UpArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().y >= joystickInput)
//        {
//            if (!inputDelay) { DirectionControl(0); } // <Solbin> 키감 수정 포함
//        }
//        else if (Input.GetKeyDown(KeyCode.DownArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().y <= -joystickInput)
//        {
//            if (!inputDelay) { DirectionControl(1); } // <Solbin>
//        }
//        else if (Input.GetKeyDown(KeyCode.LeftArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().x <= -joystickInput)
//        {
//            if (!inputDelay) { DirectionControl(2); } // <Solbin> 
//        }
//        else if (Input.GetKeyDown(KeyCode.RightArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().x >= joystickInput)
//        {
//            if (!inputDelay) { DirectionControl(3); } // <Solbin> 
//        }
//        // <Solbin> 메뉴 진입
//        else if (Input.GetKeyDown(KeyCode.M) || vrifAction.Player.UI_Menu.triggered) // <Solbin> Menu Enter
//        {
//            OnMainMenuControl();
//        }
//        // 모든 진입 키 입력 값
//        else if (Input.GetKeyDown(KeyCode.Z) || vrifAction.Player.UI_Click.triggered) // <Solbin> Menu Select
//        {
//            OnOffControl(0);
//        }
//        // 모든 뒤로가기 키 입력 값
//        else if (Input.GetKeyDown(KeyCode.X) || vrifAction.Player.UI_Exit.triggered) // <Solbin> Exit Menu
//        {
//            OnOffControl(1);
//        }
//        // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 값
//        else if (Input.GetKeyDown(KeyCode.Keypad8) || vrifAction.Player.RightController.ReadValue<Vector2>().y >= joystickInput)
//        {
//            if (!inputDelay) { inputDelay = true; RightDirectionControl(0); } // <Solbin> 키감 수정 포함
//        }
//        else if (Input.GetKeyDown(KeyCode.Keypad2) || vrifAction.Player.RightController.ReadValue<Vector2>().y <= -joystickInput)
//        {
//            if (!inputDelay) { inputDelay = true; RightDirectionControl(1); } // <Solbin>
//        }
//        else if (Input.GetKeyDown(KeyCode.Keypad4) || vrifAction.Player.RightController.ReadValue<Vector2>().x <= -joystickInput)
//        {
//            if (!inputDelay) { inputDelay = true; RightDirectionControl(2); } // <Solbin>
//        }
//        else if (Input.GetKeyDown(KeyCode.Keypad6) || vrifAction.Player.RightController.ReadValue<Vector2>().x >= joystickInput)
//        {
//            if (!inputDelay) { inputDelay = true; RightDirectionControl(3); } // <Solbin>
//        }
//        else if (Input.GetKeyDown(KeyCode.P) && uiController == 0)
//        {
//            uiController = 4;
//            mainObjTf.GetComponent<ItemCrafting>().OnCrafting();
//            OpenMenuCheck();
//        }
//        //* Test : 아래의 해당 키를 누르면 인벤토리에 아이템 추가 기능
//        else if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            mainObjTf.GetComponent<Inventory>().AddInventory("딸기", 1);
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            mainObjTf.GetComponent<Inventory>().AddInventory("우유", 1);
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha3))
//        {
//            mainObjTf.GetComponent<Inventory>().AddInventory("고기", 1);
//        }
//    }



//    private void OnEnable()
//    {
//        vrifAction = new VRIFAction();
//        vrifAction.Enable();
//    }

//    private void OnDisable()
//    {
//        vrifAction?.Disable();
//    }

//    // 모든 UI 에서 방향키 입력을 받아 전달하는 함수
//    public void DirectionControl(int arrowType)
//    {
//        // <Solbin> 
//        inputDelay = true;
//        // <Solbin> ===

//        if (inputDelay)
//        {
//            if (uiController == 1)
//            {
//                mainObjTf.GetComponent<MainMenu>().ChangeOrder(arrowType);
//            }
//            else if (uiController == 2)
//            {
//                mainObjTf.GetComponent<Inventory>().OrderCheck(arrowType);
//            }
//            else if (uiController == 3)
//            {
//                mainObjTf.GetComponent<Inventory>().DetailOrderCheck(arrowType);
//            }
//            else if (uiController == 4)
//            {
//                mainObjTf.GetComponent<ItemCrafting>().OrderCheck(arrowType);
//            }
//            else if (uiController == 5)
//            {
//                mainObjTf.GetComponent<Dictionary>().ChangeOrder(arrowType);
//            }
//            else if (uiController == 6 || uiController == 9)
//            {
//                quickSlotTf.GetComponent<QuickSlot>().DirectionControl(arrowType);
//            }
//            else if (uiController == 7)
//            {
//                mainObjTf.GetComponent<Inventory>().DropItemOrderCheck(arrowType);
//            }
//            else if (uiController == 8)
//            {
//                mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(arrowType);
//            }

//            // <Solbin>
//            Invoke("ClearInputDelay", 0.5f);
//            // <Solbin> ===
//        }
//    }     // DirectionControl()

//    // 모든 UI 에서 다른 방향키 입력을 받아 전달하는 함수
//    public void RightDirectionControl(int arrowType)
//    {
//        // <Solbin>
//        inputDelay = true;
//        // <Solbin> ===

//        if (inputDelay)
//        {
//            if (uiController == 2)
//            {
//                if (arrowType == 2 || arrowType == 3)
//                {
//                    mainObjTf.GetComponent<Inventory>().ChangePage(arrowType);
//                }
//                else if (arrowType == 0 || arrowType == 1)
//                {
//                    mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(arrowType);
//                }
//            }
//            else if (uiController == 4)
//            {
//                mainObjTf.GetComponent<ItemCrafting>().ControlDetailOrder(arrowType);
//            }
//            else if (uiController == 5)
//            {
//                mainObjTf.GetComponent<Dictionary>().OtherChangeOrder(arrowType);
//            }

//            // <Solbin>
//            Invoke("ClearInputDelay", 0.5f);
//            // <Solbin> ===
//        }
//    }     // RightDirectionControl()

//    // 모든 UI 에서 진입, 뒤로가기 입력을 받아 전달하는 함수
//    public void OnOffControl(int keyType)
//    {
//        // 진입 키 입력 값 전달
//        if (keyType == 0)
//        {
//            switch (uiController)
//            {
//                // 아무 UI 도 안켜져 있을 때 A 버튼을 누르면 퀵슬롯이 켜짐
//                case 0:
//                    if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.NORMAL) // <Solbin> 퀵슬롯 제한 추가 
//                    {
//                        uiController = 9;
//                        quickSlotTf.GetComponent<QuickSlot>().SingleOpenQuickSlot();
//                        OpenQuickSlot();
//                    }
//                    break;
//                case 1:
//                    mainObjTf.GetComponent<MainMenu>().ConnectMenu();
//                    break;
//                case 2:
//                    mainObjTf.GetComponent<Inventory>().OnItemDetailInfo();
//                    break;
//                case 3:
//                    mainObjTf.GetComponent<Inventory>().SelectInfo();
//                    break;
//                case 4:
//                    uiController = 8;
//                    mainObjTf.GetComponent<ItemCrafting>().OnDetailCrafting();
//                    break;
//                case 6:
//                    quickSlotTf.GetComponent<QuickSlot>().SelectSlot();
//                    break;
//                case 7:
//                    mainObjTf.GetComponent<Inventory>().FunctionDropItem();
//                    break;
//                case 8:
//                    mainObjTf.GetComponent<ItemCrafting>().ConnectCrafting();
//                    break;
//                case 9:
//                    quickSlotTf.GetComponent<QuickSlot>().SelectSlot();
//                    break;
//                default:
//                    break;
//            }
//        }
//        // 뒤로가기 키 입력 값 전달
//        else if (keyType == 1)
//        {
//            switch (uiController)
//            {
//                case 1:
//                    uiController = 0;
//                    mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//                    ExitMenuCheck();
//                    break;
//                case 2:
//                    uiController = 1;
//                    mainObjTf.GetComponent<Inventory>().ExitInventory();
//                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//                    break;
//                case 3:
//                    uiController = 2;
//                    mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
//                    break;
//                case 4:
//                    uiController = 0;
//                    mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
//                    ExitMenuCheck();
//                    break;
//                case 5:
//                    uiController = 1;
//                    mainObjTf.GetComponent<Dictionary>().OffDictionary();
//                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//                    break;
//                case 6:
//                    uiController = 3;
//                    quickSlotTf.GetComponent<QuickSlot>().ConnectInventory();
//                    break;
//                case 7:
//                    uiController = 3;
//                    mainObjTf.GetComponent<Inventory>().ExitDropItem(1);
//                    break;
//                case 8:
//                    uiController = 4;
//                    mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
//                    break;
//                case 9:
//                    uiController = 0;
//                    quickSlotTf.GetComponent<QuickSlot>().SingleCloseQuickSlot();
//                    ExitQuickSlot();
//                    break;
//                default:
//                    break;
//            }
//        }
//    }     // OnOffControl()

//    // 메인 메뉴 버튼을 눌렀을 때 메인 메뉴를 열고, 다른 메뉴들이 열려있으면 모두 닫고 초기화 시키는 함수
//    public void OnMainMenuControl()
//    {
//        switch (uiController)
//        {
//            // 메뉴가 아무것도 안열려 있으면 메뉴 UI 를 연다
//            case 0:
//                mainObjTf.GetComponent<MainMenu>().OnMainMenu();
//                OpenMenuCheck();
//                uiController = 1;
//                OpenMenuCheck(); // <Solbin> 메인메뉴 열릴 때 => UI 상태로 전환 
//                break;
//            // 메뉴가 하나라도 켜져있으면 해당 메뉴를 모두 닫고 초기화 화면으로 돌아감
//            case 1:
//                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 2:
//                mainObjTf.GetComponent<Inventory>().ExitInventory();
//                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 3:
//                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
//                mainObjTf.GetComponent<Inventory>().ExitInventory();
//                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 4:
//                mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 5:
//                mainObjTf.GetComponent<Dictionary>().OffDictionary();
//                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 6:
//                quickSlotTf.GetComponent<QuickSlot>().ConnectInventory();
//                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
//                mainObjTf.GetComponent<Inventory>().ExitInventory();
//                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 7:
//                mainObjTf.GetComponent<Inventory>().ExitDropItem(1);
//                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
//                mainObjTf.GetComponent<Inventory>().ExitInventory();
//                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 8:
//                mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
//                mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
//                ExitMenuCheck();
//                uiController = 0;
//                break;
//            case 9:
//                quickSlotTf.GetComponent<QuickSlot>().SingleCloseQuickSlot();
//                ExitQuickSlot();
//                uiController = 0;
//                break;
//            default:
//                break;
//        }
//    }     // OnMainMenuControl()

//    #region Input 키 입력 값 모음

//    public void AfterWarpExitMap()
//    {
//        mapCameraTf.GetComponent<CameraControl>().ExitCheckPointInfo();
//        mapControllerTf.GetComponent<MapControl>().ExitMap();
//        mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
//        mainObjTf.GetComponent<MainMenu>().OffMainMenu();
//        ExitMenuCheck();
//        uiController = 0;
//    }     // AfterWarpExitMap()

//    // 단일 퀵슬롯이 열릴때 게임 시간 슬로우 효과
//    private void OpenQuickSlot()
//    {
//        // TODO: 슬로우 타임 필요

//        // <Solbin> 메뉴를 열 때 NORMAL 상태라면 UI 상태로 전환
//        if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.NORMAL)
//        {
//            VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.UI);
//        }// <Solbin> UI 상태로 전환 
//    }

//    // 단일 퀵슬롯을 닫았을 때 슬로우 효과 해제
//    private void ExitQuickSlot()
//    {
//        // TODO: 시간 원상 복귀 필요

//        // <Solbin> 메뉴를 닫을 때 UI 상태라면 NORMAL 상태로 전환
//        if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.UI)
//        {
//            VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.NORMAL);
//        }// <Solbin> NORMAL 상태로 전환 
//    }

//    // 어떤 메뉴든 열릴 때 게임 시간 정지
//    private void OpenMenuCheck()
//    {
//        // TODO: 시간 정지 필요 

//        // <Solbin> 메뉴를 열 때 NORMAL 상태라면 UI 상태로 전환
//        if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.NORMAL)
//        {
//            VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.UI);
//        }// <Solbin> UI 상태로 전환 
//    }

//    // 어떤 메뉴든 닫았을 때 게임 시간 재개
//    private void ExitMenuCheck()
//    {
//        // TODO: 시간 원상 복귀 필요

//        // <Solbin> 메뉴를 닫을 때 UI 상태라면 NORMAL 상태로 전환
//        if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.UI)
//        {
//            VRIFStateSystem.Instance.ChangeState(VRIFStateSystem.GameState.NORMAL);
//        }// <Solbin> NORMAL 상태로 전환 
//    }



//    #region LAGACY
//    //* LEGACY CODE
//    //// 인벤토리 창을 열고있는 상태에서만 실행
//    //if (mainObjTf.GetComponent<Inventory>().lookInventory == true)
//    //{
//    //    // 아이템 상세 정보창을 보고있지 않을때 실행
//    //    if (mainObjTf.GetComponent<Inventory>().lookItemDetailInfo == false)
//    //    {
//    //        // 상, 하, 좌, 우 키를 눌렀을 때
//    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().OrderCheck(0);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().OrderCheck(1);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().OrderCheck(2);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().OrderCheck(3);
//    //        }
//    //        // 확인 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.Z))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().OnItemDetailInfo();
//    //        }
//    //        // 뒤로 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.X))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().ExitInventory();
//    //        }
//    //        // 아이템 타입 페이지 업 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.Keypad4))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().PageUp();
//    //        }
//    //        // 아이템 타입 페이지 다운 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.Keypad6))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().PageDown();
//    //        }
//    //        // 아이템 수량 페이지 업 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.Keypad8))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(0);
//    //        }
//    //        // 아이템 수량 페이지 다운 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.Keypad2))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(1);
//    //        }
//    //    }
//    //    // 아이템 상세 정보창을 보고있을 때 실행
//    //    else
//    //    {
//    //        // 상, 하, 좌, 우 키를 눌렀을 때
//    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(0);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(1);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(2);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(3);
//    //        }
//    //        // 뒤로 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.X))
//    //        {
//    //            mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
//    //        }
//    //    }
//    //}
//    //else if (mainObjTf.GetComponent<ItemCrafting>().lookCrafting == true)
//    //{
//    //    // 제작 상세 정보창을 보고있지 않을때 실행
//    //    if (mainObjTf.GetComponent<ItemCrafting>().lookCraftingInfo == false)
//    //    {
//    //        // 상, 하, 좌, 우 키를 눌렀을 때
//    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(0);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(1);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(2);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(3);
//    //        }
//    //        // 확인 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.Z))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().OnDetailCrafting();
//    //        }
//    //        // 뒤로 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.X))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
//    //        }
//    //        // 제작 아이템 타입 페이지 변경 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().PageChange();
//    //        }
//    //    }
//    //    // 제작 상세 정보창을 보고있을 때 실행
//    //    else
//    //    {
//    //        // 상, 하, 좌, 우 키를 눌렀을 때
//    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(0);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(1);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(2);
//    //        }
//    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(3);
//    //        }
//    //        // 뒤로 키를 눌렀을 때
//    //        else if (Input.GetKeyDown(KeyCode.X))
//    //        {
//    //            mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
//    //        }
//    //    }
//    //}
//    //// 아무 UI 창도 열지 않았을 때 실행
//    //else
//    //{
//    //    // 인벤토리 열기 키를 눌렀을 때
//    //    if (Input.GetKeyDown(KeyCode.N))
//    //    {
//    //        mainObjTf.GetComponent<Inventory>().ControlInventory();
//    //    }
//    //    // 크래프팅 열기 키를 눌렀을 때
//    //    else if (Input.GetKeyDown(KeyCode.M))
//    //    {
//    //        mainObjTf.GetComponent<ItemCrafting>().OnCrafting();
//    //    }
//    //}

//    //// Legacy : 인벤토리 아이콘을 클릭했을 때 실행되는 함수
//    //if (Input.GetMouseButtonDown(0))
//    //{
//    //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//    //    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
//    //    {
//    //        // "ItemImg" 태그로 아이콘을 구분함
//    //        if (hitInfo.transform.tag == "ItemImg")
//    //        {
//    //            hitItemSlot = hitInfo.transform.gameObject;

//    //            // 클릭한 인벤토리 아이콘의 활성화 값이 true 면 실행
//    //            if (hitItemSlot.GetComponent<SaveItemInfo>().activeSlotCheck == true)
//    //            {
//    //                hitItemSlot.GetComponent<SaveItemInfo>().LoadNameInfo(out string itemName);
//    //                mainObjTf.GetComponent<Inventory>().OnItemInfo(itemName);
//    //            }
//    //            // 클릭한 인벤토리 아이콘의 활성화 값이 false 면 실행
//    //            else
//    //            {
//    //                mainObjTf.GetComponent<Inventory>().EmptyInfo();
//    //            }
//    //        }
//    //        // "CraftingImg" 태그로 아이콘을 구분함
//    //        else if (hitInfo.transform.tag == "CraftingImg")
//    //        {
//    //            hitItemSlot = hitInfo.transform.gameObject;

//    //            hitItemSlot.GetComponent<SaveCraftingInfo>().LoadInfo(out string slotName);
//    //            mainObjTf.GetComponent<ItemCrafting>().CraftingCheck(slotName);
//    //        }
//    //    }
//    //}
//    #endregion LAGACY

//    // <Solbin>
//    private void ClearInputDelay() { inputDelay = false; }
//    // <Solbin> ===

//}     // Update()

//#endregion Input 키 입력 값 모음




//// ---------------------------- 안녕 ?


////using UnityEngine;
////using BNG;

////public class UIController : MonoBehaviour
////{
////    #region 변수 설정

////    // 퀵슬롯 트랜스폼
////    public Transform quickSlotTf;
////    // 지도 카메라 트랜스폼
////    public Transform mapCameraTf;
////    // 맵 컨트롤러 트랜스폼
////    public Transform mapControllerTf;
////    // NPC 컨트롤러 트랜스폼
////    public Transform npcControllerTf;

////    // 활성화 상태 UI 창 타입 구분값
////    // 0 : UI 창 모두 꺼짐
////    // 1 : 메인 메뉴 창
////    // 2 : 인벤토리 창
////    // 3 : 인벤토리 세부 창
////    // 4 : 제작 창
////    // 5 : 도감 창
////    // 6 : 인벤토리 퀵슬롯 창
////    // 7 : 아이템 버리기 창
////    // 8 : 제작 세부 창
////    // 9 : 사용 퀵슬롯 창
////    // 10 : 지도 창
////    // 11 : 지도 선택 창
////    // 12 : NPC 대화 상태
////    public int uiController { get; set; } = default;

////    // 플레이어 트랜스폼
////    private Transform mainObjTf = default;

////    #endregion 변수 설정

////    // <Solbin> VRIF Action
////    private VRIFAction vrifAction = default;
////    // <Solbin> VRIF State System
////    [SerializeField] private VRIFStateSystem vrifStateSystem = default;
////    // <Solbin> VRIF Time System
////    [SerializeField] private VRIFTimeSystem vrifTimeSystem = default;
////    // <Solbin> UI 조이스틱 입력 기준값
////    private float joystickInput = 0.95f;
////    // <Solbin> Input Delay를 위한 bool 값
////    private bool inputDelay = false;

////    void Awake()
////    {
////        uiController = 0;
////    }     // Awake()

////    void Start()
////    {
////        mainObjTf = GetComponent<Transform>().transform;
////    }     // Start()

////    void Update()
////    {
////        UpdateFunction();
////    }     // Update()

////    #region UpdateFunction()

////    private void StopMovePlayer()
////    {
////        mainObjTf.GetComponent<Rigidbody>().velocity = Vector3.zero;
////    }     // StopMovePlayer()

////    // Update 마다 조건에 맞으면 실행되는 함수
////    private void UpdateFunction()
////    {
////        if (uiController == 0)
////        {
////            mainObjTf.GetComponent<Meen_MovePlayer>().UpdateFunction();

////            if (npcControllerTf.GetComponent<NPCController>().onNavigationCheck > 0)
////            {
////                npcControllerTf.GetComponent<NPCController>().ConnectUpdateFunction();
////            }
////        }
////        else if (uiController == 10) { mapCameraTf.GetComponent<CameraControl>().UpdateFunction(); }

////        // 모든 상, 하, 좌, 우 기본 키보드 키 입력 값
////        if (Input.GetKeyDown(KeyCode.UpArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().y >= joystickInput)
////        {
////            // <Solbin> GetKey 같은 느낌이라 너무 예민하다고 느껴진다. 수정 필요
////            DirectionControl(0);
////        }
////        else if (Input.GetKeyDown(KeyCode.DownArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().y <= -joystickInput)
////        {
////            DirectionControl(1);
////        }
////        else if (Input.GetKeyDown(KeyCode.LeftArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().x <= -joystickInput)
////        {
////            DirectionControl(2);
////        }
////        else if (Input.GetKeyDown(KeyCode.RightArrow) || vrifAction.Player.LeftController.ReadValue<Vector2>().x >= joystickInput)
////        {
////            DirectionControl(3);
////        }
////        // 모든 상, 하, 좌, 우 키보드 키 입력 종료 값
////        else if (Input.GetKeyUp(KeyCode.UpArrow))
////        {
////            KeyUpDirectionControl(0);
////        }
////        else if (Input.GetKeyUp(KeyCode.DownArrow))
////        {
////            KeyUpDirectionControl(1);
////        }
////        else if (Input.GetKeyUp(KeyCode.LeftArrow))
////        {
////            KeyUpDirectionControl(2);
////        }
////        else if (Input.GetKeyUp(KeyCode.RightArrow))
////        {
////            KeyUpDirectionControl(3);
////        }
////        // <Solbin> 메뉴 진입
////        else if (Input.GetKeyDown(KeyCode.M) || vrifAction.Player.UI_Menu.triggered) // <Solbin> Menu Enter
////        {
////            OnMainMenuControl();
////        }
////        // 모든 진입 키 입력 값
////        else if (Input.GetKeyDown(KeyCode.Z) || vrifAction.Player.UI_Click.triggered) // <Solbin> Menu Select
////        {
////            OnOffControl(0);
////        }
////        // 모든 뒤로가기 키 입력 값
////        else if (Input.GetKeyDown(KeyCode.X) || vrifAction.Player.UI_Exit.triggered) // <Solbin> Exit Menu
////        {
////            OnOffControl(1);

////            //if (vrifStateSystem.gameState == VRIFStateSystem.GameState.UI) // <Solbin> UI 상태일때
////            //{
////            //    OnOffControl(1);
////            //    vrifStateSystem.ChangeState(VRIFStateSystem.GameState.NORMAL); // <Solbin> NORMAL 상태로 전환
////            //}
////        }
////        else if (Input.GetKeyDown(KeyCode.C))
////        {
////            npcControllerTf.GetComponent<NPCController>().InputController();
////        }
////        else if (Input.GetKeyDown(KeyCode.T))
////        {
////            mapCameraTf.GetComponent<CameraControl>().FunctionCheckPoint();
////        }
////        // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 값
////        else if (Input.GetKeyDown(KeyCode.Keypad8) || vrifAction.Player.RightController.ReadValue<Vector2>().y >= joystickInput)
////        {
////            RightDirectionControl(0);
////        }
////        else if (Input.GetKeyDown(KeyCode.Keypad2) || vrifAction.Player.RightController.ReadValue<Vector2>().y <= -joystickInput)
////        {
////            RightDirectionControl(1);
////        }
////        else if (Input.GetKeyDown(KeyCode.Keypad4) || vrifAction.Player.RightController.ReadValue<Vector2>().x <= -joystickInput)
////        {
////            RightDirectionControl(2);
////        }
////        else if (Input.GetKeyDown(KeyCode.Keypad6) || vrifAction.Player.RightController.ReadValue<Vector2>().x >= joystickInput)
////        {
////            RightDirectionControl(3);
////        }
////        // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 종료 값
////        else if (Input.GetKeyUp(KeyCode.Keypad8))
////        {
////            KeyUpRightDirectionControl(0);
////        }
////        else if (Input.GetKeyUp(KeyCode.Keypad2))
////        {
////            KeyUpRightDirectionControl(1);
////        }
////        else if (Input.GetKeyUp(KeyCode.Keypad4))
////        {
////            KeyUpRightDirectionControl(2);
////        }
////        else if (Input.GetKeyUp(KeyCode.Keypad6))
////        {
////            KeyUpRightDirectionControl(3);
////        }
////        else if (Input.GetKeyDown(KeyCode.P) && uiController == 0)
////        {
////            uiController = 4;
////            mainObjTf.GetComponent<ItemCrafting>().OnCrafting();
////            OpenMenuCheck();
////        }
////        //* Test : 아래의 해당 키를 누르면 인벤토리에 아이템 추가 기능
////        else if (Input.GetKeyDown(KeyCode.Alpha1))
////        {
////            mainObjTf.GetComponent<Inventory>().AddInventory("딸기", 1);
////        }
////        else if (Input.GetKeyDown(KeyCode.Alpha2))
////        {
////            mainObjTf.GetComponent<Inventory>().AddInventory("우유", 1);
////        }
////        else if (Input.GetKeyDown(KeyCode.Alpha3))
////        {
////            mainObjTf.GetComponent<Inventory>().AddInventory("고기", 1);
////        }
////        else if (Input.GetKeyDown(KeyCode.Alpha4))
////        {
////            mainObjTf.GetComponent<Inventory>().AddInventory("송이 버섯", 1);
////        }

////        // <Solbin> 지나치게 예민한 입력값을 막기 위함
////        if (inputDelay) { Invoke("ClearInputDelay", 0.5f); }
////        // <Solbin> ===
////    }     // UpdateFunction()

////    #endregion UpdateFunction()

////    private void OnEnable()
////    {
////        vrifAction = new VRIFAction();
////        vrifAction.Enable();
////    }

////    private void OnDisable()
////    {
////        vrifAction?.Disable();
////    }

////    #region DirectionControl()

////    // 모든 UI 에서 방향키 입력을 받아 전달하는 함수
////    public void DirectionControl(int arrowType)
////    {
////        switch (uiController)
////        {
////            case 1:
////                mainObjTf.GetComponent<MainMenu>().ChangeOrder(arrowType);
////                break;
////            case 2:
////                mainObjTf.GetComponent<Inventory>().OrderCheck(arrowType);
////                break;
////            case 3:
////                mainObjTf.GetComponent<Inventory>().DetailOrderCheck(arrowType);
////                break;
////            case 4:
////                mainObjTf.GetComponent<ItemCrafting>().OrderCheck(arrowType);
////                break;
////            case 5:
////                mainObjTf.GetComponent<Dictionary>().ChangeOrder(arrowType);
////                break;
////            case 6:
////                quickSlotTf.GetComponent<QuickSlot>().DirectionControl(arrowType);
////                break;
////            case 7:
////                mainObjTf.GetComponent<Inventory>().DropItemOrderCheck(arrowType);
////                break;
////            case 8:
////                mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(arrowType);
////                break;
////            case 9:
////                quickSlotTf.GetComponent<QuickSlot>().DirectionControl(arrowType);
////                break;
////            case 10:
////                mapCameraTf.GetComponent<CameraControl>().MoveMap(arrowType);
////                break;
////            case 11:
////                mapCameraTf.GetComponent<CameraControl>().ChangeCheckPointButton(arrowType);
////                break;
////            default:
////                break;
////        }
////    }     // DirectionControl()

////    // 모든 상, 하, 좌, 우 키보드 키 입력 종료 값을 받아 전달하는 함수
////    public void KeyUpDirectionControl(int arrowType)
////    {
////        switch (uiController)
////        {
////            case 10:
////                mapCameraTf.GetComponent<CameraControl>().StopMoveMap(arrowType);
////                break;
////            default:
////                break;
////        }
////    }     // KeyUpDirectionControl()

////    // 모든 UI 에서 다른 방향키 입력을 받아 전달하는 함수
////    public void RightDirectionControl(int arrowType)
////    {
////        switch (uiController)
////        {
////            case 2:
////                if (arrowType == 2 || arrowType == 3)
////                {
////                    mainObjTf.GetComponent<Inventory>().ChangePage(arrowType);
////                }
////                else if (arrowType == 0 || arrowType == 1)
////                {
////                    mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(arrowType);
////                }
////                break;
////            case 4:
////                mainObjTf.GetComponent<ItemCrafting>().ControlDetailOrder(arrowType);
////                break;
////            case 5:
////                mainObjTf.GetComponent<Dictionary>().OtherChangeOrder(arrowType);
////                break;
////            case 10:
////                if (arrowType == 0 || arrowType == 1) { mapCameraTf.GetComponent<CameraControl>().CheckInCamera(arrowType); }
////                break;
////            default:
////                break;
////        }
////    }     // RightDirectionControl()

////    // 모든 두번째 상, 하, 좌, 우 키보드 키 입력 종료 값을 받아 전달하는 함수
////    public void KeyUpRightDirectionControl(int arrowType)
////    {
////        switch (uiController)
////        {
////            case 10:
////                if (arrowType == 0 || arrowType == 1) { mapCameraTf.GetComponent<CameraControl>().CheckOutCamera(arrowType); }
////                break;
////            default:
////                break;
////        }
////    }     // KeyUpRightDirectionControl()

////    #endregion DirectionControl()

////    #region OnOffControl()

////    // 모든 UI 에서 진입, 뒤로가기 입력을 받아 전달하는 함수
////    public void OnOffControl(int keyType)
////    {
////        // 진입 키 입력 값 전달
////        if (keyType == 0)
////        {
////            switch (uiController)
////            {
////                // 아무 UI 도 안켜져 있을 때 A 버튼을 누르면 퀵슬롯이 켜짐
////                case 0:
////                    StopMovePlayer();
////                    uiController = 9;
////                    quickSlotTf.GetComponent<QuickSlot>().SingleOpenQuickSlot();
////                    OpenQuickSlot();
////                    break;
////                case 1:
////                    mainObjTf.GetComponent<MainMenu>().ConnectMenu();
////                    break;
////                case 2:
////                    mainObjTf.GetComponent<Inventory>().OnItemDetailInfo();
////                    break;
////                case 3:
////                    mainObjTf.GetComponent<Inventory>().SelectInfo();
////                    break;
////                case 4:
////                    uiController = 8;
////                    mainObjTf.GetComponent<ItemCrafting>().OnDetailCrafting();
////                    break;
////                case 6:
////                    quickSlotTf.GetComponent<QuickSlot>().SelectSlot();
////                    break;
////                case 7:
////                    mainObjTf.GetComponent<Inventory>().FunctionDropItem();
////                    break;
////                case 8:
////                    mainObjTf.GetComponent<ItemCrafting>().ConnectCrafting();
////                    break;
////                case 9:
////                    quickSlotTf.GetComponent<QuickSlot>().SelectSlot();
////                    break;
////                case 10:
////                    mapCameraTf.GetComponent<CameraControl>().SelectCheckPoint();
////                    break;
////                case 11:
////                    mapCameraTf.GetComponent<CameraControl>().SelectCheckPointButton();
////                    break;
////                default:
////                    break;
////            }
////        }
////        // 뒤로가기 키 입력 값 전달
////        else if (keyType == 1)
////        {
////            switch (uiController)
////            {
////                case 1:
////                    uiController = 0;
////                    mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                    ExitMenuCheck();
////                    break;
////                case 2:
////                    uiController = 1;
////                    mainObjTf.GetComponent<Inventory>().ExitInventory();
////                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                    break;
////                case 3:
////                    uiController = 2;
////                    mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
////                    break;
////                case 4:
////                    uiController = 0;
////                    mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
////                    ExitMenuCheck();
////                    break;
////                case 5:
////                    uiController = 1;
////                    mainObjTf.GetComponent<Dictionary>().OffDictionary();
////                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                    break;
////                case 6:
////                    uiController = 3;
////                    quickSlotTf.GetComponent<QuickSlot>().ConnectInventory();
////                    break;
////                case 7:
////                    uiController = 3;
////                    mainObjTf.GetComponent<Inventory>().ExitDropItem(1);
////                    break;
////                case 8:
////                    uiController = 4;
////                    mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
////                    break;
////                case 9:
////                    uiController = 0;
////                    quickSlotTf.GetComponent<QuickSlot>().SingleCloseQuickSlot();
////                    ExitQuickSlot();
////                    break;
////                case 10:
////                    uiController = 1;
////                    mapControllerTf.GetComponent<MapControl>().ExitMap();
////                    mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                    break;
////                case 11:
////                    mapCameraTf.GetComponent<CameraControl>().ExitCheckPointInfo();
////                    break;
////                default:
////                    break;
////            }
////        }
////    }     // OnOffControl()

////    // 메인 메뉴 버튼을 눌렀을 때 메인 메뉴를 열고, 다른 메뉴들이 열려있으면 모두 닫고 초기화 시키는 함수
////    public void OnMainMenuControl()
////    {
////        switch (uiController)
////        {
////            // 메뉴가 아무것도 안열려 있으면 메뉴 UI 를 연다
////            case 0:
////                StopMovePlayer();
////                uiController = 1;
////                mainObjTf.GetComponent<MainMenu>().OnMainMenu();
////                OpenMenuCheck(); // <Solbin> 메인메뉴 열릴 때 => UI 상태로 전환 
////                break;
////            // 메뉴가 하나라도 켜져있으면 해당 메뉴를 모두 닫고 초기화 화면으로 돌아감
////            case 1:
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 2:
////                mainObjTf.GetComponent<Inventory>().ExitInventory();
////                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 3:
////                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
////                mainObjTf.GetComponent<Inventory>().ExitInventory();
////                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 4:
////                mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 5:
////                mainObjTf.GetComponent<Dictionary>().OffDictionary();
////                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 6:
////                quickSlotTf.GetComponent<QuickSlot>().ConnectInventory();
////                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
////                mainObjTf.GetComponent<Inventory>().ExitInventory();
////                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 7:
////                mainObjTf.GetComponent<Inventory>().ExitDropItem(1);
////                mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
////                mainObjTf.GetComponent<Inventory>().ExitInventory();
////                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 8:
////                mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
////                mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 9:
////                quickSlotTf.GetComponent<QuickSlot>().SingleCloseQuickSlot();
////                ExitQuickSlot();
////                uiController = 0;
////                break;
////            case 10:
////                mapControllerTf.GetComponent<MapControl>().ExitMap();
////                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            case 11:
////                mapCameraTf.GetComponent<CameraControl>().ExitCheckPointInfo();
////                mapControllerTf.GetComponent<MapControl>().ExitMap();
////                mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
////                mainObjTf.GetComponent<MainMenu>().OffMainMenu();
////                ExitMenuCheck();
////                uiController = 0;
////                break;
////            default:
////                break;
////        }
////    }     // OnMainMenuControl()

//    // 지도에서 워프 기능 사용 후 지도 기능을 비활성화 하는 함수


////    #endregion OnOffControl()

////    #region UI 상태에 따라 게임 슬로우 상태, 정지 상태 변경 기능

////    // 단일 퀵슬롯이 열릴때 게임 시간 슬로우 효과
////    private void OpenQuickSlot()
////    {
////        // TODO: 타임 슬로우 효과 연결
////    }

////    // 단일 퀵슬롯을 닫았을 때 슬로우 효과 해제
////    private void ExitQuickSlot()
////    {
////        // TODO: 타임 슬로우 효과 해제 연결
////    }

////    // 어떤 메뉴든 열릴 때 게임 시간 정지
////    private void OpenMenuCheck()
////    {
////        // TODO: 시간 정지 필요 

////        // <Solbin> 메뉴를 열 때 NORMAL 상태라면 UI 상태로 전환
////        if (VRIFStateSystem.gameState == VRIFStateSystem.GameState.NORMAL)
////        {
////            VRIFStateSystem.gameState = VRIFStateSystem.GameState.UI;
////        }// <Solbin> UI 상태로 전환 

////        Debug.Log("UI 상태");
////    }

////    // 어떤 메뉴든 닫았을 때 게임 시간 재개
////    public void ExitMenuCheck()
////    {
////        // TODO: 시간 원상 복귀 필요

////        // <Solbin> 메뉴를 닫을 때 UI 상태라면 NORMAL 상태로 전환
////        if (VRIFStateSystem.gameState == VRIFStateSystem.GameState.UI)
////        {
////            VRIFStateSystem.gameState = VRIFStateSystem.GameState.NORMAL;
////        }// <Solbin> NORMAL 상태로 전환 

////        Debug.Log("NORMAL 상태");
////    }

////    #endregion UI 상태에 따라 게임 슬로우 상태, 정지 상태 변경 기능

////    #region LAGACY
////    //* LEGACY CODE
////    //// 인벤토리 창을 열고있는 상태에서만 실행
////    //if (mainObjTf.GetComponent<Inventory>().lookInventory == true)
////    //{
////    //    // 아이템 상세 정보창을 보고있지 않을때 실행
////    //    if (mainObjTf.GetComponent<Inventory>().lookItemDetailInfo == false)
////    //    {
////    //        // 상, 하, 좌, 우 키를 눌렀을 때
////    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().OrderCheck(0);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().OrderCheck(1);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().OrderCheck(2);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().OrderCheck(3);
////    //        }
////    //        // 확인 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.Z))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().OnItemDetailInfo();
////    //        }
////    //        // 뒤로 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.X))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().ExitInventory();
////    //        }
////    //        // 아이템 타입 페이지 업 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.Keypad4))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().PageUp();
////    //        }
////    //        // 아이템 타입 페이지 다운 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.Keypad6))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().PageDown();
////    //        }
////    //        // 아이템 수량 페이지 업 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.Keypad8))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(0);
////    //        }
////    //        // 아이템 수량 페이지 다운 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.Keypad2))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().ChangeItemGroupPage(1);
////    //        }
////    //    }
////    //    // 아이템 상세 정보창을 보고있을 때 실행
////    //    else
////    //    {
////    //        // 상, 하, 좌, 우 키를 눌렀을 때
////    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(0);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(1);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(2);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().DetailOrderCheck(3);
////    //        }
////    //        // 뒤로 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.X))
////    //        {
////    //            mainObjTf.GetComponent<Inventory>().OffItemDetailInfo();
////    //        }
////    //    }
////    //}
////    //else if (mainObjTf.GetComponent<ItemCrafting>().lookCrafting == true)
////    //{
////    //    // 제작 상세 정보창을 보고있지 않을때 실행
////    //    if (mainObjTf.GetComponent<ItemCrafting>().lookCraftingInfo == false)
////    //    {
////    //        // 상, 하, 좌, 우 키를 눌렀을 때
////    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(0);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(1);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(2);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().OrderCheck(3);
////    //        }
////    //        // 확인 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.Z))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().OnDetailCrafting();
////    //        }
////    //        // 뒤로 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.X))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().ExitCrafting();
////    //        }
////    //        // 제작 아이템 타입 페이지 변경 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().PageChange();
////    //        }
////    //    }
////    //    // 제작 상세 정보창을 보고있을 때 실행
////    //    else
////    //    {
////    //        // 상, 하, 좌, 우 키를 눌렀을 때
////    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(0);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.RightArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(1);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.UpArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(2);
////    //        }
////    //        else if (Input.GetKeyDown(KeyCode.DownArrow))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().DetailOrderCheck(3);
////    //        }
////    //        // 뒤로 키를 눌렀을 때
////    //        else if (Input.GetKeyDown(KeyCode.X))
////    //        {
////    //            mainObjTf.GetComponent<ItemCrafting>().ExitDetailCrafting();
////    //        }
////    //    }
////    //}
////    //// 아무 UI 창도 열지 않았을 때 실행
////    //else
////    //{
////    //    // 인벤토리 열기 키를 눌렀을 때
////    //    if (Input.GetKeyDown(KeyCode.N))
////    //    {
////    //        mainObjTf.GetComponent<Inventory>().ControlInventory();
////    //    }
////    //    // 크래프팅 열기 키를 눌렀을 때
////    //    else if (Input.GetKeyDown(KeyCode.M))
////    //    {
////    //        mainObjTf.GetComponent<ItemCrafting>().OnCrafting();
////    //    }
////    //}

////    //// Legacy : 인벤토리 아이콘을 클릭했을 때 실행되는 함수
////    //if (Input.GetMouseButtonDown(0))
////    //{
////    //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

////    //    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
////    //    {
////    //        // "ItemImg" 태그로 아이콘을 구분함
////    //        if (hitInfo.transform.tag == "ItemImg")
////    //        {
////    //            hitItemSlot = hitInfo.transform.gameObject;

////    //            // 클릭한 인벤토리 아이콘의 활성화 값이 true 면 실행
////    //            if (hitItemSlot.GetComponent<SaveItemInfo>().activeSlotCheck == true)
////    //            {
////    //                hitItemSlot.GetComponent<SaveItemInfo>().LoadNameInfo(out string itemName);
////    //                mainObjTf.GetComponent<Inventory>().OnItemInfo(itemName);
////    //            }
////    //            // 클릭한 인벤토리 아이콘의 활성화 값이 false 면 실행
////    //            else
////    //            {
////    //                mainObjTf.GetComponent<Inventory>().EmptyInfo();
////    //            }
////    //        }
////    //        // "CraftingImg" 태그로 아이콘을 구분함
////    //        else if (hitInfo.transform.tag == "CraftingImg")
////    //        {
////    //            hitItemSlot = hitInfo.transform.gameObject;

////    //            hitItemSlot.GetComponent<SaveCraftingInfo>().LoadInfo(out string slotName);
////    //            mainObjTf.GetComponent<ItemCrafting>().CraftingCheck(slotName);
////    //        }
////    //    }
////    //}
////    #endregion LAGACY

////    // <Solbin>
////    private void ClearInputDelay() { inputDelay = false; }
////    // <Solbin> ===

////}     // Update()

#endregion Lagacy Function