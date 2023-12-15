using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    public Camera renderCamera;
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if(canvas!=null&&renderCamera!=null)
        {
            canvas.worldCamera = renderCamera;
        }
        else
        {

        }
    }
}
