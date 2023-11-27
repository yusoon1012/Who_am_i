using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region 변수 설정

    // 검은색 바탕화면 스크린 오브젝트
    public GameObject menuScreen;
    // 메인 메뉴 묶음 오브젝트
    public GameObject mainMenu;
    // 메인 메뉴의 각 메뉴 이미지 오브젝트
    public GameObject[] menuImage = new GameObject[8];
    // 선택된 메뉴 표시 오브젝트
    public GameObject[] selectMenu = new GameObject[8];

    // 플레이어 트랜스폼
    private Transform playerTf = default;

    // 각 메뉴 선택 확인값
    private int order = default;

    #endregion 변수 설정

    void Awake()
    {
        playerTf = GetComponent<Transform>().transform;

        order = 0;
    }     // Awake()

    // 메인 메뉴를 활성화 하는 함수
    public void OnMainMenu()
    {
        menuScreen.SetActive(true);
        mainMenu.SetActive(true);
        selectMenu[order].SetActive(true);

        for (int i = 0; i < 8; i++)
        {
            menuImage[i].SetActive(true);
        }
    }     // OnMainMenu()

    // 메인 메뉴를 비활성화 하는 함수
    public void OffMainMenu()
    {
        for (int i = 0; i < 8; i++)
        {
            menuImage[i].SetActive(false);
        }

        selectMenu[order].SetActive(false);
        menuScreen.SetActive(false);
        mainMenu.SetActive(false);
    }     // OffMainMenu()

    // 메인 메뉴에서 다른 메뉴로 넘어가는 함수
    public void ConnectMenu()
    {
        if (order == 1)
        {
            playerTf.GetComponent<UIController>().uiController = 2;
            menuImage[6].SetActive(false);
            menuImage[7].SetActive(false);
            playerTf.GetComponent<Inventory>().ControlInventory();
        }
    }     // ConnectMenu()

    // 다른 메뉴에서 메인 메뉴로 넘어오는 함수
    public void DisconnectMenu()
    {
        menuImage[6].SetActive(true);
        menuImage[7].SetActive(true);
    }     // DisconnectMenu()

    // 메인 메뉴에서 메뉴 선택을 변경하는 함수
    public void ChangeOrder(int arrowType)
    {
        selectMenu[order].SetActive(false);

        switch (arrowType)
        {
            case 0:
                if (order <= 2)
                {
                    order = 6;
                }
                else if (order >= 3 && order <= 5)
                {
                    order = 7;
                }
                else if (order == 6)
                {
                    order = 1;
                }
                else if (order == 7)
                {
                    order = 4;
                }
                break;
            case 1:
                if (order <= 2)
                {
                    order = 6;
                }
                else if (order >= 3 && order <= 5)
                {
                    order = 7;
                }
                else if (order == 6)
                {
                    order = 1;
                }
                else if (order == 7)
                {
                    order = 4;
                }
                break;
            case 2:
                if (order == 0)
                {
                    order = 7;
                }
                else
                {
                    order -= 1;
                }
                break;
            case 3:
                if (order == 7)
                {
                    order = 0;
                }
                else
                {
                    order += 1;
                }
                break;
            default:
                break;
        }

        selectMenu[order].SetActive(true);
    }     // ChangeOrder()
}
