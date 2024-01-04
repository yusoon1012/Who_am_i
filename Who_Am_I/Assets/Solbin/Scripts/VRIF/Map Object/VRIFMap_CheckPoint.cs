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
    // 빠른 이동 좌표 
    public Vector3 teleportPosition { get; private set; }

    private VRIFMap_CheckPoint[] cpArray = default;

    [Tooltip("체크포인트 개수")]
    private int CPCount()
    {
        VRIFMap_CheckPoint[] checkPoints = FindObjectsOfType<VRIFMap_CheckPoint>();
        return checkPoints.Length;
    }

    private void Start()
    {
        //cpArray = new VRIFMap_CheckPoint[5]; // 체크포인트 개수 대로 배열 생성 ? 필요한가?

        if (!activated)
        {
            transform.GetChild(0).gameObject.SetActive(false); // 빛 켜기 
        }
        else { transform.GetChild(0).gameObject.SetActive(true); }

        teleportPosition = teleport.position;

        CheckRegion();
    }

    private void CheckRegion()
    {
        if (gameObject.name.Contains("Spring")) { region = "Spring"; }
        else if (gameObject.name.Contains("Summer")) { region = "Summer"; }
        else if (gameObject.name.Contains("Fall")) { region = "Fall"; }
        else if (gameObject.name.Contains("Winter")) { region = "Winter"; }
    }

    public void Activated()
    {
        if (!activated) { activated = true; } // 비활성화 상태일때 '활성화' 변경 

        // [FB]TODO: 파이어베이스에 (bool)activated 저장

        transform.GetChild(0).gameObject.SetActive(true); // 빛 켜기 

        Renderer renderer = transform.GetComponent<Renderer>(); // 렌더러
        Material[] materials = renderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            materials[materials.Length - 1].SetFloat("_Scale", 0); // Material Scale Down
        }
    }
}

// TODO: 1. 활성화 했다면 다른 체크포인트를 활성화하기 전까지 재활성화 불가 
// TODO: 2. 다른 체크포인트를 활성화할 때 해당 체크포인트를 제외한 다른 체크 포인트는 전부 비활성화 된다. 재활성화 가능한 상태 
