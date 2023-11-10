using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    #region 필드
    // 텔레포트 홀 좌표 딕셔너리
    private Dictionary<string, Vector3> hallPos = new Dictionary<string, Vector3>();
    // 플레이어 트랜스폼
    [SerializeField] private Transform player = default;
    // 텔레포트 UI
    public Canvas teleportUI = default;
    // 임시 홀 게임오브젝트
    [SerializeField] private Transform mainHall = default;
    [SerializeField] private Transform cityHall = default;
    [SerializeField] private Transform iceHall = default;
    [SerializeField] private Transform forestHall = default;
    [SerializeField] private Transform beachHall = default;
    #endregion

    private void Start()
    {
        GetHallPos(); // 좌표 세팅
    }

    /// <summary>
    /// (초기 세팅)텔레포트홀 좌표 딕셔너리 세팅
    /// </summary>
    private void GetHallPos()
    {
        // TODO: 차후 맵 확정 시 검색 방식으로 교체
        hallPos["MainHall"] = mainHall.position;
        hallPos["CityHall"] = cityHall.position;
        hallPos["IceHall"] = iceHall.position;
        hallPos["ForestHall"] = forestHall.position;
        hallPos["BeachHall"] = beachHall.position;
    }

    #region 텔레포트 구현
    /// <summary>
    /// 텔레포트 버튼 클릭 시 텔레포트 홀의 좌표를 얻는다
    /// </summary>
    /// <param name="buttonName">이동 버튼 게임오브젝트의 이름</param>
    /// <returns>텔레포트 할 홀의 좌표</returns>
    public void TeleportPos(string buttonName)
    {
        Vector3 teleportPos = default;

        switch (buttonName)
        {
            case "MainHall":
                teleportPos = hallPos["MainHall"];
                break;

            case "CityHall": // 도시맵 이동
                Debug.Log("도시맵으로 이동!");
                teleportPos = hallPos["CityHall"];
                break;

            case "IceHall": // 얼음맵 이동
                teleportPos = hallPos["IceHall"];
                break;

            case "ForestHall": // 숲맵 이동
                teleportPos = hallPos["ForestHall"];
                break;

            case "BeachHall": // 해안지역 이동
                teleportPos = hallPos["BeachHall"];
                break;

            default:
                Debug.LogWarning("대상 홀을 찾지 못함");
                teleportPos = Vector3.zero;
                break;
        }

        Teleport(teleportPos);
    }

    private void Teleport(Vector3 _teleportPos)
    {
        Vector3 teleportPos = _teleportPos;

        // TODO: 차후 텔레포트 효과 추가
        player.position = teleportPos;
    }
    #endregion
}
