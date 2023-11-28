using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTest : MonoBehaviour
{
    public Transform npcTf;

    private bool npcOn = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (npcOn == false && collision.tag == "Player")
        {
            npcOn = true;
            npcTf.gameObject.SetActive(true);
        }
    }
}
