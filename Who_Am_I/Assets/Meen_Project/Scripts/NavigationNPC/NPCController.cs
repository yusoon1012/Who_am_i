using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    // 플레이어 트랜스폼
    public Transform playerTf;
    // 길안내 NPC 가 시작되는 지점 트랜스폼
    public Transform startNavigationZoneTf;
    // 길안내 NPC 트랜스폼
    public Transform navigationNpcTf;

    // 길안내 재시작 지점 트랜스폼
    public Transform restartNavigationZoneTf;
    // 대화창 오브젝트
    public GameObject dialogObj;
    // 대화창에서 NPC 이름 출력 텍스트
    public Text dialogNameText;
    // 대화창에서 대화 내용 출력 텍스트
    public Text dialogContentsText;
    // 대화창에서 "다음" 출력 텍스트
    public Text nextText;

    // 대화창 순서 구분 변수
    public int dialogController { get; set; } = default;
    // 플레이어가 길안내 NPC 에 접근한 상태인지 체크
    public bool navigationEnterNpc { get; set; } = false;
    // 길안내 NPC 의 기능이 동작중인 상태인지 체크
    public int onNavigationCheck { get; set; } = default;

    // NPC 대화 목록 딕셔너리
    Dictionary<string, DialogsMain> npcDialogs = new Dictionary<string, DialogsMain>();

    // 대화창이 모두 출력되고 "다음" 텍스트가 출력된 상태인지 체크
    private bool dialogEdge = false;
    
    void Awake()
    {
        dialogController = 0;
        onNavigationCheck = 0;
    }     // Awake()

    void Start()
    {
        // NPC 대화 목록 저장
        npcDialogs.Add("NavigationNPC", new Dialogs001());
    }     // Start()

    // 저장된 NPC 대화 목록 정보를 NPC 이름으로 가져오는 함수
    public DialogsMain ReadingDialogs(string name, out DialogsMain dialog)
    {
        // 대화 목록 딕셔너리에서 Key 값을 비교하여 존재하면 실행
        if (npcDialogs.ContainsKey(name))
        {
            // "dialog" 값을 해당 키 값으로 설정함
            dialog = npcDialogs[name];
        }
        else
        {
            dialog = null;
        }

        return dialog;
    }     // ReadingDialogs()

    public void PrintDialog(string npcName, string dialog)
    {
        playerTf.GetComponent<UIController>().uiController = 12;

        // 대화 창 오브젝트를 활성화함
        dialogObj.gameObject.SetActive(true);
        // 대화 창 이름을 가져온 대화 정보에서 NPC 이름으로 출력함
        dialogNameText.text = string.Format("{0}", npcName);
        // 대화 창 내용을 가져온 대화 정보에서 대화 순서를 참고하여 출력함
        dialogContentsText.text = string.Format("{0}", dialog);

        // 대화 창 출력이 완료된 이후 Next 텍스트가 출력되기 전까지 딜레이 시간을 진행하는 코루틴 함수를 실행
        StartCoroutine(DialogDelay());
    }     // PrintDialogs()

    // NPC 와 대화중일 때 전용 "확인" 키 정보를 확인하는 함수
    public void InputController()
    {
        // 길안내 NPC 와 근접한 상태일 때 실행
        if (navigationEnterNpc == true)
        {
            // dialogController 값에 따라 기능이 다르게 실행되게 함
            switch (dialogController)
            {
                // dialogController 값이 0 이면 실행
                case 0:
                    // uiController 값을 NPC 와 대화중인 상태로 변경시킴
                    playerTf.GetComponent<UIController>().uiController = 12;
                    // 대화 단계값에 따라 NPC 와 대화를 출력하는 함수를 실행함
                    PrintDialogs("NavigationNPC", dialogController);
                    // 대화 단계값을 1 로 변경시킴
                    dialogController = 1;
                    break;
                case 1:
                    // 다음 표시가 출력된 상태일 때
                    if (dialogEdge == true)
                    {
                        // 다음 표시를 비활성화 된 상태로 변경함
                        dialogEdge = false;
                        // 다음 텍스트를 비활성화 시킴
                        nextText.gameObject.SetActive(false);
                        // 대화 단계값에 따라 NPC 와 대화를 출력하는 함수를 실행함
                        PrintDialogs("NavigationNPC", dialogController);
                        // 대화 단계값을 2 로 변경시킴
                        dialogController = 2;
                    }
                    break;
                case 2:
                    // 다음 표시가 출력된 상태일 때
                    if (dialogEdge == true)
                    {
                        dialogEdge = false;
                        nextText.gameObject.SetActive(false);
                        PrintDialogs("NavigationNPC", dialogController);
                        // 대화 단계값을 3 으로 변경시킴
                        dialogController = 3;
                    }
                    break;
                case 3:
                    // 다음 표시가 출력된 상태일 때
                    if (dialogEdge == true)
                    {
                        // NPC 와 대화가 종료되어 uiController 값을 일반 상태로 변경시킴
                        playerTf.GetComponent<UIController>().uiController = 0;
                        dialogEdge = false;
                        //** NPC 와 대화를 종료하고 모든 값을 초기화함
                        navigationEnterNpc = false;
                        nextText.gameObject.SetActive(false);
                        dialogNameText.text = string.Format(" ");
                        dialogContentsText.text = string.Format(" ");
                        dialogObj.gameObject.SetActive(false);
                        // 길안내 NPC 의 상태를 길안내를 진행중인 상태로 변경함
                        onNavigationCheck = 1;
                        // 길안내 NPC 의 길안내 기능 스크립트를 활성화 함
                        navigationNpcTf.gameObject.GetComponent<MoveNavigation>().enabled = true;
                        // 길안내 시작 지점의 스크립트를 비활성화 함
                        startNavigationZoneTf.gameObject.GetComponent<StartNavigation>().enabled = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }     // InputController()

    // 저장된 NPC 대화 목록을 가져와 대화창의 내용을 출력하는 함수
    private void PrintDialogs(string npcName, int dialogCount)
    {
        DialogsMain dialog = new DialogsMain();

        // 저장된 NPC 대화 목록 정보를 NPC 이름으로 가져오는 함수를 실행시켜 대화 정보를 가져옴
        ReadingDialogs(npcName, out dialog);

        // 대화 창 오브젝트를 활성화함
        dialogObj.gameObject.SetActive(true);
        // 대화 창 이름을 가져온 대화 정보에서 NPC 이름으로 출력함
        dialogNameText.text = string.Format("{0}", dialog.npcName);
        // 대화 창 내용을 가져온 대화 정보에서 대화 순서를 참고하여 출력함
        dialogContentsText.text = string.Format("{0}", dialog.dialogs[dialogCount]);

        // 대화 창 출력이 완료된 이후 다음 텍스트가 출력되기 전까지 딜레이 시간을 진행하는 코루틴 함수를 실행
        StartCoroutine(DialogDelay());
    }     // PrintDialogs()

    // 대화창이 모두 출력되고 "다음"이 출력되기 전까지 딜레이 시간을 주는 코루틴 함수
    IEnumerator DialogDelay()
    {
        yield return new WaitForSeconds(1f);

        // 다음 표시를 활성화 된 상태로 변경함
        dialogEdge = true;
        // 다음 표시 텍스트를 활성화 시킴
        nextText.gameObject.SetActive(true);
    }     // DialogDelay()

    // 길안내 NPC 와 대화중인 상태일 때 마다 Update 에서 실시간으로 실행시키는 함수
    public void ConnectUpdateFunction()
    {
        // 길안내 NPC 의 길안내 기능 스크립트에 있는 실시간 진행 함수를 실행 시킴
        navigationNpcTf.GetComponent<MoveNavigation>().UpdateFunction();
    }     // ConnectUpdateFunction()

    // 길안내 NPC 를 플레이어가 따라가지 않고 일정시간이 지난 후 안내창을 출력하는 함수
    public void LeavePlayerDialog()
    {
        DialogsMain dialog = new DialogsMain();

        // 저장된 NPC 대화 목록 정보를 NPC 이름으로 가져오는 함수를 실행시켜 대화 정보를 가져옴
        ReadingDialogs("NavigationNPC", out dialog);

        // 대화 창 오브젝트를 활성화함
        dialogObj.gameObject.SetActive(true);
        // 대화 창 이름을 가져온 대화 정보에서 NPC 이름으로 출력함
        dialogNameText.text = string.Format("{0}", dialog.npcName);
        // 대화 창 내용을 가져온 대화 정보에서 대화 순서를 참고하여 출력함
        dialogContentsText.text = string.Format("{0}", dialog.dialogs[3]);

        // 대화 창을 출력한 뒤 일정 시간 이후 대화 창을 비활성화 하는 기능의 코루틴 함수를 실행함
        StartCoroutine(EndLeavePlayerDialog());
    }     // LeavePlayerDialog()

    // 대화 창을 출력한 뒤 일정 시간 이후 대화 창을 비활성화 하는 기능의 코루틴 함수
    IEnumerator EndLeavePlayerDialog()
    {
        yield return new WaitForSeconds(3f);

        //** NPC 와 대화를 종료하고 모든 값을 초기화함
        dialogNameText.text = string.Format(" ");
        dialogContentsText.text = string.Format(" ");
        dialogObj.gameObject.SetActive(false);
    }     // EndLeavePlayerDialog()

    // 길안내 NPC 가 길안내를 잠시 중지하는 기능의 함수
    public void StopNavigationNPC()
    {
        // 길안내 NPC 의 상태를 길안내를 중지한 상태로 변경함
        onNavigationCheck = 2;
        // 길안내 NPC 의 재시작 전용 범위 오브젝트를 활성화 함
        restartNavigationZoneTf.gameObject.SetActive(true);
        // 길안내 재시작 전용 범위 오브젝트 위치를 길안내 NPC 의 위치로 이동시킴
        restartNavigationZoneTf.position = navigationNpcTf.position;
        // 길안내 NPC 를 비활성화 함
        navigationNpcTf.gameObject.SetActive(false);
    }     // StopNavigationNPC()

    // 길안내 NPC 가 길안내를 재시작 하는 기능을 실행하는 함수
    public void RestartNavigationNPC()
    {
        // 길안내 NPC 의 상태를 길안내를 진행중인 상태로 변경함
        onNavigationCheck = 1;
        // 길안내 재시작 전용 범위 오브젝트를 비활성화 함
        restartNavigationZoneTf.gameObject.SetActive(false);
        // 길안내 NPC 를 활성화 함
        navigationNpcTf.gameObject.SetActive(true);
        // 임시 비활성화 상태에서 활성화 되고 길안내를 재시작 할 때 딜레이 시간을 진행하는 코루틴 함수를 실행함
        navigationNpcTf.GetComponent<MoveNavigation>().RestartDelay();
    }     // RestartNavigationNPC()
}
