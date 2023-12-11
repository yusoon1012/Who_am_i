using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class VRIFItem_DropItem : MonoBehaviour
{
    [SerializeField] private GameObject p_dropItem = default;

    public void DropItem()
    {
        Vector3 dropPos = transform.position;
        Instantiate(p_dropItem, dropPos, Quaternion.identity); // 아이템 드롭 
    }
}
