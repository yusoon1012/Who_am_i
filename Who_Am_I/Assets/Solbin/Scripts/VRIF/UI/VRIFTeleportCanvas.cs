using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTeleportCanvas : MonoBehaviour
{
    // 선택 이미지 (게임 오브젝트)
    [SerializeField] private GameObject select_Beach = default;
    [SerializeField] private GameObject select_Forest = default;
    [SerializeField] private GameObject select_Temple = default;
    [SerializeField] private GameObject select_Winter = default;
    [SerializeField] private GameObject select_Fall = default;
    // 선택 이미지 딕셔너리
    private Dictionary<int, GameObject> selectImgDic = new Dictionary<int, GameObject>();
    // 무엇이 선택되었는지 
    private int number = default;
    // VRIFAction
    private VRIFAction vrifAction = default;
    // 입력 딜레이 시간
    private bool waitInput = false;

    private void Start()
    {
        Setting();
    }

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
    }

    private void Setting()
    {
        selectImgDic[1] = select_Beach;
        selectImgDic[2] = select_Forest;
        selectImgDic[3] = select_Temple;
        selectImgDic[4] = select_Winter;
        selectImgDic[5] = select_Fall;

        number = 3; // UI 활성화 시 신전이 먼저 선택되도록 // TODO: 추후 지금 있는 지역의 텔레포트 홀이 먼저 표시되도록 변경해볼까
    }

    private void Update()
    {
        UIControl();
    }

    /// <summary>
    /// UI 조작
    /// </summary>
    private void UIControl()
    {
        if (vrifAction.Player.LeftController.ReadValue<Vector2>().y >= 0.7f) // 위로 
        {
            if (!waitInput)
            {
                waitInput = true;
                number -= 1;
            }
        }
        else if (vrifAction.Player.LeftController.ReadValue<Vector2>().y <= -0.7f) // 아래로
        {
            if (!waitInput)
            {
                waitInput = true;
                number += 1;
            }
        }

        if (number < 1) { number = 5; }// 최솟값인 Beach의 Key는 1이다
        else if (number > 5) { number = 1; }// 최대값인 Fall의 Key는 5이다.

        UIUpdate();

        if (vrifAction.Player.UI_Click.triggered)
        {
            // TODO: 이동로직 구현 
        }
    }

    /// <summary>
    /// 조작을 UI에 반영
    /// </summary>
    private void UIUpdate()
    {
        selectImgDic[number].SetActive(true);

        for (int i = 1; i <= selectImgDic.Count; i++)
        {
            if (i == number) { selectImgDic[i].SetActive(true); }
            else if (i != number) { selectImgDic[i].SetActive(false); }
        }
    }
}
