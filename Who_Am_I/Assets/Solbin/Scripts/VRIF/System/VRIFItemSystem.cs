using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 장착 아이템 관리
/// </summary>
public class VRIFItemSystem : MonoBehaviour
{
    // 아이템 이름 - 아이템 오브젝트 딕셔너리
    private Dictionary<string, GameObject> mountingItem = default;

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

    // TestAction
    private TestAction testAction = default;

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

    private void OnEnable()
    {
        testAction = new TestAction();
        testAction.Enable();
    }

    private void OnDisable()
    {
        testAction?.Disable();
    }

    /// <summary>
    /// 딕셔너리 세팅
    /// </summary>
    private void DicSetting()
    {
        mountingItem = new Dictionary<string, GameObject>();

        mountingItem["NerfGun"] = nerfGun;
        mountingItem["Shavel"] = shavel;
        mountingItem["FishingRod"] = fishingRod;
        mountingItem["DragonflyNet"] = dragonflyNet;
    }
    #endregion

    private void Update()
    {
        if (testAction.Test.NerfGun.triggered)
        {
            if (!nerfGun.activeSelf)
            {
                MountingItem("NerfGun");
            }
            else { ReleaseItem("NerfGun"); }
        }
    }

    public void TestItemMounting(string name) // 테스트: 프로토타입 때 임시 사용할 메소드로, Test UI의 버튼과 연결
    {
        switch(name)
        {
            case "NerfGun":
                if (!nerfGun.activeSelf)
                {
                    MountingItem("NerfGun");
                }
                else { ReleaseItem("NerfGun"); }
                break;

            case "Shavel":
                if (!shavel.activeSelf)
                {
                    MountingItem("Shavel");
                }
                else { ReleaseItem("Shavel"); }
                break;
        }
    }

    #region 구현: 아이템 장착/해제 메소드
    /// <summary>
    /// 플레이어가 아이템을 장착하는 메소드
    /// </summary>
    /// <param name="_item">장착할 아이템</param>
    public void MountingItem(string _name)
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

        foreach (var obj in mountingItem) // 아이템 활성화 전 중복을 막기 위해 모든 아이템 비활성화 처리 
        {
            GameObject myItem = obj.Value;
            myItem.SetActive(false);
        }

        item.SetActive(true); // 지정 아이템만 활성화
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
