using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dictionary : MonoBehaviour
{
    #region 변수 설정

    // 도감 창 오브젝트
    public GameObject dicObj;
    // 도감 상세 정보 창 오브젝트
    public GameObject dicInfoObj;
    // 아이템 이름 표시 텍스트
    public Text itemNameText;
    // 아이템 정보, 획득 방법 텍스트
    public Text[] itemInfoText = new Text[2];
    // 도감 바깥 이미지 (아이콘 창)
    public Image[] dicOutImage = new Image[18];
    // 도감 안쪽 이미지 (아이템 이미지)
    public Image[] dicInImage = new Image[18];
    // 도감 아이템 타입 페이지 버튼
    public Image[] typeImage = new Image[2];

    // 아이콘 선택 변경 시 컬러 값
    private Color orderColor = default;
    // 아이템 획득, 미획득 구분 컬러 값
    private Color itemColor = default;

    // 플레이어 트랜스폼
    private Transform playerTf = default;

    // 미리 저장한 음식 이름 배열
    private string[,] foodPageItems = new string[10, 18];
    // 미리 저장한 도구 이름 배열
    private string[,] equipmentPageItems = new string[10, 18];
    // 도감에서 선택중인 아이콘 이미지 구분 값
    private int order = default;
    // 도감에서 선택중인 아이템 타입 구분 값
    private int orderType = default;
    // 도감에서 선택중인 칸 페이지 타입 구분 값
    private int orderPage = default;

    #endregion 변수 설정

    void Awake()
    {
        order = 0;
        orderType = 0;
        orderPage = 0;
    }     // Awake()

    void Start()
    {
        playerTf = GetComponent<Transform>().transform;

        SettingDictionary();
    }     // Start()

    // 미리 저장하는 도감의 아이템 이름 배열
    public void SettingDictionary()
    {
        foodPageItems[0, 0] = "딸기";
        foodPageItems[0, 1] = "우유";
        foodPageItems[0, 2] = "딸기 우유";

        for (int i = 3; i < 18; i++)
        {
            foodPageItems[0, i] = "Empty";
        }

        for (int j = 0; j < 18; j++)
        {
            foodPageItems[1, j] = "Empty";
            equipmentPageItems[0, j] = "Empty";
            equipmentPageItems[1, j] = "Empty";
        }
    }     // SettingDictionary()

    // 도감 창을 초기화 시키는 함수
    public void CleanDictionary()
    {
        for (int i = 0; i < 18; i++)
        {
            dicInImage[i].sprite = ItemManager.instance.itemImages[0].sprite;
            itemColor = Color.black;
            dicInImage[i].color = itemColor;
        }
    }     // CleanDictionary()

    // 현재 도감의 페이지의 아이템들의 이미지를 불러오고 획득, 미획득 상태를 체크하는 함수
    public void CheckDictionary()
    {
        ItemsMain itemInfo = new ItemsMain();

        for (int i = 0; i < 18; i++)
        {
            // 현재 보고 있는 페이지가 음식 페이지일 경우
            if (orderType == 0)
            {
                // 아이템 저장 정보가 "Empty" 가 아닐 경우
                if (foodPageItems[orderPage, i] != "Empty")
                {
                    // 아이템 매니저에서 아이템 마다 이미지 번호를 가져옴
                    ItemManager.instance.DictionaryItemImage(foodPageItems[orderPage, i], out itemInfo);
                    // 도감 아이콘마다 불러온 아이템 이미지로 교체
                    dicInImage[i].sprite = ItemManager.instance.itemImages[itemInfo.itemImageNum].sprite;
                    // 아이템 매니저에서 아이템 마다 획득, 미획득 상태값을 가져옴
                    ItemManager.instance.DictionaryCheck(orderType, foodPageItems[orderPage, i], out bool itemCheck);

                    // 아이템을 획득한 상태면
                    if (itemCheck == true)
                    {
                        // 아이템 아이콘을 컬러 상태로 변경
                        itemColor = Color.white;
                        dicInImage[i].color = itemColor;
                    }
                    // 아이템을 미획득한 상태면
                    else
                    {
                        // 아이템 아이콘을 블랙 상태로 변경
                        itemColor = Color.black;
                        dicInImage[i].color = itemColor;
                    }
                }
                // 아이템 저장 정보가 "Empty" 일 경우
                else
                {
                    // 스킵
                    continue;
                }
            }
            // 현재 보고 있는 페이지가 도구 페이지일 경우
            else if (orderType == 1)
            {
                // 아이템 저장 정보가 "Empty" 가 아닐 경우
                if (equipmentPageItems[orderPage, i] != "Empty")
                {
                    // 아이템 매니저에서 아이템 마다 이미지 번호를 가져옴
                    ItemManager.instance.DictionaryItemImage(equipmentPageItems[orderPage, i], out itemInfo);
                    // 도감 아이콘마다 불러온 아이템 이미지로 교체
                    dicInImage[i].sprite = ItemManager.instance.itemImages[itemInfo.itemImageNum].sprite;
                    // 아이템 매니저에서 아이템 마다 획득, 미획득 상태값을 가져옴
                    ItemManager.instance.DictionaryCheck(orderType, equipmentPageItems[orderPage, i], out bool itemCheck);

                    // 아이템을 획득한 상태면
                    if (itemCheck == true)
                    {
                        // 아이템 아이콘을 컬러 상태로 변경
                        itemColor = Color.white;
                        dicInImage[i].color = itemColor;
                    }
                    // 아이템을 미획득한 상태면
                    else
                    {
                        // 아이템 아이콘을 블랙 상태로 변경
                        itemColor = Color.black;
                        dicInImage[i].color = itemColor;
                    }
                }
                // 아이템 저장 정보가 "Empty" 일 경우
                else
                {
                    // 스킵
                    continue;
                }
            }
        }
    }     // CheckDictionary()

    // 도감 선택 아이콘을 변경하는 함수
    public void ChangeOrder(int arrowType)
    {
        // 이전에 선택되고 있던 아이콘 색을 어둡게 설정
        orderColor = new Color32(155, 155, 155, 255);
        dicOutImage[order].color = orderColor;

        // 참조된 칸 이동 값마다 다르게 이동
        switch (arrowType)
        {
            case 0:
                if (order < 6)
                {
                    order += 12;
                }
                else
                {
                    order -= 6;
                }
                break;
            case 1:
                if (order > 11)
                {
                    order -= 12;
                }
                else
                {
                    order += 6;
                }
                break;
            case 2:
                if (order == 0)
                {
                    order = 17;
                }
                else
                {
                    order -= 1;
                }
                break;
            case 3:
                if (order == 17)
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

        // 새롭게 선택된 아이콘 색을 밝게 설정
        orderColor = new Color32(255, 255, 255, 255);
        dicOutImage[order].color = orderColor;

        ShowInfo();
    }     // ChangeOrder()

    // 도감 선택 칸 페이지, 아이템 타입 페이지를 변경하는 함수
    public void OtherChangeOrder(int arrowType)
    {
        CleanDictionary();

        // 참조된 칸 이동 값마다 다르게 이동
        if (arrowType == 0)
        {
            if (orderPage == 0)
            {
                orderPage = 1;
            }
            else
            {
                orderPage -= 1;
            }
        }
        else if (arrowType == 1)
        {
            if (orderPage == 1)
            {
                orderPage = 0;
            }
            else
            {
                orderPage += 1;
            }
        }
        else if (arrowType == 2)
        {
            Color beforeAlphaColor = typeImage[orderType].color;
            beforeAlphaColor.a = 0.2f;
            typeImage[orderType].color = beforeAlphaColor;

            if (orderType == 0)
            {
                orderType = 1;
            }
            else
            {
                orderType -= 1;
            }

            Color afterAlphaColor = typeImage[orderType].color;
            afterAlphaColor.a = 1f;
            typeImage[orderType].color = afterAlphaColor;

            orderPage = 0;
        }
        else if (arrowType == 3)
        {
            Color beforeAlphaColor = typeImage[orderType].color;
            beforeAlphaColor.a = 0.2f;
            typeImage[orderType].color = beforeAlphaColor;

            if (orderType == 1)
            {
                orderType = 0;
            }
            else
            {
                orderType += 1;
            }

            Color afterAlphaColor = typeImage[orderType].color;
            afterAlphaColor.a = 1f;
            typeImage[orderType].color = afterAlphaColor;

            orderPage = 0;
        }

        CheckDictionary();
        ShowInfo();
    }     // OtherChangeOrder()

    // 도감의 선택한 아이템의 상세 정보를 표시하는 함수
    public void ShowInfo()
    {
        ItemsMain itemInfo = new ItemsMain();

        if (orderType == 0)
        {
            if (foodPageItems[orderPage, order] != "Empty")
            {
                dicInfoObj.SetActive(true);
                ItemManager.instance.DictionaryItemImage(foodPageItems[orderPage, order], out itemInfo);

                itemNameText.text = string.Format("{0}", itemInfo.itemName);
                itemInfoText[0].text = string.Format("{0}", itemInfo.itemInfo);
                itemInfoText[1].text = string.Format("{0}", itemInfo.itemHint);
            }
            else
            {
                itemNameText.text = string.Format(" ");
                itemInfoText[0].text = string.Format(" ");
                itemInfoText[1].text = string.Format(" ");

                dicInfoObj.SetActive(false);
            }
        }
        else if (orderType == 1)
        {
            if (equipmentPageItems[orderPage, order] != "Empty")
            {
                dicInfoObj.SetActive(true);
                ItemManager.instance.DictionaryItemImage(equipmentPageItems[orderPage, order], out itemInfo);

                itemNameText.text = string.Format("{0}", itemInfo.itemName);
                itemInfoText[0].text = string.Format("{0}", itemInfo.itemInfo);
                itemInfoText[1].text = string.Format("{0}", itemInfo.itemHint);
            }
            else
            {
                itemNameText.text = string.Format(" ");
                itemInfoText[0].text = string.Format(" ");
                itemInfoText[1].text = string.Format(" ");

                dicInfoObj.SetActive(false);
            }
        }
    }     // ShowInfo()

    // 도감을 활성화 하는 함수
    public void OnDictionary()
    {
        order = 0;
        orderPage = 0;
        orderType = 0;

        dicObj.SetActive(true);
        dicInfoObj.SetActive(true);
        orderColor = Color.white;
        dicOutImage[order].color = orderColor;

        Color afterAlphaColor = typeImage[orderType].color;
        afterAlphaColor.a = 1f;
        typeImage[orderType].color = afterAlphaColor;

        CheckDictionary();
        ShowInfo();
    }     // OnDictionary()

    // 도감을 비활성화 하는 함수
    public void OffDictionary()
    {
        orderColor = new Color32(155, 155, 155, 255);
        dicOutImage[order].color = orderColor;

        Color beforeAlphaColor = typeImage[orderType].color;
        beforeAlphaColor.a = 0.2f;
        typeImage[orderType].color = beforeAlphaColor;

        dicObj.SetActive(false);
        dicInfoObj.SetActive(false);
    }     // OffDictionary()
}
