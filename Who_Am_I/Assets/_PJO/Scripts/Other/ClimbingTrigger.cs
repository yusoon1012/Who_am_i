using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.name == QuestManager_Jun.instance.currentQuest.ToString())
            {
                QuestManager_Jun.instance.CheckClear("Climbing");
            }
        }
    }
}
