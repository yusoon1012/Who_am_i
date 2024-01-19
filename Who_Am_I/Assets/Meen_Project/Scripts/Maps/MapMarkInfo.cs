using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarkInfo : MonoBehaviour
{
    // 지도상의 표식 정보 저장
    public string mapMarkInfo { get; set; } = default;
    // 지도상의 표식 워프 가능 여부를 저장
    public bool isWarpCheck { get; set; } = false;
    // 지도상의 워프 지점 순서 저장
    public int warpCheckCount = default;
    
    // 게임 시작 시 표식 정보를 저장하는 함수
    public void StartInfoSetting(string markInfo, bool warpCheck, int warpCount)
    {
        mapMarkInfo = markInfo;
        isWarpCheck = warpCheck;
        warpCheckCount = warpCount;
    }     // StartInfoSetting()

    // 표식 정보를 내보내는 함수
    public string SendStringInfo(out string markInfo)
    {
        markInfo = mapMarkInfo;

        return markInfo;
    }     // SendStringInfo()

    // 표식 워프 가능 여부를 내보내는 함수
    public bool SendBoolInfo(out bool warpCheck)
    {
        warpCheck = isWarpCheck;

        return warpCheck;
    }     // SendBoolInfo()

    // 워프 지점 순서를 내보내는 함수
    public int SendCountInfo(out int warpCount)
    {
        warpCount = warpCheckCount;

        return warpCount;
    }     // SendCountInfo()
}
