using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : OVRGrabber
{
    private void FixedUpdate()
    {
        if (m_grabbedObj != null)
        {
            Debug.LogFormat("{0}를 잡음!", m_grabbedObj.name);
        }
        else { Debug.Log("물체를 잡지 못함"); }
    }
}
