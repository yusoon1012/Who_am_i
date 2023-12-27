using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class Dictionary : MonoBehaviour
{
    #region 변수 설정

    // 도감 창 오브젝트
    public GameObject dicObj;
    // 도감 상세 정보 창 오브젝트
    public GameObject dicInfoObj;

    public Transform collectionInfoTf;
    // 아이템 이름 표시 텍스트
    public Text itemNameText;
    // 아이템 정보, 획득 방법 텍스트
    public Text[] itemInfoText = new Text[2];
    // 도감 바깥 이미지 (아이콘 창)
    public Image[] dicOutImage = new Image[18];
    // 도감 안쪽 이미지 (아이템 이미지)
    public Image[] dicInImage = new Image[18];
    // 도감 아이템 타입 페이지 버튼
    public Image[] typeImage = new Image[4];

    // 아이콘 선택 변경 시 컬러 값
    private Color orderColor = default;
    // 아이템 획득, 미획득 구분 컬러 값
    private Color itemColor = default;

    // 플레이어 트랜스폼
    private Transform playerTf = default;

    // 미리 저장한 재료 이름 배열
    private string[,] stuffPageItems = new string[2, 18];
    // 미리 저장한 음식 이름 배열
    private string[,] foodPageItems = new string[2, 18];
    // 미리 저장한 도구 이름 배열
    private string[,] equipmentPageItems = new string[1, 18];
    // 미리 저장한 콜렉션 이름 배열
    private string[,] collectionPageItems = new string[4, 18];
    // 도감에서 선택중인 아이콘 이미지 구분 값
    private int order = default;
    // 도감에서 선택중인 아이템 타입 구분 값
    private int orderType = default;
    // 도감에서 선택중인 칸 페이지 타입 구분 값
    private int orderPage = default;
    // 각 아이템 타입 최대 페이지 값
    private int[] maxPage = new int[4];

    #endregion 변수 설정

    void Awake()
    {
        maxPage[0] = 1;
        maxPage[1] = 1;
        maxPage[2] = 0;
        maxPage[3] = 3;
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
        stuffPageItems[0, 0] = "고기";
        stuffPageItems[0, 1] = "송이 버섯";

        foodPageItems[0, 0] = "딸기";
        foodPageItems[0, 1] = "우유";
        foodPageItems[0, 2] = "딸기 우유";
        foodPageItems[0, 3] = "송이 불고기";

        //collectionPageItems[0, 2] = "고기";
        //collectionPageItems[0, 3] = "딸기";
        //collectionPageItems[0, 4] = "우유";

        for (int i = 4; i < 18; i++)
        {
            foodPageItems[0, i] = "Empty";
        }

        for (int k = 2; k < 18; k++)
        {
            stuffPageItems[0, k] = "Empty";
        }

        //for (int k = 5; k < 18; k++)
        //{
        //    collectionPageItems[0, k] = "Empty";
        //}

        for (int j = 0; j < 18; j++)
        {
            stuffPageItems[1, j] = "Empty";
            foodPageItems[1, j] = "Empty";
            equipmentPageItems[0, j] = "Empty";
            collectionPageItems[0, j] = "Empty";
            collectionPageItems[1, j] = "Empty";
            collectionPageItems[2, j] = "Empty";
            collectionPageItems[3, j] = "Empty";
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
            // 현재 보고 있는 페이지가 재료 페이지일 경우
            if (orderType == 0)
            {
                // 아이템 저장 정보가 "Empty" 가 아닐 경우
                if (stuffPageItems[orderPage, i] != "Empty")
                {
                    // 아이템 매니저에서 아이템 마다 이미지 번호를 가져옴
                    ItemManager.instance.DictionaryItemImage(stuffPageItems[orderPage, i], out itemInfo);
                    // 도감 아이콘마다 불러온 아이템 이미지로 교체
                    dicInImage[i].sprite = ItemManager.instance.itemImages[itemInfo.itemImageNum].sprite;
                    // 아이템 매니저에서 아이템 마다 획득, 미획득 상태값을 가져옴
                    ItemManager.instance.DictionaryCheck(orderType, stuffPageItems[orderPage, i], out bool itemCheck);

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
            // 현재 보고 있는 페이지가 음식 페이지일 경우
            else if (orderType == 1)
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
            else if (orderType == 2)
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
            else if (orderType == 3)
            {
                if (i == 1 || i == 7 || i == 13) { continue; }

                if (i == 0 || i == 6 || i == 12)
                {
                    PrintTitleCollection(i);
                }
                else
                {
                    PrintCollectionItem(i);
                }
            }
        }
    }     // CheckDictionary()

    private void PrintTitleCollection(int count)
    {
        int pageCount = orderPage * 3;
        bool titleCheck = false;

        if (pageCount < 10)
        {
            dicInImage[count].sprite = ItemManager.instance.itemImages[5].sprite;
        }
        else
        {
            return;
        }

        switch (count)
        {
            case 0:
                collectionInfoTf.GetComponent<SaveCollections>().ReturnTitleCollections(0 + pageCount, out titleCheck);
                break;
            case 6:
                collectionInfoTf.GetComponent<SaveCollections>().ReturnTitleCollections(1 + pageCount, out titleCheck);
                break;
            case 12:
                collectionInfoTf.GetComponent<SaveCollections>().ReturnTitleCollections(2 + pageCount, out titleCheck);
                break;
            default:
                break;
        }

        if (titleCheck == true)
        {
            // 아이템 아이콘을 컬러 상태로 변경
            itemColor = Color.white;
            dicInImage[count].color = itemColor;
        }
        else
        {
            // 아이템 아이콘을 블랙 상태로 변경
            itemColor = Color.black;
            dicInImage[count].color = itemColor;
        }
    }     // PrintTitleCollection()

    private void PrintCollectionItem(int count)
    {
        ItemsMain itemInfo = new ItemsMain();
        bool collectionCheck = false;

        // 아이템 저장 정보가 "Empty" 가 아닐 경우
        if (collectionPageItems[orderPage, count] != "Empty")
        {
            // 아이템 매니저에서 아이템 마다 이미지 번호를 가져옴
            ItemManager.instance.DictionaryItemImage(collectionPageItems[orderPage, count], out itemInfo);
            // 도감 아이콘마다 불러온 아이템 이미지로 교체
            dicInImage[count].sprite = ItemManager.instance.itemImages[itemInfo.itemImageNum].sprite;

            string itemName = itemInfo.itemName;

            collectionInfoTf.GetComponent<SaveCollections>().ReturnCollectionItems(itemName, out collectionCheck);

            // 아이템을 획득한 상태면
            if (collectionCheck == true)
            {
                // 아이템 아이콘을 컬러 상태로 변경
                itemColor = Color.white;
                dicInImage[count].color = itemColor;
            }
            // 아이템을 미획득한 상태면
            else
            {
                // 아이템 아이콘을 블랙 상태로 변경
                itemColor = Color.black;
                dicInImage[count].color = itemColor;
            }
        }
    }     // PrintCollectionItem()

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

        // 현재 보고있는 탭이 콜렉션 탭이면
        if (orderType == 3)
        {
            // 아이템 칸 커서 값이 1, 7, 13 이면
            if (order == 1 || order == 7 || order == 13)
            {
                CollectionChangeOrder(arrowType);
            }
        }

        // 새롭게 선택된 아이콘 색을 밝게 설정
        orderColor = new Color32(255, 255, 255, 255);
        dicOutImage[order].color = orderColor;

        ShowInfo();
    }     // ChangeOrder()

    // 콜렉션 탭에서 커서 값이 비활성화된 칸의 값이면 커서값을 한칸 더 넘기는 함수
    private void CollectionChangeOrder(int arrowType)
    {
        if (arrowType == 0 || arrowType == 2)
        {
            order -= 1;
        }
        else if (arrowType == 1 || arrowType == 3)
        {
            order += 1;
        }
    }     // CollectionChangeOrder()

    // 도감 선택 칸 페이지, 아이템 타입 페이지를 변경하는 함수
    public void OtherChangeOrder(int arrowType)
    {
        CleanDictionary();

        // 참조된 칸 이동 값마다 다르게 이동
        if (arrowType == 0)
        {
            // 도감의 페이지 값을 변경하는 함수를 실행
            CheckChangePage(0);
        }
        else if (arrowType == 1)
        {
            // 도감의 페이지 값을 변경하는 함수를 실행
            CheckChangePage(1);
        }
        else if (arrowType == 2)
        {
            if (orderType == 3) { OffCollectionType(); }
            
            Color beforeAlphaColor = typeImage[orderType].color;
            beforeAlphaColor.a = 0.2f;
            typeImage[orderType].color = beforeAlphaColor;

            if (orderType == 0)
            {
                orderType = 3;
            }
            else
            {
                orderType -= 1;
            }

            Color afterAlphaColor = typeImage[orderType].color;
            afterAlphaColor.a = 1f;
            typeImage[orderType].color = afterAlphaColor;

            orderPage = 0;

            if (orderType == 3) { OnCollectionType(); }
        }
        else if (arrowType == 3)
        {
            if (orderType == 3) { OffCollectionType(); }

            Color beforeAlphaColor = typeImage[orderType].color;
            beforeAlphaColor.a = 0.2f;
            typeImage[orderType].color = beforeAlphaColor;

            if (orderType == 3)
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

            if (orderType == 3) { OnCollectionType(); }
        }

        CheckDictionary();
        ShowInfo();
    }     // OtherChangeOrder()

    // 콜렉션 탭에서 나갈 때 실행되는 함수
    private void OffCollectionType()
    {
        dicOutImage[1].gameObject.SetActive(true);
        dicOutImage[7].gameObject.SetActive(true);
        dicOutImage[13].gameObject.SetActive(true);
    }     // OffCollectionType()

    // 콜렉션 탭으로 변경되었을 때 실행되는 함수
    private void OnCollectionType()
    {
        dicOutImage[1].gameObject.SetActive(false);
        dicOutImage[7].gameObject.SetActive(false);
        dicOutImage[13].gameObject.SetActive(false);

        if (order == 1 || order == 7 || order == 13)
        {
            // 이전에 선택되고 있던 아이콘 색을 어둡게 설정
            orderColor = new Color32(155, 155, 155, 255);
            dicOutImage[order].color = orderColor;

            order -= 1;

            // 새롭게 선택된 아이콘 색을 밝게 설정
            orderColor = new Color32(255, 255, 255, 255);
            dicOutImage[order].color = orderColor;
        }
    }     // OnCollectionType()

    // 도감의 페이지 값을 변경하는 함수
    private void CheckChangePage(int arrowType)
    {
        if (arrowType == 0)
        {
            switch (orderType)
            {
                case 0:
                    if (orderPage == 0)
                    {
                        orderPage = maxPage[0];
                    }
                    else
                    {
                        orderPage -= 1;
                    }
                    break;
                case 1:
                    if (orderPage == 0)
                    {
                        orderPage = maxPage[1];
                    }
                    else
                    {
                        orderPage -= 1;
                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
                //case 3:
                //    if (orderPage == 0)
                //    {
                //        orderPage = maxPage[3];
                //    }
                //    else
                //    {
                //        orderPage -= 1;
                //    }
                //    break;
                default:
                    break;
            }
        }
        else if (arrowType == 1)
        {
            switch (orderType)
            {
                case 0:
                    if (orderPage == maxPage[0])
                    {
                        orderPage = 0;
                    }
                    else
                    {
                        orderPage += 1;
                    }
                    break;
                case 1:
                    if (orderPage == maxPage[1])
                    {
                        orderPage = 0;
                    }
                    else
                    {
                        orderPage += 1;
                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
                //case 3:
                //    if (orderPage == maxPage[3])
                //    {
                //        orderPage = 0;
                //    }
                //    else
                //    {
                //        orderPage += 1;
                //    }
                //    break;
                default:
                    break;
            }
        }
    }     // CheckChangePage()

    // 도감의 선택한 아이템의 상세 정보를 표시하는 함수
    public void ShowInfo()
    {
        ItemsMain itemInfo = new ItemsMain();

        switch (orderType)
        {
            case 0:
                if (stuffPageItems[orderPage, order] != "Empty")
                {
                    dicInfoObj.SetActive(true);
                    ItemManager.instance.DictionaryItemImage(stuffPageItems[orderPage, order], out itemInfo);

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
                break;
            case 1:
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
                break;
            case 2:
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
                break;
            case 3:
                break;
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
        if (orderType == 3)
        {
            dicOutImage[1].gameObject.SetActive(true);
            dicOutImage[7].gameObject.SetActive(true);
            dicOutImage[13].gameObject.SetActive(true);
        }

        orderColor = new Color32(155, 155, 155, 255);
        dicOutImage[order].color = orderColor;

        Color beforeAlphaColor = typeImage[orderType].color;
        beforeAlphaColor.a = 0.2f;
        typeImage[orderType].color = beforeAlphaColor;

        CleanDictionary();

        dicObj.SetActive(false);
        dicInfoObj.SetActive(false);
    }     // OffDictionary()
}
