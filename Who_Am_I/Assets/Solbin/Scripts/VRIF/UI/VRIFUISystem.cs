using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFUISystem : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas = default;

    private VRIFAction vrifAction = default;
    private bool activateMenu = false;

    private void Start()
    {
        menuCanvas.SetActive(false);
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

    private void Update()
    {
        if (vrifAction.Player.UI_Menu.triggered) // 메뉴 키 클릭
        {
            if (!activateMenu) // 메뉴 비활성화 상태
            {
                menuCanvas?.SetActive(true); // 메뉴 활성화
                Time.timeScale = 0f; // 일시정지 
            }
            else if (activateMenu) // 메뉴 활성화 상태 
            {
                menuCanvas?.SetActive(false); // 메뉴 비활성화
                Time.timeScale = 1f; // 시간 정상화 
            }
        }
    }
}
