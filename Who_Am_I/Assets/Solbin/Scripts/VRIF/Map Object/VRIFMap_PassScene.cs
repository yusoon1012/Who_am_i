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

    // 이동하고 싶은 계절의 이름 
    [SerializeField] private Season season = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어가 닿았다면
        {
            string seasonName = default;

            switch(season)
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
