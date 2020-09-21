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

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerStay(Collider other)
        {
            lockOnUIHandler(other);
        }

        //TODO: not sure about optimization
        private void lockOnUIHandler(Collider other)
        {
            if (animator.GetBool(AnimatorAshesh.isTargetLocked) && other.transform.tag.Equals("Enemy"))
            {
                var lockOn = targetGroup.m_Targets[0];

                if (!old)
                {
                    old = lockOn.target;
                    uiTarget = old.GetComponentInChildren<UITarget>();
                }
                else if (!old.Equals(lockOn.target))
                {
                    old = lockOn.target;
                    uiTarget = old.GetComponentInChildren<UITarget>();
                }

                whereToLay = Camera.main.WorldToScreenPoint(new Vector3(uiTarget.transform.position.x, uiTarget.transform.position.y, uiTarget.transform.position.z));
                uiTarget.img.transform.position = whereToLay;
                uiTarget.img.enabled = true;
            }

            if (!animator.GetBool(AnimatorAshesh.isTargetLocked) && uiTarget != null)
                uiTarget.img.enabled = false;
        }
    }
}
