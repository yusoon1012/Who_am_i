using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// An example Grabbable that lets you move along a designated path
    /// </summary>
    public class Zipline : GrabbableEvents {

        public Transform ZiplineStart;
        public Transform ZiplineEnd;
        public float ZiplineSpeed = 1;
        public bool UseLinearMovement = true;

        float lastMoveTime = -1f;
        bool movingForward = true;
        AudioSource audioSource;

        void Start() {
            // Start off by orienting the zipline holder
            if(ZiplineEnd != null) {
                transform.LookAt(ZiplineEnd.position);
            }

            audioSource = GetComponent<AudioSource>();
        }

        void Update() {           

            // Play vs. stop sound
            if (Time.time - lastMoveTime < 0.1f) {

                // Raise the pitch a little bit if we are moving forwards
                if(movingForward) {
                    audioSource.pitch = Time.timeScale * 1f;
                }
                else {
                    audioSource.pitch = Time.timeScale * 0.95f;
                }

                // Make sure the clip is playing
                if(!audioSource.isPlaying) {
                    audioSource.Play();
                }
            }
            else if(audioSource.isPlaying) {
                audioSource.Stop();
            }
        }

        void OnDrawGizmosSelected() {
            if (ZiplineStart != null && ZiplineEnd != null) {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.green;
                Gizmos.DrawLine(ZiplineStart.position, ZiplineEnd.position);
            }
        }

        // <Solbin> 손잡이를 잡은 것을 인식한다. 
        public override void OnTrigger(float triggerValue) {

            if (triggerValue > 0.5f) {
                moveTowards(ZiplineEnd.position, true);
            }

            base.OnTrigger(triggerValue);
        }

        public override void OnButton1() {

            // <Solbin> A 버튼
            moveTowards(ZiplineStart.position, false);

            base.OnButton1();
        }
        public override void OnButton2() {

            // <Solbin> B 버튼
            moveTowards(ZiplineEnd.position, true);

            base.OnButton2();
        }

        // <Solbin>
        /// <summary>
        /// 가속력 구현을 위한 메소드 
        /// </summary>
        private void AccelerationForce()
        {
            // TODO: 가속력 구현 
        }
        // <Solbin> ===

        // <Solbin> 앞으로 진행을 위해 실행되는 메소드 
        void moveTowards(Vector3 pos, bool forwardDirection) {

            lastMoveTime = Time.time;
            movingForward = forwardDirection;

            // Orient Zipline
            if (forwardDirection) {
                transform.LookAt(pos);
            }
            else {
                // If backward, look at object from rear
                transform.LookAt(2 * transform.position - pos);
            }

            // Linear Movement
            if (UseLinearMovement) {
                transform.position = Vector3.MoveTowards(transform.position, pos, ZiplineSpeed * Time.fixedDeltaTime);
            }
            // Lerp
            else {
                transform.position = Vector3.Lerp(transform.position, pos, ZiplineSpeed * Time.deltaTime);
            }

            // Haptics
            if(input  && thisGrabber) {
                input.VibrateController(0.1f, 0.1f, 0.1f, thisGrabber.HandSide);
            }
        }
    }
}

