using BNG;
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

    [Header("Grabbers")]
    [Tooltip("아이템 획득 전 손을 놓게 하는 것부터 처리하기 위함")]
    [SerializeField] private Grabber leftGrabber = default;
    [SerializeField] private Grabber rightGrabber = default;
    // 양쪽 Grabber
    private Grabber[] grabbers = default;


    private void Start()
    {
        grabbers = new Grabber[2];
        grabbers[0] = leftGrabber;
        grabbers[1] = rightGrabber;
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

                if (renderer == null) // 뿌리채소는 구성이 약간 다르다.
                {
                    renderer = other.transform.GetComponent<Renderer>();
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
                    renderer = other.transform.GetComponent<Renderer>();
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
    private void GetItem(GameObject _item)
    {
        for (int i = 0; i < grabbers.Length; i++)
        {
            grabbers[i].ReleaseGrab(); // 아이템 획득 전 먼저 손을 놓게 한다. 
        }

        _item.transform.position = poolPos; // 오브젝트 풀로 이동 
       
        string name = default; // 아이템 이름

        switch (_item.name)
        {
            case var tag when tag.Contains("Meat"):
                name = "고기";
                break;
            case var tag when tag.Contains("Milk"):
                name = "우유";
                break;
            case var tag when tag.Contains("Strawberry"):
                name = "딸기";
                break;
            case var tag when tag.Contains("Song_e"):
                name = "송이 버섯";
                break;
        }

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

// TODO: Meat의 Outline Shader가 제대로 홯성화되지 않는 문제 => Materail가 최외곽선 안쪽으로 적용된다. 
