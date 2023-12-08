using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// (Player Controller - Player Range)
/// </summary>

public class VRIFPlayerItem : MonoBehaviour
{
    // VRIF Action
    VRIFAction vrifAction;
    // Test Action
    private TestAction testAction;
    // Shining Material Initial Value
    private const float shiningInitialValue = 1.05f;
    // Inventory Component
    [SerializeField] private Inventory inventory = default;

    // Item Manager
    [SerializeField] private ItemManager itemManager = default;

    // TEST_ 오브젝트 풀 포지션
    private Vector3 poolPos = new Vector3(0, -10, 0);

    [Header("Item UI")]
    // 아이템 UI
    [SerializeField] private GameObject itemInfoCanvas = default;
    // UI - 아이템 이름
    [SerializeField] private Text itemName = default;
    // UI - 아이템 설명
    [SerializeField] private Text itemInfo = default;


    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
        testAction = new TestAction();
        testAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
        testAction?.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Renderer renderer = other.transform.parent.GetComponent<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++) 
            {       
                materials[materials.Length - 1].SetFloat("_Scale", shiningInitialValue); // Material Scale Up 
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (vrifAction.Player.Interaction.triggered || testAction.Test.Interaction.triggered)
            {
                GameObject realItem = other.transform.parent.gameObject;
                GetItem(realItem); // 아이템 획득 가능
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Renderer renderer = other.transform.parent.GetComponent<Renderer>(); // 렌더러
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Down
            }
        }
    }

    /// <summary>   
    /// 아이템을 획득하는 메소드 
    /// </summary>
    private void GetItem(GameObject _item)
    {
        _item.transform.position = poolPos; // 테스트용 
       
        string name = default; // 아이템 이름

        if (_item.name.Contains("Meat")) { name = "고기"; }
        else if (_item.name.Contains("Milk")) { name = "우유"; }
        else if (_item.name.Contains("StrawBerry")) { name = "딸기"; }

        inventory.AddInventory(name, 1); // 인벤토리에 추가 

        StartCoroutine(PrintUI(name));
    }

    /// <summary>
    /// 아이템 UI 출력
    /// </summary>
    private IEnumerator PrintUI(string _name)
    {
        itemInfoCanvas.SetActive(true); // 아이템 정보 UI 활성화

        itemName.text = _name; // 아이템 이름 출력 
        /// <Point> ItemManager.cs에 아이템 정보를 담은 딕셔너리가 존재한다. (public)
        itemInfo.text = itemManager.itemDataBase[_name].itemInfo; // 아이템 정보 출력

        yield return new WaitForSeconds(3); // UI 3초 출력  

        itemInfoCanvas.SetActive(false); // 아이템 정보 UI 비활성화 
    }
}
