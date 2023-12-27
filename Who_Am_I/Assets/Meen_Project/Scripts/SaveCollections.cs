using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCollections : MonoBehaviour
{
    private Transform collectionInfoTf = default;

    private bool[] collectionGroupCheck = new bool[10];

    private string[,] collectionItemCheck = new string[10, 4];

    private int[] collectionItemMaxNum = new int[10];

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
    }

    public void CheckCollection(string itemName, int itemNum)
    {
        if (collectionItemDic.ContainsKey(itemName))
        {
            collectionItemDic[itemName] = true;

            if (collectionGroupCheck[itemNum] == false)
            {
                CheckCollectionGroup(itemNum);
            }
            
            Debug.LogFormat("콜렉션 아이템 추가됨 : {0}", itemName);
        }
    }     // CheckCollection()

    private void CheckCollectionGroup(int itemNum)
    {
        int checkCount = 0;

        for (int i = 0; i < collectionItemMaxNum[itemNum]; i++)
        {
            string itemName = collectionItemCheck[itemNum, i];

            if (collectionItemDic.ContainsKey(itemName))
            {
                bool collectionCheck = collectionItemDic[itemName];

                if (collectionCheck == true)
                {
                    checkCount += 1;
                }
            }
        }

        if (checkCount >= collectionItemMaxNum[itemNum])
        {
            collectionGroupCheck[itemNum] = true;

            Debug.LogFormat("콜렉션 {0} 그룹 완성!", itemNum);
        }
    }     // CheckCollectionGroup()

    public bool ReturnTitleCollections(int count, out bool titleCheck)
    {
        titleCheck = collectionGroupCheck[count];

        return titleCheck;
    }     // ReturnTitleCollections()

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
}
