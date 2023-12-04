using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace BNG
{
    public class VRIFControllerSystem : MonoBehaviour
    {
        // VRIF Action
        VRIFAction vrifAction;
       
        // Left Controller Velocity
        public static Vector3 lVelocity { get; private set; }
        // Right Controller Velocity
        public static Vector3 rVelocity { get; private set; }

        private void OnEnable()
        {
            vrifAction = new VRIFAction();
            vrifAction.Enable();
        }

        private void OnDisable() 
        { 
            vrifAction.Disable();
        }

        private void Update()
        {
            DeviceVelocity();
        }

        /// <summary>
        /// 양쪽 컨트롤러의 Velocity
        /// </summary>
        private void DeviceVelocity()
        {
            lVelocity = vrifAction.Player.LeftVelocity.ReadValue<Vector3>();
            rVelocity = vrifAction.Player.RightVelocity.ReadValue<Vector3>();
        }
    }
}

