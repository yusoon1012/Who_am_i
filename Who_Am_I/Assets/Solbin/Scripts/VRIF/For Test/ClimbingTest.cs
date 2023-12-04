using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingTest : MonoBehaviour
{

    [SerializeField] private Transform player = default;
    private Rigidbody playerRigid = default;
    private VRIFAction vrifAction = default;

    private void Start()
    {
        playerRigid = player.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        vrifAction = new VRIFAction();
        vrifAction.Enable();
    }

    private void OnDisable()
    {
        vrifAction?.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Climbable>())
        {
            playerRigid.velocity = Vector3.zero;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.transform.GetComponent<Climbable>())
    //    {
    //        Debug.LogWarning("Attach Climbable");

    //        if (Input.GetKeyDown(KeyCode.O))
    //        {
    //            Debug.LogWarning(other.name);
    //        }
    //        // TODO: 왜 Grabber와 Climbable과 충돌하지 않았는데 충돌했다는 Debug가 출력되는가...?
    //    }
    //}
}
