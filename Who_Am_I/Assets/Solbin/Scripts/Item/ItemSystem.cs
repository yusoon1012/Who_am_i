using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 아이템 장착
/// </summary>

public class ItemSystem : MonoBehaviour
{
    // 장착 아이템 딕셔너리
    private Dictionary<string, GameObject> itemDic = new Dictionary<string, GameObject>();
    // 플레이어
    [SerializeField] private Transform player = default;
    // (장착) 삽
    [SerializeField] private GameObject shavel = default;
    // (장착) 낚시대
    [SerializeField] private GameObject fishingRod = default;
    // (장착) 너프건 
    [SerializeField] private GameObject nerfGun = default;

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// 초기 세팅
    /// </summary>
    private void Setting()
    {
        itemDic["Shavel"] = shavel;
        itemDic["FishingRod"] = fishingRod;
        itemDic["NerfGun"] = nerfGun;
    }

    /// <summary>
    /// 플레이어가 아이템을 장착하는 메소드
    /// </summary>
    /// <param name="_item">장착할 아이템</param>
    public void MountingItem(GameObject _item)
    {
        GameObject item = _item;
    }
}
