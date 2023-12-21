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
    [Tooltip("지도 내 아이콘에서 해당 정보를 가져가야 하나, 위 지역 변수와 다르게 분류 번호는 직접 입력해야 한다.")]
    public int number = default;

    [Header("텔레포트 좌표")]
    [SerializeField] Transform teleport = default;
    // 빠른 이동 좌표 
    public Vector3 teleportPosition { get; private set; }

    private void Start()
    {
        // [FB]TODO: 파이어베이스에서 (bool)activated 정보 불러오기 

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
