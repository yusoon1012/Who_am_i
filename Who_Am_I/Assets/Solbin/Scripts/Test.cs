using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : OVRGrabber
{
    new private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            m_moveHandPosition = false;
        }
    }
}
