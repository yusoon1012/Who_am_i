using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Player Controller - Player Range)
/// </summary>

public class VRIFPlayerItem : MonoBehaviour
{
    // VRIF Action
    VRIFAction vrifAction;
    // Shining Material
    [SerializeField] private Material shiningMaterial = default;
    // Shining Material Initial Value
    private const float shiningInitialValue = 1.03f;

    private void Start()
    {
        Setting();
    }

    private void Setting()
    {
        shiningMaterial.SetFloat("_Scale", 0f);
    }

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            shiningMaterial.SetFloat("_Scale", shiningInitialValue);

            GetItem(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            shiningMaterial.SetFloat("_Scale", 0f);
        }
    }

    /// <summary>   
    /// 아이템을 획득하는 메소드 
    /// </summary>
    private void GetItem(GameObject _item)
    {
        if (vrifAction.Player.Interaction.triggered)
        {
            Debug.LogWarning("Get " + _item + "item!");
        }
    }
}

// TODO: Inventory.cs에서 아이템을 받도록 코드 추가 필요 
