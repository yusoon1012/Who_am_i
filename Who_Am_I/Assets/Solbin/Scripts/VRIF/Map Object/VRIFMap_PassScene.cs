using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFMap_PassScene : MonoBehaviour
{
    public enum Season
    {
        Spring, 
        Summer,
        Fall,
        Winter
    }

    [Header("이동할 계절")]
    [SerializeField] private Season season = default;

    [Header("힌지 조인트")]
    [SerializeField] private HingeJoint hinge = default;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어가 닿았다면
        {
            float angle = hinge.angle; // 문이 열리는 각도 

            if (angle >= 45)
            {
                string seasonName = default;

                switch (season)
                {
                    case Season.Spring:
                        seasonName = "Spring";
                        break;

                    case Season.Summer:
                        seasonName = "Summer";
                        break;

                    case Season.Fall:
                        seasonName = "Fall";
                        break;

                    case Season.Winter:
                        seasonName = "Winter";
                        break;
                }

                StartCoroutine(VRIFSceneManager.Instance.LoadHallScene(seasonName)); // 로딩 복도 씬 로드
            }
        }
    }
}