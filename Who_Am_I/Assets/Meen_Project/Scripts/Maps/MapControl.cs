using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Transform[] onMapWarpTf = new Transform[16];
    //// 0 : 실제 지형 트랜스폼
    //// 1 : 지도 지형 트랜스폼
    //public Transform[] mapSizeCheck = new Transform[2];
    // 지도 메뉴얼 오브젝트
    public GameObject mapManualObj;

    // 플레이어와 지도 카메라의 움직임 구분값
    public int moveCheck { get; set; } = default;

    public int moveMapCheck { get; set; } = default;

    #region 봄 지도 초기 설정

    // 실제 지형과 지도 지형의 배치된 거리 차이 값
    private float[] dragMap_spring = new float[2];
    // 실제 지형과 지도 지형의 X, Z 축 배율 차이
    private float[] multipleMapSize_spring = new float[2];
    // 실제 지형과 지도 지형의 X, Z 축 거리 차이
    private float[] distanceMapSize_spring = new float[2];

    #endregion 봄 지도 초기 설정

    #region 여름 지도 초기 설정

    // 실제 지형과 지도 지형의 배치된 거리 차이 값
    private float[] dragMap_summer = new float[2];
    // 실제 지형과 지도 지형의 X, Z 축 배율 차이
    private float[] multipleMapSize_summer = new float[2];
    // 실제 지형과 지도 지형의 X, Z 축 거리 차이
    private float[] distanceMapSize_summer = new float[2];

    #endregion 여름 지도 초기 설정

    #region 가을 지도 초기 설정

    private float[] dragMap_autumn = new float[2];

    private float[] multipleMapSize_autumn = new float[2];

    private float[] distanceMapSize_autumn = new float[2];

    private float[] pivotAutumnSize = new float[2];

    #endregion 가을 지도 초기 설정

    #region 겨울 지도 초기 설정

    private float[] dragMap_winter = new float[2];

    private float[] multipleMapSize_winter = new float[2];

    private float[] distanceMapSize_winter = new float[2];

    private float[] pivotWinterSize = new float[2];

    #endregion 겨울 지도 초기 설정

    // 미리 지정한 체크 포인트 위치 값 배열 (X, Z)
    private float[] checkPointPosX = new float[16];
    private float[] checkPointPosZ = new float[16];

    // 스크린 컨트롤러 트랜스폼
    //public Transform screenControllerTf;
    // 체크 포인트들의 워프 지점을 배열로 가져오는 트랜스폼
    //public Transform[] checkPointPositions = new Transform[3];
    //// 체크 포인트들의 워프 지점을 저장하기 위한 딕셔너리
    //Dictionary<int, Vector3> checkPointDic = new Dictionary<int, Vector3>();

    // 현재 있는 씬을 알 수 있는 방법 : SceneManager.GetActiveScene().name

    void Awake()
    {
        // 봄 맵 체크포인트
        checkPointPosX[0] = 23.31f;
        checkPointPosX[1] = -34.68f;
        // 여름 맵 체크포인트
        checkPointPosX[2] = -47.4f;
        checkPointPosX[3] = -15.24f;
        checkPointPosX[4] = 65.64f;
        // 가을 맵 체크포인트
        checkPointPosX[5] = 3399.905f;
        checkPointPosX[6] = 3237.66f;
        checkPointPosX[7] = 3366.08f;
        checkPointPosX[8] = 3420.412f;
        checkPointPosX[9] = 3438f;
        checkPointPosX[10] = 3373.37f;
        checkPointPosX[11] = 3360.22f;
        // 겨울 맵 체크포인트
        checkPointPosX[12] = -50.7f;
        checkPointPosX[13] = 8.97f;
        checkPointPosX[14] = 67.61f;
        checkPointPosX[15] = 53.29f;

        // 봄 맵 체크포인트
        checkPointPosZ[0] = -15.27f;
        checkPointPosZ[1] = 13.08f;
        // 여름 맵 체크포인트
        checkPointPosZ[2] = 11.8f;
        checkPointPosZ[3] = 78.45f;
        checkPointPosZ[4] = 28.918f;
        // 가을 맵 체크포인트
        checkPointPosZ[5] = 1900.497f;
        checkPointPosZ[6] = 1897.05f;
        checkPointPosZ[7] = 1855.31f;
        checkPointPosZ[8] = 1902.941f;
        checkPointPosZ[9] = 1887.455f;
        checkPointPosZ[10] = 1794.83f;
        checkPointPosZ[11] = 1713.06f;
        // 겨울 맵 체크포인트
        checkPointPosZ[12] = 39.25f;
        checkPointPosZ[13] = 6.172f;
        checkPointPosZ[14] = 40.05f;
        checkPointPosZ[15] = 112.76f;

        moveCheck = 0;
        moveMapCheck = 0;

        #region 봄 지도 초기 설정

        // 실제 지형과 지도 이미지의 실제 거리 차이
        dragMap_spring[0] = 1000f;
        dragMap_spring[1] = 1000f;
        // 실제 지형과 지도 이미지의 축적 배율
        multipleMapSize_spring[0] = 4.03f;
        multipleMapSize_spring[1] = 6.2f;
        // 실제 지형과 지도 이미지의 중심 축 이동 배율 값 (x 축 음수, z 축 양수)
        distanceMapSize_spring[0] = 9.92f;
        distanceMapSize_spring[1] = 36.66f;

        #endregion 봄 지도 초기 설정

        #region 여름 지도 초기 설정

        // 실제 지형과 지도 이미지의 실제 거리 차이
        dragMap_summer[0] = 2000f;
        dragMap_summer[1] = 1000f;
        // 실제 지형과 지도 이미지의 축적 배율
        multipleMapSize_summer[0] = 1.45f;
        multipleMapSize_summer[1] = 2.99f;
        // 실제 지형과 지도 이미지의 중심 축 이동 배율 값 (x 축 양수, z 축 음수)
        distanceMapSize_summer[0] = 13.79f;
        distanceMapSize_summer[1] = 8.36f;

        #endregion 여름 지도 초기 설정

        #region 가을 지도 초기 설정

        // 실제 지형과 지도 이미지의 실제 거리 차이
        dragMap_autumn[0] = 1000f;
        dragMap_autumn[1] = 2500f;
        // 실제 지형과 지도 이미지의 축적 배율
        multipleMapSize_autumn[0] = 1.82f;
        multipleMapSize_autumn[1] = 2.92f;
        // 실제 지형과 지도 이미지의 중심 축 이동 배율 값 (x 축 음수, z 축 음수)
        distanceMapSize_autumn[0] = 10.98f;
        distanceMapSize_autumn[1] = 109.58f;
        // 가을 지도에서만 실제 맵에서 중심축이 3371 / 1850
        pivotAutumnSize[0] = 3371f;
        pivotAutumnSize[1] = 1850f;
        //pivotAutumnSize[1] = 1850f;

        #endregion 가을 지도 초기 설정

        #region 겨울 지도 초기 설정

        // 실제 지형과 지도 이미지의 실제 거리 차이
        dragMap_winter[0] = 2000f;
        dragMap_winter[1] = 2500f;
        // 실제 지형과 지도 이미지의 축적 배율
        multipleMapSize_winter[0] = 2.39f;
        multipleMapSize_winter[1] = 4.07f;
        // 실제 지형과 지도 이미지의 중심 축 이동 배율 값 (x 축 양수, z 축 양수)
        distanceMapSize_winter[0] = 0f;
        distanceMapSize_winter[1] = 0f;
        // 겨울 지도에서만 실제 맵에서 중심축이 14 / 82
        pivotWinterSize[0] = 14f;
        pivotWinterSize[1] = 82f;

        #endregion 겨울 지도 초기 설정

    }     // Awake()

    void Start()
    {
        // 현재 플레이어 위치와 진행 가능한 퀘스트 표식들의 정보를 저장
        onMapPlayerTf.GetComponent<MapMarkInfo>().StartInfoSetting("현재 위치", false, 0);
        onMapQuestTf.GetComponent<MapMarkInfo>().StartInfoSetting("진행 가능한 퀘스트", false, 0);

        for (int i = 0; i < 2; i++)
        {
            Vector3 saveCheckPointPos = new Vector3(checkPointPosX[i], 5f, checkPointPosZ[i]);

            onMapWarpTf[i].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, i);
            SettingOnMapCheckPoint(i, saveCheckPointPos, 0);
        }

        for (int j = 2; j < 5; j++)
        {
            Vector3 saveCheckPointPos2 = new Vector3(checkPointPosX[j], 5f, checkPointPosZ[j]);

            onMapWarpTf[j].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, j - 2);
            SettingOnMapCheckPoint(j, saveCheckPointPos2, 1);
        }

        for (int k = 5; k < 12; k++)
        {
            Vector3 saveCheckPointPos3 = new Vector3(checkPointPosX[k], 5f, checkPointPosZ[k]);

            onMapWarpTf[k].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, k - 5);
            SettingOnMapCheckPoint(k, saveCheckPointPos3, 2);
        }

        for (int n = 12; n < 16; n++)
        {
            Vector3 saveCheckPointPos4 = new Vector3(checkPointPosX[n], 5f, checkPointPosZ[n]);

            onMapWarpTf[n].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, n - 12);
            SettingOnMapCheckPoint(n, saveCheckPointPos4, 3);
        }

        //for (int i = 0; i < 16; i++)
        //{
        //    Vector3 saveCheckPointPos = new Vector3(checkPointPosX[i], 5f, checkPointPosZ[i]);

        //    // 봄 지도의 체크 포인트 순서
        //    if (i < 2)
        //    {
        //        // 봄 지도의 체크 포인트들의 정보를 저장하고, 맵 상의 체크 포인트 위치값을 계산하여 배치하는 함수를 실행
        //        for (int q = 0; q < 2; q++)
        //        {
        //            onMapWarpTf[i].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, q);
        //            SettingOnMapCheckPoint(i, saveCheckPointPos, 0);

        //            //checkPointDic.Add(i + 1, saveCheckPointPos);
        //        }
        //    }
        //    // 여름 지도의 체크 포인트 순서
        //    else if (i >= 2 && i < 5)
        //    {
        //        // 여름 지도의 체크 포인트들의 정보를 저장하고, 맵 상의 체크 포인트 위치값을 계산하여 배치하는 함수를 실행
        //        for (int w = 0; w < 3; w++)
        //        {
        //            onMapWarpTf[i].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, w);
        //            SettingOnMapCheckPoint(i, saveCheckPointPos, 1);
        //        }
        //    }
        //    // 가을 지도의 체크 포인트 순서
        //    else if (i >= 5 && i < 12)
        //    {
        //        // 가을 지도의 체크 포인트들의 정보를 저장하고, 맵 상의 체크 포인트 위치값을 계산하여 배치하는 함수를 실행
        //        for (int e = 0; e < 7; e++)
        //        {
        //            onMapWarpTf[i].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, e);
        //            SettingOnMapCheckPoint(i, saveCheckPointPos, 2);
        //        }
        //    }
        //    else if (i >= 12)
        //    {
        //        // 가을 지도의 체크 포인트들의 정보를 저장하고, 맵 상의 체크 포인트 위치값을 계산하여 배치하는 함수를 실행
        //        for (int r = 0; r < 4; r++)
        //        {
        //            onMapWarpTf[i].GetComponent<MapMarkInfo>().StartInfoSetting("활성화된 체크 포인트", true, r);
        //            SettingOnMapCheckPoint(i, saveCheckPointPos, 3);
        //        }
        //    }
        //}

        //moveMapCheck = 1;
        //mapCameraTf.GetComponent<CameraControl>().CheckMapType(moveMapCheck);

        //AccountMapSize();
    }     // Start()

    // 게임 시작 시 맵 상의 체크포인트 위치값을 계산하여 배치하는 함수
    private void SettingOnMapCheckPoint(int count, Vector3 checkPointPos, int mapType)
    {
        float[] countPos = new float[2];

        switch (mapType)
        {
            case 0:
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 X 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.x, 0, 0, out countPos[0]);
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 Z 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.z, 1, 0, out countPos[1]);

                // 지도상의 체크포인트 위치를 실제 맵의 체크포인트 위치와 동기화
                onMapWarpTf[count].position = new Vector3(countPos[0], 50f, countPos[1]);
                break;
            case 1:
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 X 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.x, 0, 1, out countPos[0]);
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 Z 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.z, 1, 1, out countPos[1]);

                // 지도상의 체크포인트 위치를 실제 맵의 체크포인트 위치와 동기화
                onMapWarpTf[count].position = new Vector3(countPos[0], 50f, countPos[1]);
                break;
            case 2:
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 X 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.x, 0, 2, out countPos[0]);
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 Z 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.z, 1, 2, out countPos[1]);

                // 지도상의 체크포인트 위치를 실제 맵의 체크포인트 위치와 동기화
                onMapWarpTf[count].position = new Vector3(countPos[0], 50f, countPos[1]);
                break;
            case 3:
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 X 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.x, 0, 3, out countPos[0]);
                // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 Z 축 위치값을 계산하여 가져옴
                CountDistanceMap(checkPointPos.z, 1, 3, out countPos[1]);

                // 지도상의 체크포인트 위치를 실제 맵의 체크포인트 위치와 동기화
                onMapWarpTf[count].position = new Vector3(countPos[0], 50f, countPos[1]);
                break;
            default:
                break;
        }
    }     // SettingOnMapCheckPoint()

    // 지도를 열고 UI 를 활성화 하는 함수
    public void OpenMap()
    {
        ResetCamera();
        allMapsObj.SetActive(true);
        mapManualObj.SetActive(true);
    }     // OpenMap()

    // 지도를 닫고 UI 를 비활성화 하는 함수
    public void ExitMap()
    {
        mapCameraTf.GetComponent<CameraControl>().EndMapCamera();
        mapManualObj.SetActive(false);
        allMapsObj.SetActive(false);
    }     // ExitMap()

    // 지도를 열 때 지도상의 카메라의 위치를 초기화 시키는 함수
    private void ResetCamera()
    {
        CheckMapType();

        if (moveMapCheck < 4)
        {
            // 지도상의 플레이어 표식 위치를 실제 플레이어 위치에 기반해 계산하여 위치를 변경하는 함수를 실행
            OnMapPlayerSetting();

            // 지도 카메라의 위치를 지도상의 플레이어 위치로 이동
            mapCameraTf.position = new Vector3(onMapPlayerTf.position.x, mapCameraTf.position.y, onMapPlayerTf.position.z);
            //mapCameraTf.position = new Vector3(2000f, mapCameraTf.position.y, 1000f);

            mapCameraTf.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (moveMapCheck == 4)
        {
            ExitMap();
            mainObjTf.GetComponent<MainMenu>().DisconnectMenu();
            mainObjTf.GetComponent<UIController>().uiController = 1;
        }
    }     // ResetCamera()

    private void CheckMapType()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Spring"))
        {
            moveMapCheck = 0;
        }
        else if (sceneName.Contains("Summer"))
        {
            moveMapCheck = 1;
        }
        else if (sceneName.Contains("Autumn"))
        {
            moveMapCheck = 2;
        }
        else if (sceneName.Contains("Winter"))
        {
            moveMapCheck = 3;
        }
        else
        {
            moveMapCheck = 4;
        }

        mapCameraTf.GetComponent<CameraControl>().CheckMapType(moveMapCheck);
    }     // CheckMapType()

    // 지도를 활성화할 때 지도상의 플레이어 표식 위치를 실제 플레이어 위치에 기반해 계산하여 위치를 변경하는 함수
    private void OnMapPlayerSetting()
    {
        float[] countPosition = new float[2];

        // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 X 축 위치값을 계산하여 가져옴
        CountDistanceMap(playerTf.position.x, 0, moveMapCheck, out countPosition[0]);
        // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수에 Z 축 위치값을 계산하여 가져옴
        CountDistanceMap(playerTf.position.z, 1, moveMapCheck, out countPosition[1]);

        // 지도상의 플레이어 위치를 실제 맵의 플레이어 위치와 동기화
        onMapPlayerTf.position = new Vector3(countPosition[0], 50f, countPosition[1]);
    }     // OnMapPlayerSetting()

    // 실제 지형과 지도상의 배율, 거리값을 계산하여 실제 위치값을 계산하여 값을 내보내는 함수
    private float CountDistanceMap(float pos, int dir, int mapType, out float countPos)
    {
        float count = 0f;

        // 봄 지도 축적 계산
        if (mapType == 0)
        {
            // X 축, Z 축 계산 구분
            switch (dir)
            {
                // X 축
                case 0:
                    // 실제 맵상에 X 축 위치값이 양수 일 때
                    if (pos >= 0)
                    {
                        count = (pos * multipleMapSize_spring[dir]) - distanceMapSize_spring[dir];
                    }
                    // 실제 맵상에 X 축 위치값이 음수 일 때
                    else
                    {
                        count = (pos * multipleMapSize_spring[dir]) + distanceMapSize_spring[dir];
                    }
                    break;
                // Z 축
                case 1:
                    // 실제 맵상에 Z 축 위치값이 양수 일 때
                    if (pos >= 0)
                    {
                        count = (pos * multipleMapSize_spring[dir]) + distanceMapSize_spring[dir];
                    }
                    // 실제 맵상에 Z 축 위치값이 음수 일 때
                    else
                    {
                        count = (pos * multipleMapSize_spring[dir]) - distanceMapSize_spring[dir];
                    }
                    break;
                default:
                    count = 0f;
                    break;
            }

            // 계산된 결과에 실제 맵과 지도맵의 거리 차이값을 더해줌
            countPos = dragMap_spring[dir] + count;
        }
        // 여름 지도 축적 계산
        else if (mapType == 1)
        {
            // X 축, Z 축 계산 구분
            switch (dir)
            {
                // X 축
                case 0:
                    // 실제 맵상에 X 축 위치값이 양수 일 때
                    if (pos >= 0)
                    {
                        count = (pos * multipleMapSize_summer[dir]) + distanceMapSize_summer[dir];
                    }
                    // 실제 맵상에 X 축 위치값이 음수 일 때
                    else
                    {
                        count = (pos * multipleMapSize_summer[dir]) - distanceMapSize_summer[dir];
                    }
                    break;
                // Z 축
                case 1:
                    // 실제 맵상에 Z 축 위치값이 양수 일 때
                    if (pos >= 0)
                    {
                        count = (pos * multipleMapSize_summer[dir]) - distanceMapSize_summer[dir];
                    }
                    // 실제 맵상에 Z 축 위치값이 음수 일 때
                    else
                    {
                        count = (pos * multipleMapSize_summer[dir]) + distanceMapSize_summer[dir];
                    }
                    break;
                default:
                    count = 0f;
                    break;
            }

            // 계산된 결과에 실제 맵과 지도맵의 거리 차이값을 더해줌
            countPos = dragMap_summer[dir] + count;
        }
        else if (mapType == 2)
        {
            float disPos = pos - pivotAutumnSize[dir];
            // X 축, Z 축 계산 구분
            switch (dir)
            {
                // X 축
                case 0:
                    // 실제 맵상에 X 축 위치값이 양수 일 때
                    if (disPos >= 0)
                    {
                        count = (disPos * multipleMapSize_autumn[dir]) - distanceMapSize_autumn[dir];
                    }
                    // 실제 맵상에 X 축 위치값이 음수 일 때
                    else
                    {
                        count = (disPos * multipleMapSize_autumn[dir]) + distanceMapSize_autumn[dir];
                    }
                    break;
                // Z 축
                case 1:
                    // 실제 맵상에 Z 축 위치값이 양수 일 때
                    if (pos >= 0)
                    {
                        count = (disPos * multipleMapSize_autumn[dir]) + distanceMapSize_autumn[dir];
                    }
                    // 실제 맵상에 Z 축 위치값이 음수 일 때
                    else
                    {
                        count = (disPos * multipleMapSize_autumn[dir]) + distanceMapSize_autumn[dir];
                    }
                    break;
                default:
                    count = 0f;
                    break;
            }

            // 계산된 결과에 실제 맵과 지도맵의 거리 차이값을 더해줌
            countPos = dragMap_autumn[dir] + count;
        }
        else if (mapType == 3)
        {
            float disPos = pos - pivotWinterSize[dir];
            // X 축, Z 축 계산 구분
            switch (dir)
            {
                // X 축
                case 0:
                    // 실제 맵상에 X 축 위치값이 양수 일 때
                    if (disPos >= 0)
                    {
                        count = (disPos * multipleMapSize_winter[dir]) + distanceMapSize_winter[dir];
                    }
                    // 실제 맵상에 X 축 위치값이 음수 일 때
                    else
                    {
                        count = (disPos * multipleMapSize_winter[dir]) + distanceMapSize_winter[dir];
                    }
                    break;
                // Z 축
                case 1:
                    // 실제 맵상에 Z 축 위치값이 양수 일 때
                    if (pos >= 0)
                    {
                        count = (disPos * multipleMapSize_winter[dir]) + distanceMapSize_winter[dir];
                    }
                    // 실제 맵상에 Z 축 위치값이 음수 일 때
                    else
                    {
                        count = (disPos * multipleMapSize_winter[dir]) + distanceMapSize_winter[dir];
                    }
                    break;
                default:
                    count = 0f;
                    break;
            }

            // 계산된 결과에 실제 맵과 지도맵의 거리 차이값을 더해줌
            countPos = dragMap_winter[dir] + count;
        }
        else
        {
            countPos = 0;
        }
        
        return countPos;
    }     // CountDistanceMap()

    //// 워프 지점 순서를 체크하여 해당 워프 지점으로 플레이어가 워프하는 기능의 함수
    //public Vector3 ConnectCheckPoint(int checkPoint, out Vector3 checkPointPosition)
    //{
    //    if (checkPoint == 0)
    //    {
    //        checkPointPosition = Vector3.zero;
    //    }
    //    //else if (checkPointDic.ContainsKey(checkPoint))
    //    //{
    //    //    checkPointPosition = checkPointDic[checkPoint];
    //    //}
    //    else
    //    {
    //        checkPointPosition = Vector3.zero;
    //    }

    //    if (checkPointPosition != Vector3.zero)
    //    {
    //        playerTf.position = checkPointPosition;

    //        mainObjTf.GetComponent<UIController>().AfterWarpExitMap();
    //        //screenControllerTf.GetComponent<ScreenController>().ScreenEffect(0, 0.02f, 0.02f, 1f, 1);

    //        Debug.Log("체크 포인트 워프 성공!!");
    //    }
    //    else
    //    {
    //        Debug.Log("체크 포인트 워프 Error!!!");
    //    }

    //    return checkPointPosition;
    //}     // ConnectCheckPoint()

    //// 게임 시작 시 실제 플레이 맵과 지도 맵의 차이를 계산하는 함수
    //private void AccountMapSize()
    //{
    //    dragMap[0] = (mapSizeCheck[1].position.x - mapSizeCheck[0].position.x) + 20f;
    //    dragMap[1] = (mapSizeCheck[1].position.z - mapSizeCheck[0].position.z) - 110f;

    //    Debug.LogFormat("맵의 거리 차이 : {0}, {1}", dragMap[0], dragMap[1]);
    //}     // AccountMapSize()
}
