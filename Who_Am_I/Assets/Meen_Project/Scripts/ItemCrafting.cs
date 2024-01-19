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
    // 제작 완료 창 오브젝트
    public GameObject completeCrafting;
    // 제작 창의 제작 가능한 슬롯 목록
    public GameObject[] craftingSlot = new GameObject[5];
    // 제작 상세 창 오브젝트
    public GameObject craftingInfoObj;
    // 제작 리스트 목록 이미지 오브젝트
    public GameObject[] craftingSlotObj = new GameObject[5];
    // 제작 상세 창의 버튼 오브젝트
    public GameObject[] detailSlotObj = new GameObject[4];
    // 제작 가능한 슬롯의 아이템 이미지 값
    public Image[] itemImages = new Image[5];
    // 제작 가능한 슬롯의 아이템 이름 값
    public Text[] itemTexts = new Text[5];
    // 제작 상세 창의 제작 아이템 이미지 목록
    public Image[] craftingImages = new Image[3];
    // 제작 완료 창 아이템 이미지
    public Image completeItemImage;
    // 제작 상세 창의 제작 아이템 갯수 확인 이미지 목록
    public Text[] craftingStacks = new Text[3];
    // 제작 리스트들의 제작 불가능 표시 텍스트
    public Text[] impossibleTexts = new Text[5];
    // 제작 완료 창 아이템 정보 창 텍스트
    public Text[] completeCraftingText = new Text[3];
    // 제작 상세 정보에서 선택한 아이템 이름 텍스트
    public Text itemNameText;
    // 제작 상세 창에서 선택한 아이템 정보 텍스트
    public Text itemInfoText;
    // 제작 메뉴얼 오브젝트
    public GameObject craftingManualObj;

    // 제작 창을 보고있는 상태인지 체크
    public bool lookCrafting { get; set; } = false;
    // 제작 상세 창을 보고있는 상태인지 체크
    public bool lookCraftingInfo { get; set; } = false;

    // 아이템 데이터 그룹 메인 오브젝트 트랜스폼
    private Transform mainObjTf = default;

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
    // 제작되는 아이템 효과 타입
    private int craftingEffectType = default;
    // 제작되는 아이템 일반 효과들 수치
    private int[] craftingEffectCount = new int[2];
    // 제작되는 아이템 특수 효과 정보
    private string craftingEffectInfo = default;
    // 재료 아이템들의 정보
    private string[] stuffItemInfo = new string[2];
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
    // 제작 목록에서의 현재 리스트 선택 확인 값
    private int order = default;
    // 제작 상세 목록에서의 현재 선택한 버튼 확인 값
    private int detailOrder = default;
    // 제작 가능 목록의 아이템 이름 값
    private string[,] craftingStr = new string[5, 5];
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
    // 제작 창의 페이지 값
    private int page = default;
    // 제작 창의 페이지 최대 수
    private int maxPage = default;

    #endregion 변수 설정

    void Awake()
    {
        craftingLength = 0;
        resultStuffCount = 0;
        completeItemType = 0;
        craftingStack = 0;
        order = 0;
        detailOrder = 0;
        craftingStackNum = 0;
        page = 0;
        maxPage = 4;
        craftingEffectType = 0;
    }     // Awake()

    void Start()
    {
        mainObjTf = GetComponent<Transform>().transform;

        craftingStr[0, 0] = "고구마 라떼";
        craftingStr[0, 1] = "대추차";
        craftingStr[0, 2] = "딸기 우유";
        craftingStr[0, 3] = "땅콩버터";
        craftingStr[0, 4] = "마라탕";

        craftingStr[1, 0] = "모듬 닭꼬치";
        craftingStr[1, 1] = "미완성 불도장";
        craftingStr[1, 2] = "불도장";
        craftingStr[1, 3] = "블루베리 아이스크림";
        craftingStr[1, 4] = "생선조림";

        craftingStr[2, 0] = "스테이크";
        craftingStr[2, 1] = "어묵";
        craftingStr[2, 2] = "야자 주스";
        craftingStr[2, 3] = "야채 샐러드";
        craftingStr[2, 4] = "피쉬 앤 칩스";

        craftingStr[3, 0] = "핫초코";
        craftingStr[3, 1] = "토마토 소스";
        craftingStr[3, 2] = "반죽";
        craftingStr[3, 3] = "피자";
        craftingStr[3, 4] = "유자차";

        craftingStr[4, 0] = "송이 불고기";

        for (int i = 0; i < 5; i++)
        {
            craftingSlotColor[i] = craftingSlotObj[i].GetComponent<Image>();

            if (i != 0)
            {
                craftingStr[4, i] = "Empty";
            }
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
        if (arrowType == 0 || arrowType == 1)
        {
            PageChange(arrowType);
        }

        //if (arrowType == 2 || arrowType == 3)
        //{
        //    PageChange();
        //}
    }     // ControlDetailOrder()

    // 제작 페이지 변경 키 누를 시 페이지 변경 함수
    public void PageChange(int arrowType)
    {
        switch (arrowType)
        {
            case 0:
                if (page == 0)
                {
                    page = maxPage;
                }
                else
                {
                    page -= 1;
                }
                break;
            case 1:
                if (page >= maxPage)
                {
                    page = 0;
                }
                else
                {
                    page += 1;
                }
                break;
            default:
                break;
        }

        //// 크래프팅 페이지 변경
        //if (page == 0)
        //{
        //    page = 1;
        //}
        //else
        //{
        //    page = 0;
        //}

        //// 페이지 버튼 알파값 변경
        //for (int i = 0; i < 2; i++)
        //{
        //    Color color = pageImages[i].color;

        //    if (page == i)
        //    {
        //        color.a = 1f;
        //        pageImages[i].color = color;
        //    }
        //    else
        //    {
        //        color.a = 0.2f;
        //        pageImages[i].color = color;
        //    }
        //}

        CheckPage();
    }     // PageChange()

    // 제작 페이지 초기값을 설정
    public void CheckPage()
    {
        CleanSlot();

        // 현재 지정된 페이지의 제작 리스트 이름과 제작 아이템 이미지를 모두 출력
        for (int i = 0; i < 5; i++)
        {
            if (craftingStr[page, i] != "Empty")
            {
                ItemManager.instance.ItemImage(craftingStr[page, i], 3, out craftingSlotImageNum[i]);
                itemImages[i].sprite = ItemManager.instance.itemImages[craftingSlotImageNum[i]].sprite;
                itemTexts[i].text = string.Format("{0}", craftingStr[page, i]);
                craftingSlot[i].GetComponent<SaveCraftingInfo>().SaveInfo(craftingStr[page, i]);

                // 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수를 실행함
                CheckDisposableCrafting(craftingStr[page, i], out bool check);

                // 일회성 제작을 완료한 상태의 레시피 리스트면
                if (check == true)
                {
                    // 제작 불가능 이미지를 활성화함
                    impossibleTexts[i].gameObject.SetActive(true);
                }
            }
        }

        ShowOrder();
    }     // CheckPage()

    // 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수
    private bool CheckDisposableCrafting(string itemName, out bool check)
    {
        // 아이템 매니저에서 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수를 실행함
        ItemManager.instance.LoadCheckDisposableCrafting(itemName, out bool checkImpossible);

        check = checkImpossible;

        return check;
    }     // CheckDisposableCrafting()

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
            impossibleTexts[i].gameObject.SetActive(false);
        }
    }     // CleanSlot()

    // 제작 창 열기
    public void OnCrafting()
    {
        if (lookCrafting == false)
        {
            mainScreen.SetActive(true);
            crafting.SetActive(true);
            craftingManualObj.SetActive(true);
        }

        lookCrafting = true;

        // 크래프팅에 현재 선택되어 있는 슬롯의 색을 밝게 변경
        newColor = new Color32(255, 255, 255, 255);
        craftingSlotColor[order].color = newColor;

        CheckPage();
    }     // OnCrafting()

    // 제작 창 닫기
    public void ExitCrafting()
    {
        // 크래프팅에 이전에 선택되어 있던 슬롯의 색을 어둡게 변경
        currentColor = new Color32(155, 155, 155, 255);
        craftingSlotColor[order].color = currentColor;

        lookCrafting = false;
        page = 0;
        order = 0;
        detailOrder = 0;

        CleanSlot();

        craftingManualObj.SetActive(false);
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
            // 아이템 매니저에서 일회성 제작 레시피인지 체크하는 함수를 실행함
            ItemManager.instance.LoadCheckDisposableType(craftingName, out bool checkItem);
            // 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수를 실행함
            CheckDisposableCrafting(craftingName, out bool check);

            // 일회성 제작 레시피거나, 일회성 제작을 완료한 상태의 레시피면 기능을 스킵함
            if (check == true || checkItem == true) { return; }

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
            // 아이템 매니저에서 일회성 제작 레시피인지 체크하는 함수를 실행함
            ItemManager.instance.LoadCheckDisposableType(craftingName, out bool checkItem);
            // 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수를 실행함
            CheckDisposableCrafting(craftingName, out bool check);

            // 일회성 제작 레시피거나, 일회성 제작을 완료한 상태의 레시피면 기능을 스킵함
            if (check == true || checkItem == true) { return; }

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
        // 재료 아이템을 선택중이면 해당 재료 아이템의 이름과 정보를 출력함
        if (detailOrder == 0)
        {
            itemNameText.text = string.Format("{0}", stuffName[0]);
            itemInfoText.text = string.Format("{0}", stuffItemInfo[0]);
        }
        // 재료 아이템을 선택중이면 해당 재료 아이템의 이름과 정보를 출력함
        else if (detailOrder == 1)
        {
            itemNameText.text = string.Format("{0}", stuffName[1]);
            itemInfoText.text = string.Format("{0}", stuffItemInfo[1]);
        }
        // 제작 아이템을 선택중이면 해당 제작 아이템의 이름을 출력함
        else if (detailOrder == 2)
        {
            itemNameText.text = string.Format("{0}", craftingName);
            // 제작 아이템을 선택중이면 해당 제작 아이템의 사용 효과 구분에 따라 다른 효과 정보를 출력함
            switch (craftingEffectType)
            {
                // 제작 아이템의 효과 타입이 0 이면 일반 효과만 출력함
                case 0:
                    if (craftingEffectCount[1] > 0)
                    {
                        itemInfoText.text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 + {1}", 
                            craftingEffectCount[0], craftingEffectCount[1]);
                    }
                    else if (craftingEffectCount[1] < 0)
                    {
                        itemInfoText.text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 {1}", 
                            craftingEffectCount[0], craftingEffectCount[1]);
                    }
                    break;
                // 제작 아이템의 효과 타입이 1 이면 일반 효과와 특수 효과를 출력함
                case 1:
                    if (craftingEffectCount[1] > 0)
                    {
                        itemInfoText.text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 + {1}, {2}", 
                            craftingEffectCount[0], craftingEffectCount[1], craftingEffectInfo);
                    }
                    else if (craftingEffectCount[1] < 0)
                    {
                        itemInfoText.text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 {1}, {2}", 
                            craftingEffectCount[0], craftingEffectCount[1], craftingEffectInfo);
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            itemNameText.text = string.Format(" ");
            itemInfoText.text = string.Format(" ");
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
                craftingStacks[j].text = string.Format("{0} / {1}", stuffNowStack[j], stuffStackNum[j]);
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

            DetailOrderUpDown();
        }
    }     // OnDetailCrafting()

    // 크래프팅 상세 정보창 나가기 함수
    public void ExitDetailCrafting()
    {
        // 크래프팅에 이전에 선택되어 있던 슬롯의 색을 어둡게 변경
        currentColor = new Color32(155, 155, 155, 255);
        detailSlotColor[detailOrder].color = currentColor;

        itemNameText.text = string.Format(" ");
        itemInfoText.text = string.Format(" ");
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

        ItemManager.instance.CraftingEffectType(craftingName, out craftingEffectType);
        ItemManager.instance.CraftingEffectCount(craftingName, 0, out craftingEffectCount[0]);
        ItemManager.instance.CraftingEffectCount(craftingName, 1, out craftingEffectCount[1]);

        if (craftingEffectType == 1) { ItemManager.instance.CraftingEffectInfo(craftingName, out craftingEffectInfo); }

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
            ItemManager.instance.CraftingStuffInfo(stuffName[i], out stuffItemInfo[i]);
        }
        //* 제작에 필요한 재료들의 정보를 순차적으로 불러옴

        for (int j = 0; j < craftingLength + 1; j++)
        {
            if (j != 2)
            {
                // 0, 1 번째 순서에는 재료 아이템 정보
                craftingImages[j].sprite = ItemManager.instance.itemImages[stuffImageNum[j]].sprite;
                craftingStacks[j].text = string.Format("{0} / {1}", stuffNowStack[j], stuffStackNum[j]);
            }
            else
            {
                // 2 번째 순서에는 제작 아이템 정보
                craftingImages[j].sprite = ItemManager.instance.itemImages[craftingImageNum].sprite;

                // 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수를 실행함
                CheckDisposableCrafting(craftingName, out bool check);
                
                // 일회성 제작을 완료하지 않은 상태면 제작되는 숫자를 표시함
                if (check == false)
                {
                    craftingStacks[j].text = string.Format("{0}", craftingStack);
                }
                // 일회성 제작을 완료한 상태면 "X" 문자를 표시함
                else
                {
                    craftingStacks[j].text = string.Format("X");
                }
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
        // 일회성 제작 레시피인지 체크하고, 일회성 제작을 완료한 상태인지 체크하는 함수를 실행함
        CheckDisposableCrafting(craftingName, out bool check);

        // 일회성 제작을 완료한 레시피로 제작 버튼을 누르면 제작이 불가능하도록 함
        if (check == true) { return; }
        // 재료 아이템이 충분하지 않으면 실행하지 않음
        else if (detailOrder == 3 && resultStuffCheck == true)
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
        // 아이템 매니저에서 일회성 제작 레시피인지 체크하는 함수
        ItemManager.instance.LoadCheckDisposableType(craftingName, out bool checkItem);
        // 일회성 제작 레시피면
        if (checkItem == true)
        {
            // 아이템 매니저에서 일회성 제작을 완료한 상태로 변경하는 함수를 실행함
            ItemManager.instance.ChangeDiaposableCrafting(craftingName);
        }

        QuestManager_Jun.instance.CheckClear(craftingName);
        Debug.LogFormat("{0} : {1} 개 제작 완료", craftingName, craftingStackNum);

        CheckPage();
        ExitDetailCrafting();
        PrintCompleteCrafting(checkItem);
    }     // Crafting()

    // 제작 완료 후 제작 아이템 확인 팝업창이 활성화 되는 함수
    private void PrintCompleteCrafting(bool oneTimeCheck)
    {
        // UIController 값을 제작 아이템 확인 팝업창을 보고있는 상태로 변경
        mainObjTf.GetComponent<UIController>().uiController = 13;
        
        // 제작 아이템 확인 팝업창을 활성화함
        completeCrafting.gameObject.SetActive(true);
        // 제작 아이템 확인 팝업창에 제작 완료된 아이템 이미지를 출력함
        completeItemImage.sprite = ItemManager.instance.itemImages[craftingImageNum].sprite;
        // 제작 아이템 확인 팝업창에 제작 완료된 아이템 이름을 출력함
        completeCraftingText[0].text = string.Format("[ {0} ]", craftingName);

        // 제작 아이템 확인 팝업창에 제작 완료된 아이템의 사용 효과 구분에 따라 다른 효과 정보를 출력함
        switch (craftingEffectType)
        {
            // 제작 아이템의 효과 타입이 0 이면 일반 효과만 출력함
            case 0:
                if (craftingEffectCount[1] > 0)
                {
                    completeCraftingText[1].text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 + {1}", 
                        craftingEffectCount[0], craftingEffectCount[1]);
                }
                else if (craftingEffectCount[1] < 0)
                {
                    completeCraftingText[1].text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 {1}", 
                        craftingEffectCount[0], craftingEffectCount[1]);
                }
                break;
            // 제작 아이템의 효과 타입이 1 이면 일반 효과와 특수 효과를 출력함
            case 1:
                if (craftingEffectCount[1] > 0)
                {
                    completeCraftingText[1].text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 + {1}, {2}", 
                        craftingEffectCount[0], craftingEffectCount[1], craftingEffectInfo);
                }
                else if (craftingEffectCount[1] < 0)
                {
                    completeCraftingText[1].text = string.Format("사용 효과 : 포만감 + {0}, 응가 게이지 {1}, {2}", 
                        craftingEffectCount[0], craftingEffectCount[1], craftingEffectInfo);
                }
                break;
            default:
                break;
        }

        // 만약 일회성 제작 레시피를 제작한 상태면 제작 불가능 안내문을 활성화함
        if (oneTimeCheck == true) { completeCraftingText[2].gameObject.SetActive(true); }

    }     // PrintCompleteCrafting()

    // 제작 아이템 확인 팝업창을 비활성화 하는 함수
    public void ExitCompleteCrafting()
    {
        // 제작 아이템 확인 팝업창에 제작 완료된 아이템 이미지를 초기화함
        completeItemImage.sprite = ItemManager.instance.itemImages[0].sprite;
        // 제작 아이템 확인 팝업창에 제작 완료된 아이템 이름을 초기화함
        completeCraftingText[0].text = string.Format(" ");
        // 제작 아이템 확인 팝업창에 제작 완료된 아이템 효과를 초기화함
        completeCraftingText[1].text = string.Format(" ");
        // 제작 불가능 안내문을 비활성화함
        completeCraftingText[2].gameObject.SetActive(false);
        // 제작 아이템 확인 팝업창을 비활성화함
        completeCrafting.gameObject.SetActive(false);
    }     // ExitCompleteCrafting()

    #endregion 크래프팅 상세 정보 기능
}