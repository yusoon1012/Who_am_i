using System;
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
        public bool UseLinearMovement = true;

        [Header("짚라인 출발 속도")]
        [SerializeField] private float ZiplineSpeed = 1;

        [Header("짚라인 최대 속도")]
        [SerializeField] private float ZipLineMaxSpeed = default;

        [Header("짚라인 위치 복귀 타이머")]
        [SerializeField] private int ZipLineTimer = 10;

        float lastMoveTime = -1f;
        bool movingForward = true;
        AudioSource audioSource;

        // <Solbin> 짚라인 이동 bool값  
        private bool move = false;
        // <Solbin> 짚라인 원래 위치
        private Vector3 originPos = default;
        // <Solbin> 짚라인 원래 출발속도 
        private float originZiplineSpeed = default;

        // <Solbin> 해당 물체가 Grabber에 닿아있는지 판단
        private bool inTrigger = false;

        void Start() {
            // Start off by orienting the zipline holder
            if(ZiplineEnd != null) {
                transform.LookAt(ZiplineEnd.position);
            }

            audioSource = GetComponent<AudioSource>();

            // <Solbin>
            originPos = transform.position;
            originZiplineSpeed = ZiplineSpeed;
            // <Solbin> ===
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

            if (VRIFStateSystem.Instance.gameState == VRIFStateSystem.GameState.ZIPLINE) // 짚라인을 잡았을 때
            {
                ShakingController();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Grabber>()) { inTrigger = true; }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Grabber>()) { inTrigger = false; }
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
        public override void OnButton2()
        {
            // <Solbin> B 버튼 => 원 에셋은 해당 버튼을 누르면 앞으로 나아간다.
            moveTowards(ZiplineEnd.position, true);

            base.OnButton2();
        }

        // <Solbin>
        /// <summary>
        /// 컨트롤러를 흔들면 앞으로 나아가도록 한다. (아래로) 
        /// </summary>
        private void ShakingController()
        {
            if (inTrigger)
            {
                // 컨트롤러를 아래로 흔들면 (가속력 구분)
                if (VRIFInputSystem.Instance.lVelocity.y <= -0.5f || VRIFInputSystem.Instance.rVelocity.y <= -0.5f)
                {
                    move = true;

                    StartCoroutine(AccelerationForce()); // 가속력 코루틴
                }
                else if (VRIFInputSystem.Instance.lVelocity.y <= -0.75f || VRIFInputSystem.Instance.rVelocity.y <= -0.75f)
                {
                    move = true;
                    ZiplineSpeed += 3;

                    StartCoroutine(AccelerationForce()); // 가속력 코루틴
                }

                if (move) // 이동 bool값일때
                {
                    moveTowards(ZiplineEnd.position, true);

                    if (Vector3.Distance(transform.position, ZiplineEnd.position) <= 0.1f ||
                        VRIFStateSystem.Instance.gameState != VRIFStateSystem.GameState.ZIPLINE) // 끝에 도달했거나 짚라인 상태가 아니면 
                    {
                        move = false;
                        StartCoroutine(ResetPosition());
                    }
                }
            }
        }

        /// <summary>
        /// 짚라인을 재위치로 이동시키는 메소드 
        /// </summary>
        private IEnumerator ResetPosition()
        {
            yield return new WaitForSeconds(ZipLineTimer); // 대기 후 복귀

            transform.position = originPos; // 원 위치로 이동 
        }

        /// <summary>
        /// 가속력 구현을 위한 코루틴
        /// </summary>
        private IEnumerator AccelerationForce()
        {
            while(move) // 짚라인이 움직이는 동안
            {
                yield return new WaitForSeconds(1.5f);

                ZiplineSpeed += 1;

                if (!move) { break; }
            }

            ZiplineSpeed = originZiplineSpeed;
        }
        // <Solbin> ===

        /// <summary>
        /// 전진 메소드
        /// </summary>
        /// <param name="pos">ZiplineEnd.position</param>
        /// <param name="forwardDirection">전진 여부</param>
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

