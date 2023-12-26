using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRIFSaveManager : MonoBehaviour
{
    public static VRIFSaveManager Instance;

    [Header("Player Controller")]
    [SerializeField] private Transform playerController = default;

    [Tooltip("마지막으로 있던 씬 (현재 씬)")]
    public string sceneName { get; private set; }

    [Tooltip("플레이어의 위치")]
    public Vector3 playerPosition { get; private set; }

    [Tooltip("플레이어 소화기(UI)")]
    public int fullness { get; private set; } // 포만도
    public int poo { get; private set; } // 배출도

    // TODO: 체크포인트 활성화 여부 등 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetData()
    {
        sceneName = SceneManager.GetActiveScene().name; // 현재 씬 이름
        playerPosition = playerController.position; // Player Controller의 위치 
        fullness = transform.GetComponent<VRIFStatusSystem>().m_Fullness; // 플레이어 포만도
        poo = transform.GetComponent<VRIFStatusSystem>().m_Poo; // 플레이어 배출도 
    }
}
