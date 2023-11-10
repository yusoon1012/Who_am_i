using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 손의 속도를 구한다. 
/// </summary>
public class Hand_Acceleration : MonoBehaviour
{
    private Vector3 previousPos = default;
    private Vector3 currentPos = default;
    private double velocity = default;

    private void Start() { previousPos = transform.position; } // 초기화

    private void CheckVelocity()
    {
        currentPos = transform.position;

        var distance = Vector3.Distance(previousPos, currentPos);
        velocity = distance / Time.deltaTime;
        previousPos = currentPos; 
    }

    private void Update() 
    {
        CheckVelocity();

        Debug.LogFormat("손의 속력: {0}", velocity);
    }
}
