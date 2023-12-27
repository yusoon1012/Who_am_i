using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnQuest : MonoBehaviour
{
    private GameObject thisNpc = default;

    void Start()
    {
        thisNpc = GetComponent<GameObject>().gameObject;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Meen_QuestManager.instance.onNpcCheck = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Meen_QuestManager.instance.onNpcCheck = null;
        }
    }
}
