using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    IEnumerator TestCor;

    private bool coroutineCheck = false;

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        if (coroutineCheck == false)
    //        {
    //            StartCoroutine();
    //        }
    //        else
    //        {
    //            StopCoroutine();
    //        }
    //    }
    //}

    private void StartCoroutine()
    {
        coroutineCheck = true;
        TestCor = FunctionCoroutine();
        Debug.Log("코루틴 시작");
        
        StartCoroutine(TestCor);
    }

    private void StopCoroutine()
    {
        if (TestCor != null)
        {
            coroutineCheck = false;
            Debug.Log("코루틴 중지");

            StopCoroutine(TestCor);
        }
    }

    IEnumerator FunctionCoroutine()
    {
        yield return new WaitForSeconds(5f);

        coroutineCheck = false;
        Debug.Log("코루틴 정상 종료됨");
    }
}
