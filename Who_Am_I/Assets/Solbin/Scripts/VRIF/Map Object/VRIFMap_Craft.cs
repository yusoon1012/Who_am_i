using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFMap_Craft : MonoBehaviour
{
    private int playerLayer = default;

    [Header("FX Fire")]
    [SerializeField] private ParticleSystem fxFire = default;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            fxFire.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            fxFire.Stop();
            fxFire.Clear();
        }
    }
}
