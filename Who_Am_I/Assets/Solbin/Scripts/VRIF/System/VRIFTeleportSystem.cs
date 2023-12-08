using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTeleportSystem : MonoBehaviour
{
    private Dictionary<string, Transform> teleportHall = new Dictionary<string, Transform>();

    [Header("Teleport Hall")]
    [Tooltip("각 지역 내 텔레포트홀 Transform")]
    [SerializeField] private Transform beachHall = default;
    [SerializeField] private Transform forestHall = default;
    [SerializeField] private Transform templeHall = default;
    [SerializeField] private Transform winterHall = default;
    [SerializeField] private Transform fallHall = default;

    [Header("Player")]
    [SerializeField] private Transform player = default;

    private void Start()
    {
        Setting();
    }

    private void Setting()
    {
        teleportHall["Beach"] = beachHall;
        teleportHall["Forest"] = forestHall;
        teleportHall["Temple"] = templeHall;
        teleportHall["Winter"] = winterHall;
        teleportHall["Fall"] = fallHall;
    }

    public void Teleport(string _regionName)
    {
        Vector3 position = teleportHall[_regionName].position;
        position.y = 0;

        player.position = position;
    }
}
