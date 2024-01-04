using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    // 지도 UI 오브젝트
    public GameObject allMapsObj;
    // 아이템 데이터 그룹 메인 오브젝트 트랜스폼
    public Transform mainObjTf;
    // 실제 플레이어 트랜스폼
    public Transform playerTf;
    // 지도 플레이어 마크 트랜스폼
    public Transform onMapPlayerTf;
    // 지도 퀘스트 마크 트랜스폼
    public Transform onMapQuestTf;
    // 지도 카메라 트랜스폼
    public Transform mapCameraTf;
    // 지도 체크 포인트 마크 트랜스폼
    public Transform[] onMapWarpTf = new Transform[3];
    // 체크 포인트들의 워프 지점을 배열로 가져오는 트랜스폼
    public Transform[] checkPointPositions = new Transform[3];
    // 0 : 실제 지형 트랜스폼
    // 1 : 지도 지형 트랜스폼
    public Transform[] mapSizeCheck = new Transform[2];
    // 스크린 컨트롤러 트랜스폼
    public Transform screenControllerTf;

    // 플레이어와 지도 카메라의 움직임 구분값
    public int moveCheck { get; set; } = default;
    
    // 체크 포인트들의 워프 지점을 저장하기 위한 딕셔너리
    Dictionary<int, Vector3> checkPointDic = new Dictionary<int, Vector3>();

    // 지도상의 플레이어 위치값
    private Vector3 onMapPlayerPosition = Vector3.zero;
    // 미리 지정한 체크 포인트 위치 값 배열 (X, Z)
    private float[] checkPointPosX = new float[10];
    private float[] checkPointPosZ = new float[10];
    // 실제 지형과 지도 지형의 배치된 거리 차이 값
    private float[] dragMap = new float[2];
    // 실제 지형과 지도 지형의 X, Z 축 배율 차이
    private float[] multipleMapSize = new float[2];

    private float[] distanceMapSize = new float[2];

    void Awake()
    {
        moveCheck = 0;
        checkPointPosX[0] = 23.31f;
        checkPointPosX[1] = -34.68f;
        checkPointPosZ[0] = -15.27f;
        checkPointPosZ[1] = 13.08f;

        multipleMapSize[0] = 4.08f;
        multipleMapSize[1] = 6.2f;
        distanceMapSize[0] = 9.8f;
        distanceMapSize[1] = 35.48f;
    }     // Awake()

    void Start()
    {
        AccountMapSize();

        onMapPlayerTf.GetComponent<MapMarkInfo>().StartInfoSetting("현재 위치", false, 0);
        onMapQuestTf.GetComponent<MapMarkInfo>().StartInfoSetting("진행 가능한 퀘스트", false, 0);

        for (int i = 0; i < 3; i++)
        {
            Vector3 saveCheckPointPos = new Vector3(checkPointPosX[i], 5f, checkPointPosZ[i]);

            onMapWarpTf[i].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, i + 1);
            checkPointDic.Add(i + 1, saveCheckPointPos);
            SettingOnMapCheckPoint(i, saveCheckPointPos);
        }
    }     // Start()

    private void SettingOnMapCheckPoint(int count, Vector3 checkPointPos)
    {
        float[] countPos = new float[2];

        CountDistanceMap(checkPointPos.x, 0, out countPos[0]);
        CountDistanceMap(checkPointPos.z, 1, out countPos[1]);

        onMapWarpTf[count].position = new Vector3(countPos[0], 50f, countPos[1]);
    }     // SettingOnMapCheckPoint()

    // 지도를 열고 UI 를 활성화 하는 함수
    public void OpenMap()
    {
        ResetCamera();
        allMapsObj.SetActive(true);
    }     // OpenMap()

    // 지도를 닫고 UI 를 비활성화 하는 함수
    public void ExitMap()
    {
        mapCameraTf.GetComponent<CameraControl>().EndMapCamera();
        allMapsObj.SetActive(false);
    }     // ExitMap()

    // 지도를 열 때 지도상의 카메라의 위치를 초기화 시키는 함수
    private void ResetCamera()
    {
        OnMapPlayerSetting();

        // 지도 카메라의 위치를 지도상의 플레이어 위치로 이동
        mapCameraTf.position = new Vector3(onMapPlayerTf.position.x, mapCameraTf.position.y, onMapPlayerTf.position.z);

        mapCameraTf.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }     // ResetCamera()

    // 워프 지점 순서를 체크하여 해당 워프 지점으로 플레이어가 워프하는 기능의 함수
    public Vector3 ConnectCheckPoint(int checkPoint, out Vector3 checkPointPosition)
    {
        if (checkPoint == 0)
        {
            checkPointPosition = Vector3.zero;
        }
        else if (checkPointDic.ContainsKey(checkPoint))
        {
            checkPointPosition = checkPointDic[checkPoint];
        }
        else
        {
            checkPointPosition = Vector3.zero;
        }

        if (checkPointPosition != Vector3.zero)
        {
            playerTf.position = checkPointPosition;

            mainObjTf.GetComponent<UIController>().AfterWarpExitMap();
            screenControllerTf.GetComponent<ScreenController>().ScreenEffect(0, 0.02f, 0.02f, 1f, 1);

            Debug.Log("체크 포인트 워프 성공!!");
        }
        else
        {
            Debug.Log("체크 포인트 워프 Error!!!");
        }
        
        return checkPointPosition;
    }     // ConnectCheckPoint()

    // 게임 시작 시 실제 플레이 맵과 지도 맵의 차이를 계산하는 함수
    private void AccountMapSize()
    {
        dragMap[0] = (mapSizeCheck[1].position.x - mapSizeCheck[0].position.x) + 20f;
        dragMap[1] = (mapSizeCheck[1].position.z - mapSizeCheck[0].position.z) - 110f;

        Debug.LogFormat("맵의 거리 차이 : {0}, {1}", dragMap[0], dragMap[1]);
    }     // AccountMapSize()

    // 지도를 활성화할 때 지도상의 플레이어 표식 위치를 실제 플레이어 위치에 기반해 계산하여 위치를 변경하는 함수
    private void OnMapPlayerSetting()
    {
        float[] countPosition = new float[2];

        CountDistanceMap(playerTf.position.x, 0, out countPosition[0]);
        CountDistanceMap(playerTf.position.z, 1, out countPosition[1]);

        onMapPlayerTf.position = new Vector3(countPosition[0], 50f, countPosition[1]);
    }     // OnMapPlayerSetting()

    private float CountDistanceMap(float pos, int dir, out float countPos)
    {
        float count = 0f;
        switch (dir)
        {
            case 0:
                if (pos >= 0)
                {
                    count = (pos * multipleMapSize[dir]) - distanceMapSize[dir];
                }
                else
                {
                    count = (pos * multipleMapSize[dir]) + distanceMapSize[dir];
                }
                break;
            case 1:
                if (pos >= 0)
                {
                    count = (pos * multipleMapSize[dir]) + distanceMapSize[dir];
                }
                else
                {
                    count = (pos * multipleMapSize[dir]) - distanceMapSize[dir];
                }
                break;
            default:
                count = 0f;
                break;
        }

        countPos = dragMap[dir] + count;
        
        return countPos;
    }     // CountDistanceMap()
}
