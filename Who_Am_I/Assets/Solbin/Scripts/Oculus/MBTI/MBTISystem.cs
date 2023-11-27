using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBTISystem : MonoBehaviour
{
    #region 필드
    public static MBTISystem instance;

    //// 유형 총 퍼센테이지
    //private int fullPercent = 100;
    //// 에너지 성향
    //private float energy_E = default; // 외향
    //private float energy_I = default; // 내향
    //// 인식 성향
    //private float recognize_S = default; // 감각
    //private float recognize_N = default; // 직관
    //// 판단 성향
    //private float judgment_T = default; // 사고
    //private float judgment_F = default; // 감정
    //// 생활 양식
    //private float lifeCycle_J = default; // 판단
    //private float lifeCycle_P = default; // 인식
    #endregion

    public static MBTISystem Instance()
    {
        if (instance == null)
        {
            instance = new MBTISystem();
        }

        return instance;
    }

    // TODO: MBTI 수치 조정 메소드 추가 

}
