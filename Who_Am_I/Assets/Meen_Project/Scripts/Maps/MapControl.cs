using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public GameObject allMapsObj;
    public Transform playerTf;
    public Transform mapCameraTf;

    public int moveCheck = default;

    void Awake()
    {
        moveCheck = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            moveCheck = 1;
            ResetCamera();
            allMapsObj.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            moveCheck = 0;
            allMapsObj.SetActive(false);
        }
    }

    private void ResetCamera()
    {
        Vector3 playerPosition = playerTf.position;
        mapCameraTf.position = new Vector3(playerPosition.x, mapCameraTf.position.y, playerPosition.z);
    }
}
