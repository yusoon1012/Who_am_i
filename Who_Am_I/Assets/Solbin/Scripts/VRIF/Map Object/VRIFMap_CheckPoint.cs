using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFMap_CheckPoint : MonoBehaviour
{
    // [FB] 체크포인트의 활성화 여부 
    public bool activated { get; private set; } = false;

    [Header("소속 지역")]
    [Tooltip("지도 내 아이콘에서 해당 정보를 가져가야 한다.")]
    public string region = default;

    [Header("소속 번호")]
    [Range(0, 10)]
    [Tooltip("지도 내 아이콘에서 해당 정보를 가져가야 하나, 위 지역 변수와 다르게 분류 번호는 인스펙터에서 직접 입력해야 한다.")]
    public int number = default;

    [Header("자식: Teleport Position")]
    [SerializeField] Transform teleport = default;

    [Header("오디오")]
    public AudioClip saveClip = default;
    private AudioSource audioSource = default;

    // 빠른 이동 좌표 
    public Vector3 teleportPosition { get; private set; }

    // 현재 씬의 체크 포인트 배열
    private VRIFMap_CheckPoint[] cpArray = default;

    private void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();

        cpArray = FindObjectsOfType<VRIFMap_CheckPoint>(); // 체크포인트 개수 대로 배열 생성 ? 필요한가?

        if (!activated)
        {
            transform.GetChild(0).gameObject.SetActive(false); // 빛 켜기 
        }
        else { transform.GetChild(0).gameObject.SetActive(true); }

        teleportPosition = teleport.position;

        CheckRegion();
    }

    /// <summary>
    /// 체크포인트 이름을 확인해 지역 자동 할당 
    /// </summary>
    private void CheckRegion()
    {
        if (gameObject.name.Contains("Spring")) { region = "Spring"; }
        else if (gameObject.name.Contains("Summer")) { region = "Summer"; }
        else if (gameObject.name.Contains("Fall")) { region = "Fall"; }
        else if (gameObject.name.Contains("Winter")) { region = "Winter"; }
    }

    /// <summary>
    /// 체크포인트 비활성화 => 활성화 동작
    /// </summary>
    public void Activated()
    {
        foreach (var cp in cpArray)
        {
            cp.activated = false; // 체크포인트 전부 비활성화
        }

        audioSource.PlayOneShot(saveClip);

        activated = true; // 해당 체크포인트만 활성화

        Save(); // 저장

        transform.GetChild(0).gameObject.SetActive(true); // 빛 켜기 

        Renderer renderer = transform.GetComponent<Renderer>(); // 렌더러
        Material[] materials = renderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Down
        }
    }

    /// <summary>
    /// VRIFSaveManager가 저장할 수 있도록 한다. 
    /// </summary>
    private void Save()
    {
        // TODO: 아이템 저장도 필요 

        VRIFGameManager.Instance.SaveGame(); // 저장 
    }
}

