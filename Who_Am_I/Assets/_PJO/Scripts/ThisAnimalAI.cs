using BNG;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ThisAnimalAI : MonoBehaviour
{
    [SerializeField]
    [Header("이동 가능한 구간")]
    private GameObject map = default;

    private ThisAnimalData thisData = default;
    private NavMeshAgent navigation = default;
    private Animator animator = default;

    private Coroutine moveCoroutine = default;
    private Coroutine waitCoroutine = default;

    private Vector3[] mapPoints = default;

    private int MAX_WAIT_TIME = 10;
    private int MIN_WAIT_TIME = 2;
    private int CLIP_COUNT = 3;

    private void Start()
    {
        // { 애니메이션 컴포넌트
        animator = GFunc.SetComponent<Animator>(this.gameObject);
        if (animator == null)
        {
            GFunc.SubmitNonFindText<Animator>(this.gameObject);
            return;
        }
        // } 애니메이션 컴포넌트
        // { 데이터 컴포넌트
        thisData = GFunc.SetComponent<ThisAnimalData>(this.gameObject);
        if (thisData == null)
        {
            GFunc.SubmitNonFindText<ThisAnimalData>(this.gameObject);
            return;
        }
        // } 데이터 컴포넌트
        // { 네비게이션 컴포넌트
        navigation = GFunc.SetComponent<NavMeshAgent>(this.gameObject);
        if (navigation == null)
        {
            GFunc.SubmitNonFindText<NavMeshAgent>(this.gameObject);
            return;
        }
        // } 네비게이션 컴포넌트
        mapPoints = GFunc.GetMeshVertiesArray(this.map);

        // Test
        for (int i = 0; i < mapPoints.Length; i++)
        {
            mapPoints[i] = mapPoints[i] * 10;
        }
    }       // Start()

    public void Escape(Vector3 _playerPosition)
    {
        thisData.isAction = true;

        Vector3 setPosition_ = default;
        float setDistance_ = 0;

        for (int i = 0; i < mapPoints.Length; i++)
        {
            float distance_ = Vector3.Distance(transform.position, _playerPosition);

            if (setDistance_ < distance_)
            {
                setDistance_ = distance_;
                setPosition_ = mapPoints[i];
            }
        }

        moveCoroutine = StartCoroutine(Move(setPosition_));
    }

    public void UnEscape()
    {
        thisData.isAction = true;

        bool isAction = GFunc.RandomBool();

        if (isAction == true)
        {
            //moveCoroutine = StartCoroutine(Move(mapPoints[GFunc.RandomValueInt(0, mapPoints.Length)]));
        }
        else
        {
            waitCoroutine = StartCoroutine(Wait());
        }
    }

    public void SetCoroutine()
    {
        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); }
        if (waitCoroutine != null) { StopCoroutine(waitCoroutine); }
    }

    private IEnumerator Move(Vector3 _setPosition)
    {
        navigation.SetDestination(_setPosition);
        animator.SetBool("Walk", true);
        while (transform.position.x != _setPosition.x && transform.position.z != _setPosition.z)
        {
            yield return null;
        }
        animator.SetBool("Walk", false);
        thisData.isAction = false;
    }

    private IEnumerator Wait()
    {
        float waitTime = GFunc.RandomValueInt(MIN_WAIT_TIME, MAX_WAIT_TIME);
        yield return new WaitForSeconds(waitTime);

        animator.SetInteger("Idle", GFunc.RandomValueInt(0, CLIP_COUNT));
        animator.SetTrigger("IdleAction");

        waitTime = GFunc.RandomValueInt(MIN_WAIT_TIME, MAX_WAIT_TIME);
        yield return new WaitForSeconds(waitTime);

        thisData.isAction = false;
    }
}
