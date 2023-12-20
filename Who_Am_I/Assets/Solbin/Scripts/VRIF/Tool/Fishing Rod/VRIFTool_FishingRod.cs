using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTool_FishingRod : MonoBehaviour
{
    public static VRIFTool_FishingRod Instance;

    [Header("[PlayerRange] VRIFPlayerFishing")]
    [SerializeField] private VRIFPlayerFishing vrifPlayerFishing = default;
    [Header("[PlayerRange] Fishing Hit Range")]
    [SerializeField] Collider fishingHitRange = default;

    [Header("낚시찌 시작 위치")]
    [SerializeField] Transform bobberStart = default;
    [Header("낚시찌 던져질 위치")]
    [SerializeField] Transform bobberFinish = default;

    [Header("낚시찌 프리팹")]
    [SerializeField] private GameObject bobberPrefab = default;

    [Header("화살표")]
    [SerializeField] private GameObject leftArrow = default; // 왼쪽
    [SerializeField] private GameObject upArrow = default; // 위쪽
    [SerializeField] private GameObject rightArrow = default; // 오른쪽 

    [Header("낚시줄")]
    [SerializeField] private LineRenderer lineRenderer = default;

    // 낚시찌 게임 오브젝트
    private GameObject bobber = default;
    // 낚시찌 Rigidbody
    private Rigidbody bobberRigid = default;
    // 낚시찌 AddForce delay
    private bool throwBobber = false;
    // 오브젝트 풀 
    private Vector3 poolPos = new Vector3(0, -10, 0);

    // 낚시 중 (낚시 코루틴 실행 중)
    public bool isFishing { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        bobber = Instantiate(bobberPrefab, poolPos, Quaternion.identity); // 낚시찌 프리팹으로 낚시찌 생성
        bobberRigid = bobber.GetComponent<Rigidbody>();

        VRIFTool_FishingBobber.fishNibble += StartCheckStrike;

        //lineRenderer.positionCount = 2; // 낚시대 끝과 낚시찌를 연결 
    }

    private void OnTriggerExit(Collider other)
    {
        if (vrifPlayerFishing.activateFishing && other == fishingHitRange) // 물가에 접촉해있고 낚시대를 위로 들어올렸을 때 
        {
            if (!isFishing) { StartCoroutine(CheckSwing()); } // 이미 낚시 중이 아닐 때  
        }
    }

    /// <summary>
    /// 1초간 아래로 휘두르는 동작을 확인
    /// </summary>
    private IEnumerator CheckSwing()
    {
        float time = 0f;

        while (time < 1) // 뒤로 젖힌 후로부터 1초 간 
        {
            time += Time.deltaTime;

            if (VRIFInputSystem.Instance.rVelocity.y <= -0.7f) // 아래로 휘두르는 것이 확인되면
            {
                bobber.transform.position = bobberStart.position; // 낚시찌 시작 지점에 위치

                ThrowBobber();
            }

            yield return null;
        }
    }

    /// <summary>
    /// 낚시찌를 던진다
    /// </summary>
    private void ThrowBobber()
    {
        if (!throwBobber)
        {
            throwBobber = true;

            bobberRigid.AddForce(Vector3.up * 4.5f, ForceMode.Impulse);
            bobberRigid.AddForce(bobberFinish.position * 0.7f, ForceMode.Impulse);

            Invoke("ClearThrowBobber", 1);
        }
    }

    private void ClearThrowBobber() { throwBobber = false; } // 낚시찌에 다시 힘을 가할 수 있도록 한다. 

    /// <summary>
    /// 입질 이벤트를 구독해 낚시 코루틴을 시작한다. 
    /// </summary>
    private void StartCheckStrike(object sender, EventArgs e) { if (!isFishing) { StartCoroutine(CheckStrike()); } }

    /// <summary>
    /// 입질이 확인되었다면 낚시 시작
    /// </summary>
    private IEnumerator CheckStrike()
    {
        isFishing = true;

        int successNum = 0; // 낚시 성공 횟수

        for (int i = 0; i < 3; i++)
        {
            InputBridge.Instance.VibrateController(0.1f, 0.2f, 0.5f, ControllerHand.Left);
            InputBridge.Instance.VibrateController(0.1f, 0.2f, 0.5f, ControllerHand.Right);

            int direction; // 화살표 방향
            GameObject arrow = default; // 화살표 UI 오브젝트 

            direction = UnityEngine.Random.Range(1, 3); // 1: 좌, 2: 위, 3: 우

            switch (direction)
            {
                case 1: // 좌
                    arrow = leftArrow;
                    break;
                case 2: // 위
                    arrow = upArrow;
                    break;
                case 3: // 우 
                    arrow = rightArrow;
                    break;
            }

            arrow.SetActive(true); // 화살표 활성화

            float time = 0; // 타이머
            bool success = false; // 낚시 성공 여부 

            while (time < 3) // 3초 동안 화살표 출력 (낚시 성공 여부 체크)
            {
                time += Time.deltaTime;

                if (direction == 1 && VRIFInputSystem.Instance.rVelocity.x <= -0.7f) { success = true; break; } // 컨트롤러를 좌측으로 휘둘렀으면
                else if (direction == 2 && VRIFInputSystem.Instance.rVelocity.y >= 0.7f) { success = true; break; } // 위로 휘둘렀으면
                else if (direction == 3 && VRIFInputSystem.Instance.rVelocity.x >= 0.7f) { success = true; break; } // 우측으로 휘둘렀으면
                else { yield return null; } // 아무것도 하지 않았다면 while문 계속 진행 
            }

            if (success) { arrow.SetActive(false); successNum += 1; Debug.LogWarning("Success Number= " + successNum); } // 3초 내 물고기를 잡았다면
            else if (!success) { arrow.SetActive(false); StartCoroutine(GetFish(false)); yield break; }


            if (successNum >= 3) { StartCoroutine(GetFish(true)); } // 물고기 획득 성공

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator GetFish(bool _gotcha)
    {
        bobberRigid.AddForce(Vector3.up * 6f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.3f);

        if (_gotcha) // TODO: 낚시에 성공했을때 낚시찌 위치에 생선 띄운다. 
        {

        }

        bobberRigid.velocity = Vector3.zero; // 낚시찌 정지
        bobber.transform.position = poolPos; // 낚시찌 원상 복구 

        isFishing = false; // 다시 낚시를 할 수 있도록 함. 
    }

    // TODO: 낚시줄을 구현해야 한다. 
    // TODO: 물고기가 왼손 위로 올라와야 한다. 
}
