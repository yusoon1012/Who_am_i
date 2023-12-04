using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFTeleportHall : MonoBehaviour
{
    [SerializeField] GameObject playerTeleportRange = default;
    [SerializeField] GameObject teleportUI = default;
    [SerializeField] VRIFStateSystem vrifStateSystem = default;

    private VRIFAction vrifAction = default;

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
        if (other.gameObject == playerTeleportRange) 
        {
            if (vrifAction.Player.Interaction.triggered)
            {
                teleportUI.SetActive(true);
                vrifStateSystem.ChangeState(VRIFStateSystem.GameState.UI);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerTeleportRange)
        {
            if (vrifAction.Player.Interaction.triggered)
            {
                teleportUI.SetActive(false);
                vrifStateSystem.ChangeState(VRIFStateSystem.GameState.NORMAL);
            }
        }
    }
}
