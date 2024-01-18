//using UnityEditor.PackageManager;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    // 맵 컨트롤러 트랜스폼
    public Transform mapController;
    // 지도상에서 표식 정보 이미지
    public Image mapMarkImage;
    // 지도상에서 표식 정보 텍스트
    public Text mapMarkInfoUiText;
    // 체크 포인트 워프 체크 UI 이미지
    public Image warpInfoUI;
    // 체크 포인트 워프 체크 UI 버튼
    public Image[] warpCheckButton = new Image[2];

    public Image blackScreen;

    // 플레이어 트랜스폼
    public Transform playerTf;
    // 아이템 데이터 그룹 메인 오브젝트 트랜스폼
    public Transform mainObjTf;

    // 지도상의 표식들을 체크하는 레이어 마스크
    public LayerMask mapMarkLayer;

    // 지도 카메라가 움직이는 속도값
    public float speed = default;
    // 지도 카메라를 확대, 축소하는 속도값
    public float zoomSpeed = default;

    public float changeAlphaCount = default;

    public float changeAlphaTime = default;

    public int mapTypeCheck = default;

    // 지도 카메라 참조
    private Camera mapCamera = default;
    // 지도상에서 정보를 출력중인 표식을 체크
    private GameObject nowSelectMapMark = default;

    // 지도 카메라가 확대되는 상태 체크
    private bool inCamera = false;
    // 지도 카메라가 축소되는 상태 체크
    private bool outCamera = false;

    // 0 : 위 방향
    // 1 : 아래 방향
    // 2 : 왼쪽 방향
    // 3 : 오른쪽 방향
    private bool[] moveCamera = new bool[4];
    // 지도상에 표시하고 있는 표식이 워프가 가능한지 여부 체크
    private bool isWarpMarkCheck = false;
    // 워프 포인트 체크 UI 내의 커서 값
    private int checkPointInfoCursor = default;
    // 1 번째 봄 맵 상, 하, 좌, 우 카메라 한계값
    private float[,] mapLimitCamera = new float[4, 4];

    private int warpOrder = default;

    private float[,] pivotMaps = new float[4, 2];

    //// 지도상의 플레이어 트랜스폼
    //public Transform onMapPlayerTf;

    void Awake()
    {
        speed = 1.75f;
        zoomSpeed = 0.2f;
        checkPointInfoCursor = 0;
        warpOrder = 0;
        changeAlphaCount = 0.05f;
        changeAlphaTime = 0.04f;
        mapTypeCheck = 0;

        // 봄 지도의 중심점 좌표
        pivotMaps[0, 0] = 1000f;
        pivotMaps[0, 1] = 1000f;
        // 여름 지도의 중심점 좌표
        pivotMaps[1, 0] = 2000f;
        pivotMaps[1, 1] = 1000f;
        // 가을 지도의 중심점 좌표
        pivotMaps[2, 0] = 1000f;
        pivotMaps[2, 1] = 2500f;
        // 겨울 지도의 중심점 좌표
        pivotMaps[3, 0] = 2000f;
        pivotMaps[3, 1] = 2500f;

        // 봄 지도의 카메라 한계값
        mapLimitCamera[0, 0] = 1460f;
        mapLimitCamera[0, 1] = 765f;
        mapLimitCamera[0, 2] = 690f;
        mapLimitCamera[0, 3] = 1270f;
        // 여름 지도의 카메라 한계값
        mapLimitCamera[1, 0] = 1300f;
        mapLimitCamera[1, 1] = 625f;
        mapLimitCamera[1, 2] = 1730f;
        mapLimitCamera[1, 3] = 2310f;
        // 가을 지도의 카메라 한계값
        mapLimitCamera[2, 0] = 2845f;
        mapLimitCamera[2, 1] = 2150f;
        mapLimitCamera[2, 2] = 695f;
        mapLimitCamera[2, 3] = 1275f;
        // 겨울 지도의 카메라 한계값
        mapLimitCamera[3, 0] = 2850f;
        mapLimitCamera[3, 1] = 2150f;
        mapLimitCamera[3, 2] = 1710f;
        mapLimitCamera[3, 3] = 2290f;
    }     // Awake()

    void Start()
    {
        mapCamera = GetComponent<Camera>();
    }     // Start()

    public void CheckMapType(int type)
    {
        mapTypeCheck = type;
    }     // CheckMapType()

    // 지도 카메라를 움직이는 방향키 값을 구분하는 함수
    public void MoveMap(int arrowType)
    {
        switch (arrowType)
        {
            case 0:
                moveCamera[0] = true;
                break;
            case 1:
                moveCamera[1] = true;
                break;
            case 2:
                moveCamera[2] = true;
                break;
            case 3:
                moveCamera[3] = true;
                break;
            default:
                break;
        }
    }     // MoveMap()

    // 지도 카메라를 멈추도록 방향키를 떼었을 때 방향키 값을 구분하는 함수
    public void StopMoveMap(int arrowType)
    {
        switch (arrowType)
        {
            case 0:
                moveCamera[0] = false;
                break;
            case 1:
                moveCamera[1] = false;
                break;
            case 2:
                moveCamera[2] = false;
                break;
            case 3:
                moveCamera[3] = false;
                break;
            default:
                break;
        }
    }     // StopMoveMap()

    // 지도 카메라를 확대하는 입력키 값을 구분하는 함수
    public void CheckInCamera(int arrowType)
    {
        //switch (arrowType)
        //{
        //    case 0:
        //        inCamera = true;
        //        break;
        //    case 1:
        //        outCamera = true;
        //        break;
        //    default:
        //        break;
        //}
    }     // CheckInCamera()

    // 지도 카메라를 축소하는 입력키 값을 구분하는 함수
    public void CheckOutCamera(int arrowType)
    {
        //switch (arrowType)
        //{
        //    case 0:
        //        inCamera = false;
        //        break;
        //    case 1:
        //        outCamera = false;
        //        break;
        //    default:
        //        break;
        //}
    }     // CheckOutCamera()

    // Update 마다 조건에 맞으면 실행되는 함수
    public void UpdateFunction()
    {
        // 지도 카메라에서 레이를 발사하는 함수를 실행
        MapMarkRayPoint();

        // 카메라를 움직이는 값이 하나라도 존재하면 실행
        if (moveCamera[0] == true || moveCamera[1] == true || moveCamera[2] == true || moveCamera[3] == true)
        {
            float[] moveCount = new float[2];

            // 지도상의 카메라의 X, Y 축 위치값을 가져옴
            moveCount[0] = mapCamera.transform.position.x;
            moveCount[1] = mapCamera.transform.position.z;

            // 카메라 움직이는 키 값에 따라 카메라 위치값을 변경시킴
            if (moveCamera[0] == true)
            {
                if (mapCamera.transform.position.z >= mapLimitCamera[mapTypeCheck, 0])
                {
                    moveCount[1] = mapLimitCamera[mapTypeCheck, 0];
                }
                else
                {
                    moveCount[1] += speed;
                }
            }
            else if (moveCamera[1] == true)
            {
                if (mapCamera.transform.position.z <= mapLimitCamera[mapTypeCheck, 1])
                {
                    moveCount[1] = mapLimitCamera[mapTypeCheck, 1];
                }
                else
                {
                    moveCount[1] -= speed;
                }
            }
            else if (moveCamera[2] == true)
            {
                if (mapCamera.transform.position.x <= mapLimitCamera[mapTypeCheck, 2])
                {
                    moveCount[0] = mapLimitCamera[mapTypeCheck, 2];
                }
                else
                {
                    moveCount[0] -= speed;
                }
            }
            else if (moveCamera[3] == true)
            {
                if (mapCamera.transform.position.x >= mapLimitCamera[mapTypeCheck, 3])
                {
                    moveCount[0] = mapLimitCamera[mapTypeCheck, 3];
                }
                else
                {
                    moveCount[0] += speed;
                }
            }

            // 변경된 위치값을 지도상의 카메라의 위치에 대입함
            mapCamera.transform.position = new Vector3(moveCount[0], 500f, moveCount[1]);
        }

        //// 지도 카메라 확대, 축소 기능 함수 실행
        //else if (inCamera == true || outCamera == true) { MoveInOutCamera(); }
    }     // UpdateFunction()

    // 지도 카메라에서 레이를 발사하는 함수
    private void MapMarkRayPoint()
    {
        // 지도 카메라 중앙으로 레이를 발사함
        Ray ray = mapCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // 지도상의 표식 레이어 마스크가 레이에 맞으면
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mapMarkLayer))
        {
            // 현재 정보를 표시하고 있는 표식이면 정보 출력 기능을 스킵
            if (hit.collider.gameObject == nowSelectMapMark) { return; }

            // 표식마다 보유중인 표식 정보와 워프 가능 여부값을 가져옴
            hit.collider.gameObject.GetComponent<MapMarkInfo>().SendStringInfo(out string mapMarkInfoText);
            hit.collider.gameObject.GetComponent<MapMarkInfo>().SendBoolInfo(out bool mapMarkWarpCheck);

            // 현재 정보를 표시하고 있는 표식으로 저장
            nowSelectMapMark = hit.collider.gameObject;
            // 현재 표시중인 표식이 워프가 가능한지 여부 저장
            isWarpMarkCheck = mapMarkWarpCheck;

            // 지도상의 표식 정보창을 활성화하고 표식 정보를 출력함
            mapMarkImage.gameObject.SetActive(true);
            mapMarkInfoUiText.text = string.Format("{0}", mapMarkInfoText);
        }
        // 레이가 표식 레이어 마스크에 맞지 않으면
        else
        {
            // 지도상의 표식 정보창을 비활성화하고 표식 정보와 현재 정보를 표시하고 있는 표식을 초기화함
            mapMarkInfoUiText.text = string.Format(" ");
            mapMarkImage.gameObject.SetActive(false);
            nowSelectMapMark = null;

            if (isWarpMarkCheck == true) { isWarpMarkCheck = false; }
        }
    }     // MapMarkRayPoint()

    // 체크 포인트를 눌렀을 때 워프 확인창을 활성화 하는 함수
    public void SelectCheckPoint()
    {
        if (isWarpMarkCheck == false) { return; }

        mainObjTf.GetComponent<UIController>().uiController = 11;
        checkPointInfoCursor = 0;
        warpInfoUI.gameObject.SetActive(true);

        Color imageColor = warpCheckButton[0].color;
        imageColor.a = 1f;
        warpCheckButton[0].color = imageColor;
    }     // SelectCheckPoint()

    // 활성화 된 워프 확인창의 버튼 값을 변경하는 함수
    public void ChangeCheckPointButton(int arrowType)
    {
        Color beforeImageColor = warpCheckButton[checkPointInfoCursor].color;
        beforeImageColor.a = 0.2f;
        warpCheckButton[checkPointInfoCursor].color = beforeImageColor;

        if (arrowType == 0 || arrowType == 2)
        {
            if (checkPointInfoCursor == 0) { checkPointInfoCursor = 1; }
            else { checkPointInfoCursor -= 1; }
        }
        else if (arrowType == 1 || arrowType == 3)
        {
            if (checkPointInfoCursor == 1) { checkPointInfoCursor = 0; }
            else { checkPointInfoCursor += 1; }
        }

        Color afterImageColor = warpCheckButton[checkPointInfoCursor].color;
        afterImageColor.a = 1f;
        warpCheckButton[checkPointInfoCursor].color = afterImageColor;
    }     // ChangeCheckPointButton()

    // 활성화된 체크 포인트 워프 확인창을 비활성화 하는 함수
    public void ExitCheckPointInfo()
    {
        Color imageColor = warpCheckButton[checkPointInfoCursor].color;
        imageColor.a = 0.2f;
        warpCheckButton[checkPointInfoCursor].color = imageColor;

        mainObjTf.GetComponent<UIController>().uiController = 10;
        checkPointInfoCursor = 0;
        warpInfoUI.gameObject.SetActive(false);
    }     // ExitCheckPointInfo()

    // 활성화 된 워프 확인창의 버튼을 눌렀을 때 기능을 전달하는 함수
    public void SelectCheckPointButton()
    {
        switch (checkPointInfoCursor)
        {
            // 현재 활성화 된 커서값이 0 이면
            case 0:
                ReadyCheckPoint();
                break;
            // 현재 활성화 된 커서값이 1 이면
            case 1:
                ExitCheckPointInfo();
                break;
            default:
                break;
        }
    }     // SelectCheckPointButton()

    // 체크 포인트 기능을 실행하기 전 준비 단계 함수
    public void ReadyCheckPoint()
    {
        // 현재 지도의 크로스헤어가 체크 포인트를 가리키고, 레이에 맞은 표식이 null 값이 아닐 때 실행
        if (isWarpMarkCheck == true && nowSelectMapMark != null)
        {
            warpOrder = 0;

            // 체크 포인트 번호 값을 가져옴
            nowSelectMapMark.GetComponent<MapMarkInfo>().SendCountInfo(out int warpCount);
            // 체크 포인트 번호 값을 전역 변수로 저장함
            warpOrder = warpCount;
            // 빠른 이동 시작 시 모든 메뉴 UI 를 종료함
            mainObjTf.GetComponent<UIController>().AllExitMapUI();

            // 화면 암전 효과 기능 코루틴 실행
            StartCoroutine(TeleportScreen());
        }
    }     // ReadyCheckPoint()

    // 체크 포인트 빠른 이동 시 화면 암전 효과 기능 코루틴 함수
    IEnumerator TeleportScreen()
    {
        Color screenColor = default;
        float nowAlpha = 0f;
        // 암전 효과 스크린을 활성화함
        blackScreen.gameObject.SetActive(true);

        while (true)
        {
            // 스크린의 알파값이 1f 값이 되면 while 문 종료
            if (nowAlpha >= 1f) { break; }

            // 스크린의 알파값을 미리 지정한 값만큼 증가
            screenColor.a = nowAlpha += changeAlphaCount;
            blackScreen.color = screenColor;

            // 미리 지정한 대기 시간만큼 한 프레임 딜레이
            yield return new WaitForSeconds(changeAlphaTime);
        }

        // 체크 포인트 빠른 이동 기능의 함수를 실행함
        FunctionCheckPoint();
    }     // TeleportScreen()

    // 체크 포인트 빠른 이동 종료 시 화면 암전 효과 종료 기능 코루틴 함수
    IEnumerator EndTeleportScreen()
    {
        Color screenColor = default;
        float nowAlpha = 1f;

        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            // 스크린의 알파값이 0f 값이 되면 while 문 종료
            if (nowAlpha <= 0f) { break; }

            // 스크린의 알파값을 미리 지정한 값만큼 감소
            screenColor.a = nowAlpha -= changeAlphaCount;
            blackScreen.color = screenColor;

            // 미리 지정한 대기 시간만큼 한 프레임 딜레이
            yield return new WaitForSeconds(changeAlphaTime);
        }

        // 암전 효과 스크린을 비활성화함
        blackScreen.gameObject.SetActive(false);
    }     // EndTeleportScreen()
    
    // 체크 포인트 빠른 이동 기능을 실행하는 함수
    private void FunctionCheckPoint()
    {
        // 체크 포인트 빠른 이동 기능 클래스의 함수를 실행함
        VRIFSceneManager.Instance.LoadCheckPoint("Spring", warpOrder);

        Debug.LogFormat("빠른 이동 체크포인트 번호 : {0}", warpOrder);
        Debug.Log("빠른 이동 기능 구현 후 지도 UI 모두 종료");

        // 화면 암전 효과 종료 기능 코루틴 실행
        StartCoroutine(EndTeleportScreen());
        
        warpOrder = 0;
    }     // FunctionCheckPoint()

    // 지도 카메라 확대, 축소 기능 함수
    public void MoveInOutCamera()
    {
        // 카메라 표시 거리가 100 보다 클 때에만 카메라 확대를 실행함
        if (inCamera == true && mapCamera.orthographicSize >= 100f)
        {
            mapCamera.orthographicSize -= zoomSpeed;
        }
        // 카메라 표시 거리가 300 보다 작을 때에만 카메라 축소를 실행함
        else if (outCamera == true && mapCamera.orthographicSize <= 500f)
        {
            mapCamera.orthographicSize += zoomSpeed;
        }
    }     // InOutCamera()

    // 지도를 비활성화 할 때 지도의 확대, 축소 값을 초기화하는 함수
    public void EndMapCamera()
    {
        mapCamera.orthographicSize = 150f;
    }     // EndMapCamera()

    public void ChangeMaps(int arrowType)
    {
        int changeMap = 0;

        switch (arrowType)
        {
            case 2:
                if (mapTypeCheck == 0)
                {
                    changeMap = 3;
                }
                else
                {
                    changeMap = mapTypeCheck - 1;
                }
                break;
            case 3:
                if (mapTypeCheck == 3)
                {
                    changeMap = 0;
                }
                else
                {
                    changeMap = mapTypeCheck + 1;
                }
                break;
            default:
                break;
        }

        ChangeCamera(changeMap);
    }     // ChangeMaps()

    private void ChangeCamera(int mapType)
    {
        mapCamera.transform.position = new Vector3(pivotMaps[mapType, 0], 500f, pivotMaps[mapType, 1]);

        mapTypeCheck = mapType;
    }     // ChangeCamera()
}
