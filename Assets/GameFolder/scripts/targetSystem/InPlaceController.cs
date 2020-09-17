using UnityEngine;
using Cinemachine;

namespace TargetSystem
{
    public class InPlaceController : MonoBehaviour
    {
        private Animator animator;
        private CinemachineTargetGroup targetGroup;
        [SerializeField] float maxRightAngle = -95f;
        [SerializeField] float maxLeftAngle = 35f;
        [SerializeField] float rightCompensator = 10f;
        [SerializeField] float leftCompensator = -30;
        private float angle = 0;
        private Vector3 direction;

        private void Start()
        {
            animator = GetComponent<Animator>();
            targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        }

        private void OnTriggerStay(Collider other)
        {
            turnAIdetector(other);
        }

        /*
         *   Start the specified animation 
         *   and compensate the rotation 
         *   of the specifed amount
        */
        private void rotateInPlace(int anim,float compensator)
        {
            animator.SetTrigger(anim);
            transform.Rotate(0, compensator, 0);
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
                || !other.transform.Equals(targetGroup.m_Targets.GetValue(0))
                    || !animator.GetBool(AnimatorAshesh.isCameraFreed))
                return;

            direction = other.transform.position - transform.position;

            angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);

            if (angle <= maxRightAngle)
                rotateInPlace(AnimatorAshesh.isTurningRight, rightCompensator);
            else if (angle >= maxLeftAngle)
                rotateInPlace(AnimatorAshesh.isTurningLeft, leftCompensator);
        }
    }
}
