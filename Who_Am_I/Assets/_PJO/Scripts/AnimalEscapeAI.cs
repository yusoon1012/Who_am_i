using System;
using System.Collections;
using UnityEngine;

public class AnimalEscapeAI : MonoBehaviour
{
    [SerializeField]
    [Header("포인트 좌표들이 있는 부모 오브젝트")]
    private GameObject pointParent = default;

    private ThisData thisData = default;

    private AnimalData data = default;
    private Coroutine unescape = default;

    private LayerMask pointLay = default;
    private LayerMask mapLay = default;
    private LayerMask obstacleLay = default;

    private bool isSearch = false;

    private Vector3[] points = default;

    public bool isPlayer = default;

    // 10를 포함하기 위한 +1
    private int MAX_WAIT_TIME = 11;
    private int MIN_WAIT_TIME = 5;

    private int VERTEX_RANGE = 50;

    private void Start()
    {
        // { layerMask 설정
        pointLay = LayerMask.GetMask("Point");
        mapLay = LayerMask.GetMask("Map");
        obstacleLay = LayerMask.GetMask("obstacle");
        // } layerMask 설정

        if (pointLay == 0)
        {
            Debug.LogError("point 레이어를 가져오지 못했습니다.");
            return;
        }
        // } layerMask 설정

        points = GFunc.GetChildArray(pointParent);
    }       // Start()


    private void Update()
    {
        if (isPlayer == true)
        {
            if (StartCoroutine(Unescape()) != null)
            {
                StopCoroutine(StartCoroutine(Unescape()));
                isSearch = false;
            }
            if (isSearch == false)
            {

            }
            //else if 
        }
        else
        {
            if (isSearch == false)
            {
                StartCoroutine(Unescape());
            }
        }
    }       // Update()

    //! 랜덤 위치 이동
    private IEnumerator Unescape()
    {
        isSearch = true;

        bool isAction = GFunc.RandomBool();

        if (isAction == true)
        {
            Vector3 currentPosition = transform.position;
            Vector3 setPosition = RandomPosition(currentPosition);

            float timeElapsed = 0.0f;
            float distance = Vector3.Distance(currentPosition, setPosition);
            float duration = distance / data.speed;

            transform.LookAt(setPosition);

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                float time = Mathf.Clamp01(timeElapsed / duration);

                transform.position = Vector3.Lerp(currentPosition, setPosition, time);

                yield return null;
            }
        }
        else
        {
            float waitTime = UnityEngine.Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME);

            yield return new WaitForSeconds(waitTime);
        }

        isSearch = false;
    }

    private Vector3 RandomPosition(Vector3 _currentPosition)
    {
        Vector3 colliderPosition_;
        Collider[] colliders_ = Physics.OverlapSphere(_currentPosition, VERTEX_RANGE, pointLay);

        int randomIndex_;

        do
        {
            randomIndex_ = GFunc.RandomValueInt(0, colliders_.Length);
            colliderPosition_ = colliders_[randomIndex_].transform.position;
            break;
        } while (colliderPosition_ != _currentPosition);

        Vector3 offset_ = new Vector3(_currentPosition.x - colliderPosition_.x,
            0.0f, _currentPosition.z - colliderPosition_.z);


        Vector3 setPosition_ = Vector3.Lerp(
            colliderPosition_ + new Vector3(VERTEX_RANGE * GFunc.SignIndicator(offset_.x), 0.0f, 0.0f),
            colliderPosition_ + new Vector3(0.0f, 0.0f, VERTEX_RANGE * GFunc.SignIndicator(offset_.z)),
            GFunc.RandomValueInt(1, 10) * 0.1f);

        Ray ray = new Ray(setPosition_, Vector3.up);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mapLay))
        {
            setPosition_ = hit.transform.position;
        }

        ray = new Ray(_currentPosition, setPosition_ - _currentPosition);

        if (Physics.Raycast(ray, Vector3.Distance(_currentPosition, setPosition_), obstacleLay))
        {
            Debug.Log("가는길에 오브젝트가 막고있습니다.");

            for (int i = 0; i < colliders_.Length; i++)
            {

            }

            return _currentPosition;
        }

        return setPosition_;
    }
}
