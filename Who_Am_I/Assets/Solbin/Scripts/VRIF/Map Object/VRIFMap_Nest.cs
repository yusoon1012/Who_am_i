using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFMap_Nest : MonoBehaviour
{
    // 계란 리스트
    private List<Transform> eggList = new List<Transform>();

    // 닭
    private Transform chicken = default;

    // Item Layer
    private int itemLayer = default;

    private void Start()
    {
        itemLayer = LayerMask.NameToLayer("Item");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Egg")) // 계란이면
        {
            eggList.Add(other.transform);
        }

        if (other.name.Contains("Chicken")) // 닭이면
        {
            chicken = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == chicken) // 둥지에 속한 닭이 나간거면 
        {
            ActivateItemCollider();
        }
    }

    private void ActivateItemCollider()
    {
        foreach (var egg in eggList) // 각각 계란의
        {
            int childCount = egg.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                if (egg.GetChild(i).gameObject.layer == itemLayer)
                {
                    egg.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }
}
