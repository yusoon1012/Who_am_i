using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRIFMBTISystem : MonoBehaviour
{
    #region 필드
    private struct MBTITraits
    {
        // 에너지 성향 
        public int energy_E; // 외향
        public int energy_I; // 내향
        // 인식 성향
        public int recognize_S; // 감각
        public int recognize_N; // 직관
        // 판단 성향 
        public int judgment_T; // 사고
        public int judgment_F; // 감정
        // 생활 양식 성향 
        public int lifeCycle_J; // 판단
        public int lifeCycle_P; // 인식
    }

    private MBTITraits traits; // MBTI 구조체 인스턴스

    [Header("Show MBTI")]
    [Tooltip("UI에 할당된 MBTI 수치 컴포넌트")]
    [SerializeField] private Text text_E = default;
    [SerializeField] private Text text_I = default;
    [SerializeField] private Text text_S = default;
    [SerializeField] private Text text_N = default;
    [SerializeField] private Text text_T = default;
    [SerializeField] private Text text_F = default;
    [SerializeField] private Text text_J = default;
    [SerializeField] private Text text_P = default;
    #endregion

    /// <summary>
    /// MBTI 성향을 계산하는 메소드 
    /// </summary>
    /// <param name="_alphabet">MBTI 성향</param>
    public void CalculateMBTI(char _alphabet)
    {
        switch (_alphabet)
        {
            case 'E':
                traits.energy_E += 1;
                break;

            case 'I':
                traits.energy_I += 1;
                break;

            case 'S':
                traits.recognize_S += 1;
                break;

            case 'N':
                traits.recognize_N += 1;
                break;

            case 'T':
                traits.judgment_T += 1;
                break;

            case 'F':
                traits.judgment_F += 1;
                break;

            case 'J':
                traits.lifeCycle_J += 1;
                break;

            case 'P':
                traits.lifeCycle_P += 1;
                break;
        }

        AssignNumeric();
    }

    private void AssignNumeric()
    {
        text_E.text = traits.energy_E.ToString();
        text_I.text = traits.energy_I.ToString();
        text_S.text = traits.recognize_S.ToString();
        text_N.text = traits.recognize_N.ToString();
        text_T.text = traits.judgment_T.ToString();
        text_F.text = traits.judgment_F.ToString();
        text_J.text = traits.lifeCycle_J.ToString();
        text_P.text = traits.lifeCycle_P.ToString();
    }
}