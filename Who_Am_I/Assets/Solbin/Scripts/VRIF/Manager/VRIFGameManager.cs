using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// (최우선) VRIFSaveManager, (이후 우선) VRIFGameManager는 다른 스크립트보다 우선순위 실행
/// 저장: VRIFGameManager => VRIFSaveManeger (정보 최신화 먼저)
/// 불러오기: VRIFSaveManager => VRIFGameManager (정보 불러오기 먼저)
/// </summary>

public class VRIFGameManager : MonoBehaviour
{
    public static VRIFGameManager Instance;

    [Header("Player Controller")]
    public Transform playerController = default;

    [Header("Character Controller")]
    [SerializeField] private CharacterController characterController = default;

    // 플레이어 포지션
    public Vector3 playerPos = default;
    // 플레이어 Rotation
    public Vector3 playerDir = default;
    // 현재 씬 
    public string currentScene = default;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #region 시작: VRIFSaveManager에서 정보를 받아 세팅
    // TODO: 추후 이 메소드는 삭제되고 SceneManager에서 birthPoint로 관리하게 된다. 
    public void PlayerSetting()
    {
        // VRIFSaveManager에서 playerPos와 playerDir을 먼저 세팅한다.
        characterController.enabled = false; // 위치값 덮어쓰기를 막기 위함
        playerController.position = playerPos;
        playerController.rotation = Quaternion.Euler(playerDir);
        characterController.enabled = true;
    }
    #endregion

    #region 종료: 정보를 최신화 한 후 VRIFSaveManager로 전달 
    /// <summary>
    /// 저장 전 정보를 최신화하는 메소드 (저장은 이 메소드를 무조건 통해야 한다.) 
    /// </summary>
    public void SaveGame()
    {
        currentScene = SceneManager.GetActiveScene().name; // 현재 씬 정보 (string)

        playerPos = playerController.position;
        playerDir = playerController.rotation.eulerAngles;

        VRIFSaveManager.Instance.SaveFile(); // 정보를 최신화 해 전달 
    }
    #endregion
}
