using UnityEngine;
using Cinemachine;

namespace TargetSystem
{
    public class TargetUiFollower : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] CinemachineTargetGroup targetGroup;
        private Transform old;
        private UITarget uiTarget;
        private Vector3 whereToLay;
        private Camera camera_main;
        [SerializeField] Vector3 offset;


        private void Start()
        {
            animator = GetComponent<Animator>();
            camera_main = Camera.main;
        }

        private void OnTriggerStay(Collider other)
        {
            lockOnUIHandler(other);
        }

        /*Enables and disable the ui when locking on target
         * it only executes the getComponenet once at the beginning
         * and only when we change the target because it has to
         * point to another memory location.
        */
        private void lockOnUIHandler(Collider other)
        {
            if (animator.GetBool(AnimatorAshesh.isTargetLocked) && other.transform.tag.Equals("Enemy"))
            {
                var lockOn = targetGroup.m_Targets[0];

                if (!old)
                {
                    old = lockOn.target;
                    uiTarget = old.transform.GetChild(0).GetComponent<UITarget>();
                }
                else if (!old.Equals(lockOn.target))
                {
                    old = lockOn.target;
                    uiTarget = old.GetChild(0).GetComponent<UITarget>();
                }

                whereToLay = camera_main.WorldToScreenPoint(lockOn.target.transform.position + offset);
                uiTarget.img.transform.position = whereToLay;
                uiTarget.img.enabled = true;

                if (uiTarget.isDead) uiTarget.img.enabled = false;
            }

              if (!animator.GetBool(AnimatorAshesh.isTargetLocked) && uiTarget != null)
                uiTarget.img.enabled = false;
        }
    }
}
