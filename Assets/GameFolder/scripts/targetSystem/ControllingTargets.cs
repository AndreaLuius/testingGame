using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TargetSystem
{
    public class ControllingTargets : MonoBehaviour, TargetUtilities
    {
        [SerializeField] CinemachineTargetGroup targetGroup;
        private List<Transform> list = new List<Transform>();
        private Animator animator;
        private Transform closestTarget;
        [SerializeField] float viewAngle = 70f;
        private bool isTargetLocked;
        private float theDistance = 0f;
        private float calculatedDistance = 0f;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            isTargetLocked = animator.GetBool(AnimatorAshesh.isTargetLocked);

            if (Input.GetKeyDown(KeyCode.R)
                    && !targetGroup.IsEmpty && animator.GetBool(AnimatorAshesh.arming))
                animator.SetBool(AnimatorAshesh.isTargetLocked, !isTargetLocked);
        }

        void OnTriggerEnter(Collider other)
        {
            /*
                NOT SURE IF THIS GONNA BE DELETED
            */
            /* float enemyAngle = playerViewAngle(other); */

            /* if (other.tag.Equals("Enemy")
                && !list.Contains(other.transform))
                list.Add(other.transform); */
        }

        void OnTriggerStay(Collider other)
        {
            closestEnemy(other);
            /*
                NOT SURE IF THIS GOONA BE KEPT
            */
            controlViewOverTime(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (closestTarget == null) return;

            if (other.tag.Equals("Enemy") && other.name.Equals(closestTarget.name))
            {
                targetGroup.RemoveMember(closestTarget);
                list.Clear();
                isTargetLocked = false;
                animator.SetBool(AnimatorAshesh.isTargetLocked, isTargetLocked);
            }
        }

        private void closestEnemy(Collider other)
        {
            if (isTargetLocked) return;

            for (int i = 0; i < list.Count; i++)
            {
                calculatedDistance = Vector3.Distance(list[i].transform.position, this.transform.position);

                if ((theDistance == 0f || calculatedDistance < theDistance))
                {
                    theDistance = calculatedDistance;
                    closestTarget = list[i];
                }
                else if (i == (list.Count - 1))
                {
                    theDistance = 0f;
                    if (!targetGroup.IsEmpty) targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
                    targetGroup.AddMember(closestTarget, 100, 100);
                }
            }
        }

        #region ViewAngleControl

        public float playerViewAngle(Collider other)
        {
            Vector3 targetDirection = other.transform.position - this.transform.position;

            return Vector3.Angle(targetDirection, transform.forward);
        }

        private void controlViewOverTime(Collider other)
        {
            if (isTargetLocked) return;

            float enemyAngle = playerViewAngle(other);

            if (other.tag.Equals("Enemy") && !list.Contains(other.transform)
                    && enemyAngle <= viewAngle)
                list.Add(other.transform);
            else if (enemyAngle > viewAngle)
            {
                targetGroup.RemoveMember(other.transform);
                list.Remove(other.transform);
            }
        }
        #endregion

        #region Properties
        public bool IsTargetLocked { get { return isTargetLocked; } }
        public float ViewAngle { get { return viewAngle; } }
        #endregion
    }
}
