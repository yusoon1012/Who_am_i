using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    private Vector3 moveVector = Vector3.zero;

    private Rigidbody playerRb = default;

    private float speed = default;
    private float xInput = default;
    private float zInput = default;
    private float xSpeed = default;
    private float zSpeed = default;

    void Awake()
    {
        speed = 10f;
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        if (xInput != 0 || zInput != 0)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        xSpeed = xInput * speed;
        zSpeed = zInput * speed;

        moveVector = new Vector3(xSpeed, 0f, zSpeed);

        playerRb.velocity = moveVector;
    }
}
