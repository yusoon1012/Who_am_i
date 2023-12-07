using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 수렵동물의 UI가 플레이어를 바라보도록 설정 
/// </summary>
public class VRIFNameCanvas : MonoBehaviour
{
    private Transform player;
    private Vector3 rotation = default;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // TODO: 이후 로직 개선 필요
    }

    private void Update()
    {
        transform.LookAt(player);
        rotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0); // X와 Z축은 고정 
    }
}
