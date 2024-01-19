using System.Collections.Generic;
using UnityEngine;

public class CreateItemData : MonoBehaviour
{
    public Transform mainObjTf;
    public Transform saveCollectionsTf;

    ItemsMain itemsInfo = new ItemsMain();
    CraftingMain craftingInfo = new CraftingMain();
    CollectionsMain collectionInfo = new CollectionsMain();

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

        #region 제작 레시피 데이터 생성

        ItemManager.instance.crafting.Add("고구마 라떼", new Crafting001());
        ItemManager.instance.crafting.Add("대추차", new Crafting002());
        ItemManager.instance.crafting.Add("딸기 우유", new Crafting003());
        ItemManager.instance.crafting.Add("땅콩버터", new Crafting004());
        ItemManager.instance.crafting.Add("마라탕", new Crafting005());
        ItemManager.instance.crafting.Add("모듬 닭꼬치", new Crafting006());
        ItemManager.instance.crafting.Add("미완성 불도장", new Crafting007());
        ItemManager.instance.crafting.Add("불도장", new Crafting008());
        ItemManager.instance.crafting.Add("블루베리 아이스크림", new Crafting009());
        ItemManager.instance.crafting.Add("생선조림", new Crafting010());
        ItemManager.instance.crafting.Add("스테이크", new Crafting011());
        ItemManager.instance.crafting.Add("어묵", new Crafting012());
        ItemManager.instance.crafting.Add("야자 주스", new Crafting013());
        ItemManager.instance.crafting.Add("야채 샐러드", new Crafting014());
        ItemManager.instance.crafting.Add("피쉬 앤 칩스", new Crafting015());
        ItemManager.instance.crafting.Add("핫초코", new Crafting016());
        ItemManager.instance.crafting.Add("토마토 소스", new Crafting017());
        ItemManager.instance.crafting.Add("반죽", new Crafting018());
        ItemManager.instance.crafting.Add("피자", new Crafting019());
        ItemManager.instance.crafting.Add("유자차", new Crafting020());
        ItemManager.instance.crafting.Add("송이 불고기", new Crafting021());

        ReadCraftingCSVFiles();

        #endregion 제작 레시피 데이터 생성

        #region 컬렉션 데이터 생성

        ItemManager.instance.collections.Add("피자", new Collections001());
        ItemManager.instance.collections.Add("유자차", new Collections002());
        ItemManager.instance.collections.Add("송이 불고기", new Collections003());
        ItemManager.instance.collections.Add("꼬치 고기", new Collections004());
        ItemManager.instance.collections.Add("학꽁치", new Collections005());
        ItemManager.instance.collections.Add("조개", new Collections006());
        ItemManager.instance.collections.Add("빙어", new Collections007());
        ItemManager.instance.collections.Add("민어", new Collections008());
        ItemManager.instance.collections.Add("소금쟁이", new Collections009());
        ItemManager.instance.collections.Add("메기", new Collections010());
        ItemManager.instance.collections.Add("산천어", new Collections011());
        ItemManager.instance.collections.Add("가재", new Collections012());
        ItemManager.instance.collections.Add("나리 나비", new Collections013());
        ItemManager.instance.collections.Add("달래 나비", new Collections014());
        ItemManager.instance.collections.Add("하늘 개구리", new Collections015());
        ItemManager.instance.collections.Add("들판 개구리", new Collections016());
        ItemManager.instance.collections.Add("이삭 거미", new Collections017());
        ItemManager.instance.collections.Add("낙엽 거미", new Collections018());
        ItemManager.instance.collections.Add("싸락 사슴벌레", new Collections019());
        ItemManager.instance.collections.Add("어스름 사슴벌레", new Collections020());

        ReadCollectionCSVFiles();

        mainObjTf.GetComponent<Dictionary>().SettingDictionary();

