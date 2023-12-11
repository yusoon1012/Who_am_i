using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFMap_DirtFile : MonoBehaviour
{
    private int destroyCount = 3;

    public void AddDestroy() 
    {
        destroyCount -= 1;

        if (destroyCount <= 0) // 흙더미 HP가 0보다 작거나 같으면 
        {
            transform.GetComponent<VRIFItem_DropItem>().DropItem(); // 아이템 드롭 

            Destroy(gameObject); // 흙더미 오브젝트 파괴 
        }
    }
}
