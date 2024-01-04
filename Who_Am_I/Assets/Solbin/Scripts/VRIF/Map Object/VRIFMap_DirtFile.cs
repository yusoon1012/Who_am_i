using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFMap_DirtFile : MonoBehaviour
{
    private int destroyCount = 2;

    [Header("드롭할 아이템 프리팹")]
    [SerializeField] private GameObject p_dropItem = default;

    public void AddDestroy() 
    {
        destroyCount -= 1;

        if (destroyCount <= 0) // 흙더미 HP가 0보다 작거나 같으면 
        {
            DropItem(); // 아이템 드롭

            Destroy(gameObject); // 흙더미 오브젝트 파괴 
        }
    }

    public void DropItem()
    {
        Vector3 dropPos = transform.position;
        Instantiate(p_dropItem, dropPos, Quaternion.identity); // 아이템 드롭 
    }
}
