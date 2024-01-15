using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateItemData : MonoBehaviour
{
    ItemsMain itemsInfo = new ItemsMain();

    public void AddItemData()
    {
        #region 재료 아이템 데이터 생성

        ItemManager.instance.itemDataBase.Add("카카오", new Items001());
        ItemManager.instance.itemDataBase.Add("야자 열매", new Items002());
        ItemManager.instance.itemDataBase.Add("은행", new Items003());
        ItemManager.instance.itemDataBase.Add("양배추", new Items004());
        ItemManager.instance.itemDataBase.Add("감자", new Items005());
        ItemManager.instance.itemDataBase.Add("고구마", new Items006());
        ItemManager.instance.itemDataBase.Add("무", new Items007());
        ItemManager.instance.itemDataBase.Add("송이 버섯", new Items008());
        ItemManager.instance.itemDataBase.Add("땅콩", new Items009());
        ItemManager.instance.itemDataBase.Add("죽순", new Items010());
        ItemManager.instance.itemDataBase.Add("유자", new Items011());
        ItemManager.instance.itemDataBase.Add("대추", new Items012());
        ItemManager.instance.itemDataBase.Add("꿀", new Items013());
        ItemManager.instance.itemDataBase.Add("미완성 불도장", new Items014());
        ItemManager.instance.itemDataBase.Add("땅콩버터", new Items015());
        ItemManager.instance.itemDataBase.Add("토마토 소스", new Items016());
        ItemManager.instance.itemDataBase.Add("반죽", new Items017());
        ItemManager.instance.itemDataBase.Add("고기", new Items018());
        ItemManager.instance.itemDataBase.Add("생선", new Items019());
        ItemManager.instance.itemDataBase.Add("달걀", new Items020());

        ReadStuffCSVFiles();

        #endregion 재료 아이템 데이터 생성

        #region 음식 아이템 데이터 생성

        ItemManager.instance.itemDataBase.Add("딸기", new Items021());
        ItemManager.instance.itemDataBase.Add("토마토", new Items022());
        ItemManager.instance.itemDataBase.Add("옥수수", new Items023());
        ItemManager.instance.itemDataBase.Add("블루베리", new Items024());
        ItemManager.instance.itemDataBase.Add("우유", new Items025());
        ItemManager.instance.itemDataBase.Add("딸기 우유", new Items026());
        ItemManager.instance.itemDataBase.Add("블루베리 아이스크림", new Items027());
        ItemManager.instance.itemDataBase.Add("생선조림", new Items028());
        ItemManager.instance.itemDataBase.Add("스테이크", new Items029());
        ItemManager.instance.itemDataBase.Add("고구마 라떼", new Items030());
        ItemManager.instance.itemDataBase.Add("어묵", new Items031());
        ItemManager.instance.itemDataBase.Add("피쉬 앤 칩스", new Items032());
        ItemManager.instance.itemDataBase.Add("야채 샐러드", new Items033());
        ItemManager.instance.itemDataBase.Add("야자 주스", new Items034());
        ItemManager.instance.itemDataBase.Add("모듬 닭꼬치", new Items035());
        ItemManager.instance.itemDataBase.Add("핫초코", new Items036());
        ItemManager.instance.itemDataBase.Add("피자", new Items037());
        ItemManager.instance.itemDataBase.Add("마라탕", new Items038());
        ItemManager.instance.itemDataBase.Add("유자차", new Items039());
        ItemManager.instance.itemDataBase.Add("대추차", new Items040());
        ItemManager.instance.itemDataBase.Add("송이 불고기", new Items041());
        ItemManager.instance.itemDataBase.Add("불도장", new Items042());

        ReadFoodCSVFiles();

        #endregion 음식 아이템 데이터 생성

        #region 도구 아이템 데이터 생성

        ItemManager.instance.itemDataBase.Add("사다리", new Items043());
        ItemManager.instance.itemDataBase.Add("장갑", new Items044());
        ItemManager.instance.itemDataBase.Add("황금 장갑", new Items045());
        ItemManager.instance.itemDataBase.Add("삽", new Items046());
        ItemManager.instance.itemDataBase.Add("황금 삽", new Items047());
        ItemManager.instance.itemDataBase.Add("너프건", new Items048());
        ItemManager.instance.itemDataBase.Add("황금 너프건", new Items049());
        ItemManager.instance.itemDataBase.Add("곤충 채집망", new Items050());
        ItemManager.instance.itemDataBase.Add("낚시대", new Items051());
        ItemManager.instance.itemDataBase.Add("황금 낚시대", new Items052());

        ReadEquipCSVFiles();

        #endregion 도구 아이템 데이터 생성

    }     // AddItemData()

    private void ReadStuffCSVFiles()
    {
        List<Dictionary<string, object>> stuffsReadTable = LGM_CSVReader.Read("ItemStuffTable");

        for (int i = 0; i < stuffsReadTable.Count; i++)
        {
            int id_ = (int)stuffsReadTable[i]["ID"];
            string name_ = stuffsReadTable[i]["Name"].ToString();
            int getType_ = (int)stuffsReadTable[i]["Type"];
            int hp_ = (int)stuffsReadTable[i]["HP"];
            float rangeRec_ = (int)stuffsReadTable[i]["RangeRec"];
            float respawn_ = (int)stuffsReadTable[i]["Respawn"];
            int rarity_ = (int)stuffsReadTable[i]["Rarity"];
            string info_ = stuffsReadTable[i]["Description"].ToString();
            string getMap_ = stuffsReadTable[i]["GetIt"].ToString();
            string note_ = stuffsReadTable[i]["Note"].ToString();
            int imageNum_ = (int)stuffsReadTable[i]["Image"];
            string englishName_ = stuffsReadTable[i]["English"].ToString();

            if (ItemManager.instance.itemDataBase.ContainsKey(name_))
            {
                itemsInfo = ItemManager.instance.itemDataBase[name_];

                itemsInfo.itemID = id_;
                itemsInfo.itemName = name_;
                itemsInfo.getType = getType_;
                itemsInfo.hp = hp_;
                itemsInfo.rangeRec = rangeRec_;
                itemsInfo.respawn = respawn_;
                itemsInfo.rarity = rarity_;
                itemsInfo.itemInfo = info_;
                itemsInfo.getMap = getMap_;
                itemsInfo.note = note_;
                itemsInfo.itemImageNum = imageNum_;
                itemsInfo.itemEnglishName = englishName_;
            }
        }
    }     // ReadNameCSVFiles()

    private void ReadFoodCSVFiles()
    {
        List<Dictionary<string, object>> foodsReadTable = LGM_CSVReader.Read("ItemUseTable");

        for (int i = 0; i < foodsReadTable.Count; i++)
        {
            int id_ = (int)foodsReadTable[i]["ID"];
            string name_ = foodsReadTable[i]["Name"].ToString();
            int cookType_ = (int)foodsReadTable[i]["Type"];
            int satietyGauge_ = (int)foodsReadTable[i]["SatietyGauge"];
            int pooGauge_ = (int)foodsReadTable[i]["PooGauge"];
            int rarity_ = (int)foodsReadTable[i]["Rarity"];
            float durationTime_ = (int)foodsReadTable[i]["Duration"];
            string info_ = foodsReadTable[i]["Description"].ToString();
            string getMap_ = foodsReadTable[i]["GetIt"].ToString();
            float coolTime_ = (int)foodsReadTable[i]["CoolTime"];
            string note_ = foodsReadTable[i]["Note"].ToString();
            int imageNum_ = (int)foodsReadTable[i]["Image"];
            int utile_ = (int)foodsReadTable[i]["Utile"];
            string englishName_ = foodsReadTable[i]["English"].ToString();

            if (ItemManager.instance.itemDataBase.ContainsKey(name_))
            {
                itemsInfo = ItemManager.instance.itemDataBase[name_];

                itemsInfo.itemID = id_;
                itemsInfo.itemName = name_;
                itemsInfo.cookType = cookType_;
                itemsInfo.satietyGauge = satietyGauge_;
                itemsInfo.pooGauge = pooGauge_;
                itemsInfo.rarity = rarity_;
                itemsInfo.durationTime = durationTime_;
                itemsInfo.itemInfo = info_;
                itemsInfo.getMap = getMap_;
                itemsInfo.coolTime = coolTime_;
                itemsInfo.note = note_;
                itemsInfo.itemImageNum = imageNum_;
                itemsInfo.utile = utile_;
                itemsInfo.itemEnglishName = englishName_;
            }
        }
    }     // ReadFoodCSVFiles()

    private void ReadEquipCSVFiles()
    {
        List<Dictionary<string, object>> equipsReadTable = LGM_CSVReader.Read("ItemEquipTable");

        for (int i = 0; i < equipsReadTable.Count; i++)
        {
            int id_ = (int)equipsReadTable[i]["ID"];
            string name_ = equipsReadTable[i]["Name"].ToString();
            string info_ = equipsReadTable[i]["Description"].ToString();
            string getMap_ = equipsReadTable[i]["GetIt"].ToString();
            string infinity_ = equipsReadTable[i]["Infinity"].ToString();
            string itemHint_ = equipsReadTable[i]["Note"].ToString();
            string englishName_ = equipsReadTable[i]["English"].ToString();
            int imageNum_ = (int)equipsReadTable[i]["Image"];

            if (ItemManager.instance.itemDataBase.ContainsKey(name_))
            {
                itemsInfo = ItemManager.instance.itemDataBase[name_];

                itemsInfo.itemID = id_;
                itemsInfo.itemName = name_;
                itemsInfo.itemInfo = info_;
                itemsInfo.getMap = getMap_;
                if (infinity_ == "FALSE")
                {
                    itemsInfo.infinity = false;
                }
                else if (infinity_ == "TRUE")
                {
                    itemsInfo.infinity = true;
                }

                itemsInfo.itemHint = itemHint_;
                itemsInfo.itemEnglishName = englishName_;
                itemsInfo.itemImageNum = imageNum_;
            }
        }
    }     // ReadEquipCSVFiles()
}
