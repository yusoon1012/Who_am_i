using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public GameObject allMapsObj;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            allMapsObj.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            allMapsObj.SetActive(false);
        }
    }
}
