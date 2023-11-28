using Oculus.Interaction.Samples;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveTest : MonoBehaviour
{
    public Transform[] point = new Transform[3];
    public Transform playerTf;

    private Transform npcTf;
    private Rigidbody npcRb;

    private bool moveNPC = false;
    private int pointCheck = default;

    void Awake()
    {
        npcTf = GetComponent<Transform>().transform;
        npcRb = GetComponent<Rigidbody>();

        pointCheck = 0;
    }

    void Update()
    {
        if (moveNPC == true)
        {
            MoveNPC();
        }
        else if (moveNPC == false)
        {
            StopNPC();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player" && moveNPC == true)
        {
            moveNPC = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && moveNPC == false)
        {
            moveNPC = true;
        }
        else if (collision.transform == point[0] && pointCheck == 0)
        {
            pointCheck = 1;
        }
        else if (collision.transform == point[1] && pointCheck == 1)
        {
            pointCheck = 2;
        }
        else if (collision.transform == point[2] && pointCheck == 2)
        {
            pointCheck = 3;
            StartCoroutine(OffNPC());
        }
    }

    private void MoveNPC()
    {
        if (pointCheck < 3)
        {
            npcTf.transform.LookAt(point[pointCheck]);
            transform.position = Vector3.Lerp(transform.position, point[pointCheck].position, 0.0003f);
        }
    }

    private void StopNPC()
    {
        
        npcTf.transform.LookAt(playerTf);
    }

    IEnumerator OffNPC()
    {
        yield return new WaitForSeconds(3f);

        npcTf.gameObject.SetActive(false);
    }
}
