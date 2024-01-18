using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 장착 아이템 관리
/// </summary>
public class VRIFItemSystem : MonoBehaviour
{
    public static VRIFItemSystem Instance;

    // 아이템 배열
    private GameObject[] mountingItem = default;

    [Header("움직임 적용 손 모델")]
    [SerializeField] private GameObject modelsRight = default;

    [Header("장갑이 없는 손")]
    [SerializeField] private GameObject[] bareHands = new GameObject[2];

    [Header("장착 아이템")]
    // (장착) 너프건 
    [SerializeField] private GameObject nerfGun = default;
    // (장착) 삽
    [SerializeField] private GameObject shovel = default;
    // (장착) 낚시대
    [SerializeField] private GameObject ladder = default;
    // (장착) 낚시대
    [SerializeField] private GameObject fishingRod = default;
    // (장착) 곤충 채집망
    [SerializeField] private GameObject dragonflyNet = default;
    // (장착) 장갑
    [SerializeField] private GameObject[] gloves = new GameObject[2];

    // 손 타입 (맨손 vs 장갑)
    public enum HandType
    {
        BAREHANDS,
        GLOVES
    }

    public HandType handType { get; private set; }

    // 아이템 타입
    public enum ItemType
    {
        NONE,
        NERFGUN,
        SHOVEL,
        LADDER,
        FISHINGROD,
        DRAGONFLYNET,
    }

    public ItemType itemType { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Setting();
        ArraySetting(); // 딕셔너리 세팅
    }

    #region 초기 세팅
    /// <summary>
    /// 초기 세팅
    /// </summary>
    private void Setting()
    {
        nerfGun.SetActive(false);
        shovel.SetActive(false);
        ladder.SetActive(false);
        fishingRod.SetActive(false);
        dragonflyNet.SetActive(false);
        
        foreach (var glove in gloves)
        {
            glove.SetActive(false);
        }
    }

    /// <summary>
    /// 딕셔너리 세팅
    /// </summary>
    private void ArraySetting()
    {
        mountingItem = new GameObject[5];

        mountingItem[0] = nerfGun;
        mountingItem[1] = shovel;
        mountingItem[2] = ladder;
        mountingItem[3] = fishingRod;
        mountingItem[4] = dragonflyNet;
    }
    #endregion

    public void InputItem(string name) // 현재는 Test UI의 버튼과 연결
    {
        switch(name)
        {
            case "NerfGun":
                if (!nerfGun.activeSelf) { MountingItem("NerfGun"); }
                else { ReleaseItem(); }
                break;

            case "Shovel":
                if (!shovel.activeSelf) { MountingItem("Shovel"); }
                else { ReleaseItem(); }
                break;

            case "Ladder":
                if (!ladder.activeSelf) { MountingItem("Ladder"); }
                else { ReleaseItem(); }
                break;

            case "FishingRod":
                if (!fishingRod.activeSelf) { MountingItem("FishingRod"); }
                else { ReleaseItem(); }
                break;

            case "DragonflyNet":
                if (!dragonflyNet.activeSelf) { MountingItem("DragonflyNet"); }
                else { ReleaseItem(); }
                break;

            case "Gloves":
                if (!gloves[0].activeSelf && !gloves[1].activeSelf) { OnGloves(); }
                else { OffGloves(); }
                break;
        }
    }

    #region 구현: 아이템 장착/해제 메소드
    /// <summary>
    /// 플레이어가 아이템을 장착하는 메소드
    /// </summary>
    /// <param name="_item">장착할 아이템</param>
    private void MountingItem(string _name)
    {
        string name = _name;
        GameObject item = default;

        switch (name)
        {
            case "NerfGun":
                item = nerfGun;
                itemType = ItemType.NERFGUN;
                break;
            case "Shovel":
                item = shovel;
                itemType = ItemType.SHOVEL;
                break;
            case "Ladder":
                item = ladder;
                itemType = ItemType.LADDER;
                break;
            case "FishingRod":
                item = fishingRod;
                itemType = ItemType.FISHINGROD;
                break;
            case "DragonflyNet":
                item = dragonflyNet;
                itemType = ItemType.DRAGONFLYNET;
                break;

            default:
                Debug.LogError("<Solbin> Item Error");
                break;
        }

        foreach (var _item in mountingItem) // 아이템 활성화 전 중복을 막기 위해 모든 아이템 비활성화 처리 
        {
            _item.SetActive(false);
        }

        modelsRight.SetActive(false); // 기존 손 모델 OFF (임시방편)

        item.SetActive(true); // 지정 아이템만 활성화
    }

    /// <summary>
    /// 플레이어가 아이템 장착을 해제하는 메소드
    /// </summary>
    public void ReleaseItem()
    {
        foreach (var _item in mountingItem)
        {
            _item.SetActive(false);
        }

        modelsRight.SetActive(true); // 기존 손 모델 ON (임시방편)

        itemType = ItemType.NONE; // 아이템 미장착 상태 
    }
    #endregion

    #region 구현: 장갑 장착/해제 메소드 
    /// <summary>
    /// 장갑을 장착하는 메소드
    /// </summary>
    private void OnGloves()
    {
        for (int i = 0; i < 2; i++)
        {
            bareHands[i].SetActive(false); // 맨손 OFF
            gloves[i].SetActive(true); // 장갑 ON
        }

        handType = HandType.GLOVES;
    }

    /// <summary>
    /// 장갑을 벗는 메소드 
    /// </summary>
    private void OffGloves()
    {
        for (int i = 0; i < 2; i++)
        {
            gloves[i].SetActive(false); // 장갑 OFF
            bareHands[i].SetActive(true); // 맨손 ON
        }

        handType = HandType.BAREHANDS;
    }
    #endregion
}
