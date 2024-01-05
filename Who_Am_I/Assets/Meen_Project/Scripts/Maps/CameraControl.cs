//using UnityEditor.PackageManager;
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

    // 플레이어 트랜스폼
    public Transform playerTf;

    // 지도상의 표식들을 체크하는 레이어 마스크
    public LayerMask mapMarkLayer;

    // 지도 카메라가 움직이는 속도값
    public float speed = default;
    // 지도 카메라를 확대, 축소하는 속도값
    public float zoomSpeed = default;

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
    private float[] map01LimitCamera = new float[4];

    //// 지도상의 플레이어 트랜스폼
    //public Transform onMapPlayerTf;

    void Awake()
    {
        speed = 1f;
        zoomSpeed = 0.2f;
        checkPointInfoCursor = 0;
        map01LimitCamera[0] = 11600f;
        map01LimitCamera[1] = 8400f;
        map01LimitCamera[2] = 8700f;
        map01LimitCamera[3] = 11200f;
    }     // Awake()

    void Start()
    {
        mapCamera = GetComponent<Camera>();
    }     // Start()

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
        switch (arrowType)
        {
            case 0:
                inCamera = true;
                break;
            case 1:
                outCamera = true;
                break;
            default:
                break;
        }
    }     // CheckInCamera()

    // 지도 카메라를 축소하는 입력키 값을 구분하는 함수
    public void CheckOutCamera(int arrowType)
    {
        switch (arrowType)
        {
            case 0:
                inCamera = false;
                break;
            case 1:
                outCamera = false;
                break;
            default:
                break;
        }
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
                moveCount[1] += speed;
                //if (mapCamera.transform.position.z >= map01LimitCamera[0])
                //{
                //    moveCount[1] = map01LimitCamera[0];
                //}
                //else
                //{
                //    moveCount[1] += speed;
                //}
            }
            else if (moveCamera[1] == true)
            {
                moveCount[1] -= speed;
                //if (mapCamera.transform.position.z <= map01LimitCamera[1])
                //{
                //    moveCount[1] = map01LimitCamera[1];
                //}
                //else
                //{
                //    moveCount[1] -= speed;
                //}
            }
            else if (moveCamera[2] == true)
            {
                moveCount[0] -= speed;
                //if (mapCamera.transform.position.x <= map01LimitCamera[2])
                //{
                //    moveCount[0] = map01LimitCamera[2];
                //}
                //else
                //{
                //    moveCount[0] -= speed;
                //}
            }
            else if (moveCamera[3] == true)
            {
                moveCount[0] += speed;
                //if (mapCamera.transform.position.x >= map01LimitCamera[3])
                //{
                //    moveCount[0] = map01LimitCamera[3];
                //}
                //else
                //{
                //    moveCount[0] += speed;
                //}
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

        playerTf.GetComponent<UIController>().uiController = 11;
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

        playerTf.GetComponent<UIController>().uiController = 10;
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
                //FunctionCheckPoint();
                break;
            // 현재 활성화 된 커서값이 1 이면
            case 1:
                ExitCheckPointInfo();
                break;
            default:
                break;
        }
    }     // SelectCheckPointButton()

    // 체크 포인트 워프를 실행하는 함수
    public void FunctionCheckPoint()
    {
        // 현재 지도의 크로스헤어가 체크 포인트를 가리키고, 레이에 맞은 표식이 null 값이 아닐 때 실행
        if (isWarpMarkCheck == true && nowSelectMapMark != null)
        {
            nowSelectMapMark.GetComponent<MapMarkInfo>().SendCountInfo(out int warpCount);
            mapController.GetComponent<MapControl>().ConnectCheckPoint(warpCount, out Vector3 checkPointPosition);

            //float[] countPosition = new float[2];
            //float[] disMapSize = new float[2];
            //float dragMapSize = mapController.GetComponent<MapControl>().dragMap;

            //disMapSize[0] = mapController.GetComponent<MapControl>().distanceMapSize[0];
            //disMapSize[1] = mapController.GetComponent<MapControl>().distanceMapSize[1];

            //countPosition[0] = (checkPointPosition.x / disMapSize[0]) + dragMapSize;
            //countPosition[1] = (checkPointPosition.z / disMapSize[1]) + dragMapSize;
            //Vector3 resultPosition = new Vector3(countPosition[0], 20f, countPosition[1]);

            //onMapPlayerTf.position = resultPosition;
        }
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
}
