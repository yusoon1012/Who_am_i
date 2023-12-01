using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTeleportCanvas : MonoBehaviour
{
    [System.Serializable]
    public class SelectImage
    {
        public GameObject beach;
        public GameObject forest;
        public GameObject temple;
        public GameObject winter;
        public GameObject fall;
    }

    private void Start()
    {
        Setting();
    }

    private void Setting()
    {
        
    }
}
