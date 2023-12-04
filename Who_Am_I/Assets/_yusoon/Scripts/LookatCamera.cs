using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatCamera : MonoBehaviour
{
    Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the vector from this object to the camera
        Vector3 lookDir = transform.position - m_Camera.transform.position;
        lookDir.y = 0;

        // Calculate the rotation to look away from the camera
        Quaternion lookRotation = Quaternion.LookRotation(lookDir);

        // Apply the rotation to the object
        transform.rotation = lookRotation;
    }
}
