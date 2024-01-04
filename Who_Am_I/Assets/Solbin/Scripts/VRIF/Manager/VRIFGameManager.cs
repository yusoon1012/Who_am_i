using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (최우선) VRIFSaveManager, (이후 우선) VRIFGameManager는 다른 스크립트보다 우선순위 실행
/// </summary>

public class VRIFGameManager : MonoBehaviour
{
    public static VRIFGameManager Instance;

    [Header("Player Controller")]
    public Transform m_playerController = default;

    // 플레이어 포지션
    public Vector3 playerPos = default;
    // 플레이어 Rotation
    public Vector3 playerDir = default;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // TODO: 추후 이 메소드는 삭제되고 SceneManager에서 birthPoint로 관리하게 된다. 
    public void TestSetting()
    {

    }

    public void SaveGame()
    {

    }
}
