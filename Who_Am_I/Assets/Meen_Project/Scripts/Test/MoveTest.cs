using Oculus.Interaction.Samples;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveTest : MonoBehaviour
{
    public Transform point;

    private Transform npcTf;

    private bool moveNPC = false;

    void Awake()
    {
        npcTf = GetComponent<Transform>().transform;
    }

    void Start()
    {
        StartCoroutine(MoveStart());
    }

    void Update()
    {
        if (moveNPC == true)
        {
            npcTf.transform.LookAt(point);
            transform.position = Vector3.Lerp(transform.position, point.position, 0.0001f);
        }
    }

    IEnumerator MoveStart()
    {
        yield return new WaitForSeconds(3f);

        MoveNPC();
    }

    private void MoveNPC()
    {
        Debug.Log("NPC 출발");

        moveNPC = true;
    }
}
