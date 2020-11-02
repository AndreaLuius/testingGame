using UnityEngine;
using Cinemachine;

namespace TargetSystem
{
    public class TargetUiHealth : MonoBehaviour
    {
        private Animator animator;
        private CinemachineTargetGroup targetGroup;
        private GameObject old;
        private bool isAvailable = false;

        /*TODO:
         * and then the Handlers after that check the AI
         * and optimize it as well.
        */
        private void Start()
        {
            animator = GetComponent<Animator>();
            targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        }

        private void OnTriggerStay(Collider other)
        {
            uiHealthHandler(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.tag.Equals("TargetEnemy"))
                targetGroup.m_Targets[0].target.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }

        /*Checks whether the player is locked on an enemy
        if it is it shows the health if the player switches 
        the target it is gonna point to the other health
        as soon as the target is no longer locked it will stop
        rendering the health bar*/
        private void uiHealthHandler(Collider other)
        {
            var isTargetLocked = animator.GetBool(AnimatorAshesh.isTargetLocked);

            if (other.tag.Equals("TargetEnemy") && isTargetLocked)
            {
                var tmp = targetGroup.m_Targets[0].target.transform.GetChild(1).GetChild(0).gameObject;

                if (isAvailable && !old.Equals(tmp))
                    old.SetActive(false);

                old = tmp;
                old.SetActive(true);

                isAvailable = true;
            }
            else if (other.tag.Equals("TargetEnemy") && !isTargetLocked
                        && isAvailable && targetGroup.m_Targets.Length > 0)
            {
                old.SetActive(false);
                isAvailable = false;
            }
        }

    }
}
