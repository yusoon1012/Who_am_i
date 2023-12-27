using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    // 스크린 이펙트에 사용될 스크린 이미지 목록
    public Image[] Screens = new Image[1];

    // 스크린 이펙트에서 변경할 알파값 수치
    Color screenColor = default;

    // 스크린 종류
    private int screenType = default;
    // 하나의 딜레이당 화면 알파값이 변하는 수치
    private float changeAlpha = default;
    // 하나의 딜레이마다 걸리는 시간
    private float delayTime = default;
    // 스크린 이펙트 기능이 작동중에 현재 스크린 알파값 계산값
    private float nowAlpha = default;
    // 스크린 이펙트 분기가 끝난 뒤 딜레이 시간
    private float durationTime = default;
    // 스크린 이펙트가 이루어지는 사이클의 횟수
    private int effectRound = default;
    // 스크린 이펙트 기능이 작동중에 현재 라운드 계산값
    private int nowRound = default;
    // 현재 분기가 정방향인지, 역방향인지 체크
    private bool reverseCheck = false;
    // 현재 스크린 이펙트가 진행중인 상태인지 체크
    private bool isGoingEffect = false;

    void Awake()
    {
        screenType = 0;
        effectRound = 0;
        nowRound = 0;
        changeAlpha = 0f;
        delayTime = 0f;
        durationTime = 0f;
    }     // Awake()

    // 스크린 이펙트에 관련된 변수값들을 받아 스크린 이펙트를 시작하는 함수
    // type : 스크린 종류
    // alpha : 하나의 딜레이당 화면 알파값이 변하는 수치
    // time : 하나의 딜레이마다 걸리는 시간
    // durTime : 스크린 이펙트 분기가 끝난 뒤 딜레이 시간
    // rount : 스크린 이펙트가 이루어지는 사이클의 횟수
    public void ScreenEffect(int type, float alpha, float time, float durTime, int round)
    {
        if (isGoingEffect == true) { return; }

        isGoingEffect = true;
        screenType = type;
        changeAlpha = alpha;
        delayTime = time;
        durationTime = durTime;
        effectRound = round;

        Screens[screenType].gameObject.SetActive(true);

        StartCoroutine(FunctionScreenEffect());
    }     // NormalScreenEffect()

    // 스크린 이펙트 효과 기능 코루틴 함수
    IEnumerator FunctionScreenEffect()
    {
        nowAlpha = 0f;

        while (true)
        {
            if (nowAlpha >= 1f) { break; }

            screenColor.a = nowAlpha += changeAlpha;
            Screens[screenType].color = screenColor;

            yield return new WaitForSeconds(delayTime);
        }

        reverseCheck = true;

        if (durationTime > 0f)
        {
            StartCoroutine(DelayTimeScreenEffect());
        }
        else
        {
            CheckScreenEffect();
        }
    }     // ShowNormalScreenEffect()

    // 스크린 이펙트 효과가 반대로 이루어지는 기능 코루틴 함수
    IEnumerator ReverseScreenEffect()
    {
        nowAlpha = 1f;

        while (true)
        {
            if (nowAlpha <= 0f) { break; }

            screenColor.a = nowAlpha -= changeAlpha;
            Screens[screenType].color = screenColor;

            yield return new WaitForSeconds(delayTime);
        }

        reverseCheck = false;
        nowRound += 1;

        if (nowRound < effectRound)
        {
            if (durationTime > 0f)
            {
                StartCoroutine(DelayTimeScreenEffect());
            }
            else
            {
                CheckScreenEffect();
            }
        }
        else
        {
            CheckScreenEffect();
        }
        
    }     // ReverseScreenEffect()

    // 사이클의 분기마다 스크린 이펙트가 끝이난 뒤 딜레이 타임을 주는 함수
    IEnumerator DelayTimeScreenEffect()
    {
        yield return new WaitForSeconds(durationTime);

        CheckScreenEffect();
    }     // DelayTimeScreenEffect()

    // 한 스크린 이펙트 분기가 끝나고 라운드를 체크하는 함수
    private void CheckScreenEffect()
    {
        if (nowRound < effectRound)
        {
            if (reverseCheck == false) { StartCoroutine(FunctionScreenEffect()); }
            else if (reverseCheck == true) { StartCoroutine(ReverseScreenEffect()); }
        }
        else
        {
            EndScreenEffect();
        }
    }     // CheckScreenEffect()

    // 스크린 이펙트를 종료하는 함수
    private void EndScreenEffect()
    {
        screenType = 0;
        nowRound = 0;
        effectRound = 0;
        changeAlpha = 0f;
        delayTime = 0f;
        durationTime = 0f;
        reverseCheck = false;
        isGoingEffect = false;

        Screens[screenType].gameObject.SetActive(false);
    }     // EndScreenEffect()
}