        #endregion 컬렉션 데이터 생성
    }     // AddItemData()

    #region 재료 아이템 데이터 저장

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

            mainObjTf.GetComponent<Dictionary>().SettingStuffDict(name_, i);
        }
    }     // ReadNameCSVFiles()

    #endregion 재료 아이템 데이터 저장

    #region 소비 아이템 데이터 저장

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

            mainObjTf.GetComponent<Dictionary>().SettingFoodDict(name_, i);
        }
    }     // ReadFoodCSVFiles()

    #endregion 소비 아이템 데이터 저장

    #region 도구 아이템 데이터 저장

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

            mainObjTf.GetComponent<Dictionary>().SettingEquipDict(name_, i);
        }
    }     // ReadEquipCSVFiles()

    #endregion 도구 아이템 데이터 저장

    #region 크래프팅 레시피 데이터 저장

    private void ReadCraftingCSVFiles()
    {
        List<Dictionary<string, object>> craftingsReadTable = LGM_CSVReader.Read("ItemCraftingTable");

        for (int i = 0; i < craftingsReadTable.Count; i++)
        {
            int id_ = (int)craftingsReadTable[i]["ID"];
            string name_ = craftingsReadTable[i]["Name"].ToString();
            float coolTime_ = (int)craftingsReadTable[i]["CoolTime"];
            string[] needItem_ = new string[2];
            needItem_[0] = craftingsReadTable[i]["NeedItem1"].ToString();
            needItem_[1] = craftingsReadTable[i]["NeedItem2"].ToString();
            string produce_ = craftingsReadTable[i]["Produce"].ToString();
            int utile_ = (int)craftingsReadTable[i]["Utile"];

            if (ItemManager.instance.crafting.ContainsKey(name_))
            {
                craftingInfo = ItemManager.instance.crafting[name_];

                craftingInfo.craftingID = id_;
                craftingInfo.craftingName = name_;
                craftingInfo.coolTime = coolTime_;
                craftingInfo.stuffName[0] = needItem_[0];
                craftingInfo.stuffName[1] = needItem_[1];
                if (produce_ == "FALSE")
                {
                    craftingInfo.disposableType = false;
                }
                else if (produce_ == "TRUE")
                {
                    craftingInfo.disposableType = true;
                }
                
                craftingInfo.utile = utile_;
                craftingInfo.effectInfo = " ";
                craftingInfo.craftingLength = 2;
                craftingInfo.stuffStack[0] = 1;
                craftingInfo.stuffStack[1] = 1;
                craftingInfo.craftingStack = 1;
                craftingInfo.disposable = false;
            }
        }
    }     // ReadCraftingCSVFiles()

    #endregion 크래프팅 레시피 데이터 저장

    #region 컬렉션 정보 데이터 저장

    private void ReadCollectionCSVFiles()
    {
        List<Dictionary<string, object>> collectionsReadTable = LGM_CSVReader.Read("ItemCollectionTable");

        int checkCount = -1;

        for (int i = 0; i < collectionsReadTable.Count; i++)
        {
            int id_ = (int)collectionsReadTable[i]["ID"];
            string name_ = collectionsReadTable[i]["Name"].ToString();
            int downCount_ = (int)collectionsReadTable[i]["DownCount"];
            int downImage_ = (int)collectionsReadTable[i]["DownImage"];
            string note_ = collectionsReadTable[i]["Note"].ToString();
            string hint_ = collectionsReadTable[i]["Hint"].ToString();
            string downName_ = collectionsReadTable[i]["DownName"].ToString();
            int groupCount_ = (int)collectionsReadTable[i]["GroupCount"];
            int downNum_ = (int)collectionsReadTable[i]["DownNum"];
            int itemType_ = (int)collectionsReadTable[i]["ItemType"];
            string info_ = collectionsReadTable[i]["Description"].ToString();

            if (ItemManager.instance.collections.ContainsKey(downName_))
            {
                collectionInfo = ItemManager.instance.collections[downName_];

                collectionInfo.name = downName_;
                collectionInfo.info = info_;
                collectionInfo.hint = hint_;
                collectionInfo.imageNum = downImage_;

                saveCollectionsTf.GetComponent<SaveCollections>().SettingCollectionInfo(downName_, groupCount_, downNum_);
            }

            if (downNum_ == 0 && checkCount < groupCount_)
            {
                checkCount += 1;

                saveCollectionsTf.GetComponent<SaveCollections>().SettingTitleInfo(name_, note_, downCount_, checkCount);
            }

            ItemManager.instance.collectionItems.Add(downName_, checkCount);
        }
    }     // ReadCollectionCSVFiles()

    #endregion 컬렉션 정보 데이터 저장
}
