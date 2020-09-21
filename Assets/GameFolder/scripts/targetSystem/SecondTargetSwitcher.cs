using UnityEngine;
using System.Collections;
using Cinemachine;

namespace TargetSystem
{ 
    public class SecondTargetSwitcher : MonoBehaviour
    {
        [SerializeField] ControllingTargets controllingTargets;
        [SerializeField] CinemachineTargetGroup targetGroup;
        [SerializeField] float targetDelay = .2f;
        private Animator animator;
        private bool isDelayOut = false;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            switching();
        }
     
        private void switching()
        {
            if (!animator.GetBool(AnimatorAshesh.isTargetLocked) || isDelayOut) return;

            if (Input.GetAxis("RightStickCameraX") < -.8f)
            {
                if (!targetGroup.IsEmpty) targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
                controllingTargets.closestNumber -= (controllingTargets.closestNumber) > 0 ? 1 : 0;

               if (controllingTargets.closestNumber >= 0)
                    targetGroup.AddMember(controllingTargets.EnemiesList[controllingTargets.closestNumber].Transform, 100, 100);
                StartCoroutine(targettingDelay());
            }
            else if (Input.GetAxis("RightStickCameraX") > .8f)
            {
                if (!targetGroup.IsEmpty) targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];

                    controllingTargets.closestNumber += (controllingTargets.closestNumber <= (controllingTargets.EnemiesList.Count - 2)) ? 1 : 0;

                if (controllingTargets.closestNumber <= (controllingTargets.EnemiesList.Count - 1))
                    targetGroup.AddMember(controllingTargets.EnemiesList[controllingTargets.closestNumber].Transform, 100, 100);
                StartCoroutine(targettingDelay());
            }
        }

        /*
         * Adds the specified delay
         * when we switch target       
        */
        private IEnumerator targettingDelay()
        {
            isDelayOut = true;
            yield return new WaitForSeconds(targetDelay);
            isDelayOut = false;
        }
    }
}
