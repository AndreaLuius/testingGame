using UnityEngine;
using Cinemachine;
using System.Collections;

namespace TargetSystem
{
    public class InPlaceController : MonoBehaviour
    {
        private Animator animator;
        private CinemachineTargetGroup targetGroup;
        [SerializeField] float maxRightAngle = -135;
        [SerializeField] float maxLeftAngle = 65f;
        [SerializeField] float rightCompensator = 10f;
        [SerializeField] float leftCompensator = -30;
        private float delayTime = .5f;
        private float angle = 0;
        private Vector3 direction;
        private bool isDelayOut = true;


        private void Start()
        {
            animator = GetComponent<Animator>();
            targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        }

        private void OnTriggerStay(Collider other)
        {
            //turnAIdetector(other);
            fullyTurn(other);
        }
        
        /*
         *   Check if the Enemy that you are locked on
         *   its almost behind you, and if it is
         *   the player will automatically execute
         *   an animation and will face the enemy        
        */
        private void turnAIdetector(Collider other)
        {
            if (!animator.GetBool(AnimatorAshesh.isTargetLocked)
                    || !other.tag.Equals("Enemy")
                    || !other.transform.GetChild(0).Equals(targetGroup.m_Targets[0].target)
                    || !isDelayOut
                    || !animator.GetBool(AnimatorAshesh.isCameraFreed))
                return;
         
            direction = other.transform.position - transform.position;

            angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);

            if (angle < -90 || angle > 90)
            {
                transform.LookAt(other.transform);
                return;
            }

            if ( angle <= maxRightAngle)
            {
                rotateInPlace(AnimatorAshesh.isTurningRight, rightCompensator);
                StartCoroutine(turnDelay(delayTime));
            }
            else if (angle >= maxLeftAngle)
            {
                rotateInPlace(AnimatorAshesh.isTurningLeft, leftCompensator);
                StartCoroutine(turnDelay(delayTime));
            }
        }

        private void fullyTurn(Collider other)
        {
            if (animator.GetBool(AnimatorAshesh.isTargetLocked)
                    && animator.GetBool(AnimatorAshesh.isAttacking)
                    && other.tag.Equals("TargetEnemy"))
            {
                var transf = other.transform.position;
                transf.y = 0;
                transform.LookAt(transf);
            }

        }

        private IEnumerator turnDelay(float delay)
        {
            isDelayOut = false;
            yield return new WaitForSeconds(delay);
            isDelayOut = true;
        }

        /*
         *   Start the specified animation 
         *   and compensate the rotation 
         *   of the specifed amount
         */
        private void rotateInPlace(int anim, float compensator)
        {
            animator.SetTrigger(anim);
            transform.Rotate(0, compensator, 0);
        }
    }
}
