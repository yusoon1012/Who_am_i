using BNG;
//using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    [Header("Item UI")]
    // 아이템 UI
    [SerializeField] private GameObject itemInfoCanvas = default;
    // UI - 아이템 이름
    [SerializeField] private Text itemName = default;
    // UI - 아이템 설명
    [SerializeField] private Text itemInfo = default;

    [Header("Grabbers")]
    [Tooltip("아이템 획득 전 손을 놓게 하는 것부터 처리하기 위함")]
    [SerializeField] private Grabber leftGrabber = default;
    [SerializeField] private Grabber rightGrabber = default;
    // 양쪽 Grabber
    private Grabber[] grabbers = default;

    [Header("Audio")]
    public AudioClip getItemClip = default;
    private AudioSource audioSource = default;

    private Dictionary<string, string> itemDic = new Dictionary<string, string>()
    {
        { "Juksoon", "죽순" },
        { "Peanut", "땅콩" },
        { "Song_e", "송이 버섯" },
        { "Coconut", "코코넛" },
        { "Ginkgo", "은행" },
        { "Kakao", "카카오" }

        // TODO: 추가 처리 필요 
    };

    private void Start()
    {
        grabbers = new Grabber[2];
        grabbers[0] = leftGrabber;
        grabbers[1] = rightGrabber;

        audioSource = GetComponent<AudioSource>();

        ItemSetting();
    }

    private void ItemSetting()
    {
        GameObject[] items = FindObjectsOfType<GameObject>()
                                  .Where(obj => obj.layer == LayerMask.NameToLayer("Item")).ToArray();

        foreach (var item in items)
        {
            item.AddComponent<VRIFItem_RespawnHelper>();
        }
    }

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
            if (other.transform.parent.GetComponent<Outline>())
            {
                other.transform.parent.GetComponent<Outline>().OutlineWidth = 10;
            }
            else 
            {
                Renderer renderer = other.transform.parent.GetComponent<Renderer>(); // 렌더러

                if (renderer == null) // 구성이 다른 경우가 있다.
                {
                    renderer = other.transform.parent.GetComponentInChildren<Renderer>();
                }

                Material[] materials = renderer.materials;


                for (int i = 0; i < materials.Length; i++)
                {
                    materials[materials.Length - 1].SetFloat("_Scale", shiningInitialValue); // Material Scale Up 
                }
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
            if (other.transform.parent.GetComponent<Outline>())
            {
                other.transform.parent.GetComponent<Outline>().OutlineWidth = 0;
            }
            else
            {
                Renderer renderer = other.transform.parent.GetComponent<Renderer>(); // 렌더러

                if (renderer == null) // 뿌리채소는 구성이 약간 다르다.
                {
                    renderer = other.transform.parent.GetComponentInChildren<Renderer>();
                }

                Material[] materials = renderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Up 
                }
            }
        }
    }

    /// <summary>   
    /// 아이템을 획득하는 메소드 
    /// </summary>
    private void GetItem(GameObject item_)
    {
        audioSource.PlayOneShot(getItemClip); // 아이템 획득 소리 재생

        if (item_.transform.GetComponentInChildren<VRIFItem_RespawnHelper>())
        {
            VRIFItem_RespawnHelper respawnHelper = item_.transform.GetComponentInChildren<VRIFItem_RespawnHelper>();
            StartCoroutine(respawnHelper.Respawn());
        }
      
        for (int i = 0; i < grabbers.Length; i++)
        {
            grabbers[i].ReleaseGrab(); // 아이템 획득 전 먼저 손을 놓게 한다. 
        }

        string name = default; // 아이템 이름

        if (item_.name.Contains("Strawberry")) { QuestManager_Jun.instance.CheckClear("Strawberry"); }

        foreach (var itemKey in itemDic.Keys)
        {
            if (item_.name.Contains(itemKey)) // 위 아이템 딕셔너리의 키를 포함하면
            {
                name = itemDic[itemKey]; // 한글로 변환 (키에 해당하는 값)
            }
        }

        inventory.AddInventory(name, 1); // 인벤토리에 추가 

        StartCoroutine(PrintUI(name));

        if (!item_.transform.GetComponentInChildren<VRIFItem_RespawnHelper>()) // 만약 리스폰 헬퍼가 없는 아이템이며 코드 끝까지 처리되지 않았다면
        {
            Destroy(item_);
        }
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
