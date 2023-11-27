using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

/// <summary>
/// 플레이어의 아이템 장착
/// </summary>

public class ItemSystem : MonoBehaviour
{
    private Dictionary<string, GameObject> mountingItem = new Dictionary<string, GameObject>();

    [Header("Items")]
    // (장착) 너프건 
    [SerializeField] private GameObject nerfGun = default;
    // (장착) 삽
    [SerializeField] private GameObject shavel = default;
    // (장착) 낚시대
    [SerializeField] private GameObject fishingRod = default;
    // (장착) 곤충 채집망
    [SerializeField] private GameObject dragonflyNet = default;
    // LEGACY: (장착) 새총
    //[SerializeField] private GameObject slingShot = default;

    private void Start()
    {
        Setting();
        DicSetting(); // 딕셔너리 세팅
    }

    #region 초기 세팅
    /// <summary>
    /// 초기 세팅
    /// </summary>
    private void Setting()
    {
        nerfGun.SetActive(false);
        shavel.SetActive(false);
        fishingRod.SetActive(false);
        dragonflyNet.SetActive(false);
    }

    /// <summary>
    /// 딕셔너리 세팅
    /// </summary>
    private void DicSetting()
    {
        mountingItem["NerfGun"] = nerfGun;
        mountingItem["Shavel"] = shavel;
        mountingItem["FishingRod"] = fishingRod;
        mountingItem["DragonflyNet"] = dragonflyNet;
    }
    #endregion

    #region 구현: 아이템 장착/해제 메소드
    /// <summary>
    /// 플레이어가 아이템을 장착하는 메소드
    /// </summary>
    /// <param name="_item">장착할 아이템</param>
    public void MountingItem(string _name)
    {
        string name = _name;
        GameObject item = default;

        switch(name)
        {
            case "NerfGun":
                item = nerfGun;
                break;
            case "Shavel":
                item = shavel;
                break;
            case "FishingRod":
                item = fishingRod;
                break;
            case "DragonflyNet":
                item = dragonflyNet;
                break;
            default:
                Debug.LogError("<Solbin> Item Error");
                break;
        }

        item.SetActive(true);
    }

    /// <summary>
    /// 플레이어가 아이템 장착을 해제하는 메소드
    /// </summary>
    /// <param name="_item">해제할 아이템</param>
    public void ReleaseItem(string _name)
    {
        string name = _name;
        GameObject item = default;

        switch (name)
        {
            case "NerfGun":
                item = nerfGun;
                break;
            case "Shavel":
                item = shavel;
                break;
            case "FishingRod":
                item = fishingRod;
                break;
            case "DragonflyNet":
                item = dragonflyNet;
                break;
            default:
                Debug.LogError("<Solbin> Item Error");
                break;
        }

        item.SetActive(false);
    }
    #endregion
}
