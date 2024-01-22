using BNG;
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
    public CharacterController characterController = default;

    [Header("Grabbers")]
    public Grabber leftGrabber = default;
    public Grabber rightGrabber = default;

    [Header("Grabber List")]
    public Grabber[] grabberArray = new Grabber[2];

    // 플레이어 포지션
    public Vector3 playerPos = default;
    // 플레이어 Rotation
    public Vector3 playerDir = default;
    // 현재 씬 
    public string currentScene = default;

    // (QuestManager_Jun) 현재 진행 퀘스트 번호
    public int currentQuest;
    // (QuestManager_Jun) 현재 퀘스트 리스트
    public List<Quest_Jun> questList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("<Solbin> Delete Another Player"); }

        grabberArray[0] = leftGrabber;
        grabberArray[1] = rightGrabber;
    }

    #region 시작: VRIFSaveManager에서 정보를 받아 세팅
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

        currentQuest = QuestManager_Jun.instance.currentQuest;
        questList = QuestManager_Jun.instance.questList;

        //Debug.Log("원 리스트: " + QuestManager_Jun.instance.questList[0]);
        //Debug.Log("temp 리스트: " + questList[0]);

        VRIFSaveManager.Instance.SaveFile(); // 정보를 최신화 해 전달 
    }
    #endregion
}
