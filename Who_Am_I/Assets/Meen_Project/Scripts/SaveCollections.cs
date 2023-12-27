using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCollections : MonoBehaviour
{
    // 컬렉션 정보 트랜스폼
    private Transform collectionInfoTf = default;
    // 컬렉션 타이틀들의 달성 여부 체크
    private bool[] collectionGroupCheck = new bool[10];
    // 컬렉션 타이틀을 달성하기 위한 조건 아이템들 목록
    private string[,] collectionItemCheck = new string[10, 4];
    // 컬렉션 타이틀을 달성하기 위한 조건 아이템들 갯수
    private int[] collectionItemMaxNum = new int[10];
    // 컬렉션 타이틀의 이름 정보 목록
    private string[] titleName = new string[10];
    // 컬렉션 타이틀의 정보 목록
    private string[] titleInfo = new string[10];
    // 컬렉션 타이틀의 효과 정보 목록
    private string[] titleEffect = new string[10];
    
    // 컬렉션 타이틀을 달성하기 위한 조건 아이템들 달성 여부 체크 딕셔너리
    Dictionary<string, bool> collectionItemDic = new Dictionary<string, bool>();

    void Start()
    {
        collectionInfoTf = GetComponent<Transform>().transform;

        collectionItemDic.Add("고기", false);
        collectionItemDic.Add("딸기", false);
        collectionItemDic.Add("우유", false);

        collectionItemCheck[0, 0] = "고기";
        collectionItemCheck[0, 1] = "딸기";
        collectionItemCheck[0, 2] = "우유";

        collectionItemMaxNum[0] = 3;

        titleName[0] = "아이템 수집가";
        titleInfo[0] = "당신은 모든 아이템을 수집하고 싶은 도전 의식이 있습니다!";
        titleEffect[0] = "포만감 최대치 증가";
    }     // Start()

    // 아이템을 획득할 때 마다 컬렉션을 목록을 체크하여 컬렉션에 등록되어 있지 않으면 등록하는 함수
    public void CheckCollection(string itemName, int itemNum)
    {
        // 컬렉션 타이틀을 달성하기 위한 조건 아이템 달성 여부 체크 딕셔너리에 아이템 이름을 참조하여 달성 여부가 False 값이면
        if (collectionItemDic.ContainsKey(itemName))
        {
            // 딕셔너리 Value 값을 True 로 바꿔 달성으로 변경
            collectionItemDic[itemName] = true;

            // 컬렉션 타이틀 달성 여부를 체크하여 달성되어 있지 않은 상태면 실행
            if (collectionGroupCheck[itemNum] == false)
            {
                // 컬렉션 타이틀 달성 여부를 체크하는 함수를 실행함
                CheckCollectionGroup(itemNum);
            }
            
            Debug.LogFormat("컬렉션 아이템 추가됨 : {0}", itemName);
        }
    }     // CheckCollection()

    // 컬렉션 타이틀 달성에 필요한 아이템들을 체크하여 타이틀 달성 조건을 만족하면 타이틀을 활성화 하는 함수
    private void CheckCollectionGroup(int itemNum)
    {
        // 타이틀 달성에 필요한 아이템들을 체크하기 위한 int 값
        int checkCount = 0;

        // 타이틀 달성에 필요한 아이템들의 숫자만큼 증가 실행
        for (int i = 0; i < collectionItemMaxNum[itemNum]; i++)
        {
            string itemName = collectionItemCheck[itemNum, i];

            // 타이틀 달성에 필요한 아이템들 달성 여부 체크 딕셔너리에서 달성 여부 값을 가져옴
            if (collectionItemDic.ContainsKey(itemName))
            {
                bool collectionCheck = collectionItemDic[itemName];

                // 해당 아이템이 달성이 완료된 상태면
                if (collectionCheck == true)
                {
                    // 카운트를 1 증가시킴
                    checkCount += 1;
                }
            }
        }

        // 타이틀 달성에 필요한 아이템들의 달성 완료 숫자가 달성에 필요한 아이템들의 조건 숫자와 같으면 실행
        if (checkCount >= collectionItemMaxNum[itemNum])
        {
            // 해당 컬렉션 타이들의 달성을 완료 상태로 변경
            collectionGroupCheck[itemNum] = true;

            Debug.LogFormat("컬렉션 {0} 그룹 완성!", itemNum);
        }
    }     // CheckCollectionGroup()

    // 컬렉션 타이틀 달성 여부 값을 체크하여 내보내는 함수
    public bool ReturnTitleCollections(int count, out bool titleCheck)
    {
        titleCheck = collectionGroupCheck[count];

        return titleCheck;
    }     // ReturnTitleCollections()

    // 컬렉션 타이틀 달성에 필요한 아이템의 달성 여부 값을 체크하여 내보내는 함수
    public bool ReturnCollectionItems(string itemName, out bool collectionCheck)
    {
        if (collectionItemDic.ContainsKey(itemName))
        {
            collectionCheck = collectionItemDic[itemName];
        }
        else
        {
            collectionCheck = false;
        }

        return collectionCheck;
    }     // ReturnCollectionItems()

    // 컬렉션 타이들의 정보를 내보내는 함수
    public string ReturnTitleInfo(int count, int type, out string titleInfo_)
    {
        switch (type)
        {
            case 0:
                // 참조된 값이 0 이면 타이틀의 이름 정보를 내보냄
                titleInfo_ = titleName[count];
                break;
            case 1:
                // 참조된 값이 1 이면 타이틀의 정보를 내보냄
                titleInfo_ = titleInfo[count];
                break;
            case 2:
                // 참조된 값이 2 면 타이틀의 효과 정보를 내보냄
                titleInfo_ = titleEffect[count];
                break;
            default:
                titleInfo_ = null;
                break;
        }

        return titleInfo_;
    }     // ReturnTitleInfo()
}
