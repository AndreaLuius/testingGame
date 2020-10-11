using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TargetSystem
{
    public class ControllingTargets : MonoBehaviour, TargetUtilities
    {
        [SerializeField] CinemachineTargetGroup targetGroup;
        public List<EnemyPosition> enemiesList = new List<EnemyPosition>();
        private Animator animator;
        private Transform closestTarget;
        [SerializeField] float viewAngle = 80f;
        private bool isTargetLocked;
        private float theDistance = 0f;
        private float calculatedDistance = 0f;

        public int closestNumber = 0;


        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            isTargetLocked = animator.GetBool(AnimatorAshesh.isTargetLocked);

            if (Input.GetButtonDown("GamepadLockOn")
                    && !targetGroup.IsEmpty && animator.GetBool(AnimatorAshesh.arming))
                animator.SetBool(AnimatorAshesh.isTargetLocked, !isTargetLocked);
         } 
        
        void OnTriggerStay(Collider other)
        {
            closestEnemy(other);
            controlViewOverTime(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("sensorTrigger"))
            {
                targetGroup.RemoveMember(closestTarget);
                enemiesList.Clear();
                isTargetLocked = false;
                animator.SetBool(AnimatorAshesh.isTargetLocked, isTargetLocked);
            }
        }

        private void closestEnemy(Collider other)
        {
            if (isTargetLocked) return;

            for (int i = 0; i < enemiesList.Count; i++)
            {
                calculatedDistance = Vector3.Distance(enemiesList[i].Transform.position, this.transform.position);
                if ((System.Math.Abs(theDistance) < Mathf.Epsilon || calculatedDistance < theDistance))
                {
                    theDistance = calculatedDistance;
                    closestTarget = enemiesList[i].Transform;
                    closestNumber = i;
                }
                else if (i == (enemiesList.Count - 1))
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

            return Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);
        }

        private void controlViewOverTime(Collider other)
        {
            if (!other.tag.Equals("Enemy")) return;

            float enemyAngle = playerViewAngle(other);

            Transform otherTransform = other.transform.GetChild(0);

            if (enemyAngle <= viewAngle || enemyAngle >= -viewAngle)
            {
                enemiesList.Add(new EnemyPosition(otherTransform.transform, enemyAngle));

                for (int i = 0; i < enemiesList.Count; i++)
                {
                    for(int j = 0; j < enemiesList.Count; j++)
                    {
                        if (i == j) continue;

                        if (enemiesList[i].Transform.name.Equals(enemiesList[j].Transform.name))
                        {
                            enemiesList[i].EnemyAngle = enemiesList[j].EnemyAngle;
                            enemiesList.Remove(enemiesList[j]);
                        }
                    }
                }
                enemiesList.Sort(sortByEnemyAngle);
            }
            //else if (enemyAngle > viewAngle)
            //{
            //    targetGroup?.RemoveMember(otherTransform);
            //    list.Remove(otherTransform);
            //}
        }
        #endregion
       
        private int sortByEnemyAngle(EnemyPosition a, EnemyPosition b)
        {
            if (a.EnemyAngle <= b.EnemyAngle)
                return 1;
            if (a.EnemyAngle >= b.EnemyAngle)
                return -1;
            else
                return 0;   
        }

        #region Properties
        public bool IsTargetLocked { get { return isTargetLocked; } }
        public float ViewAngle { get { return viewAngle; } }
        public List<EnemyPosition> EnemiesList { get { return enemiesList; } }
        public Transform ClosestTarget { get { return closestTarget; } }
        #endregion

        //private static class OldCode
        //{
        //public List<Transform> list = new List<Transform>();

        //private void controlViewOverTime(Collider other)
        //{
        //    if (isTargetLocked || !other.tag.Equals("Enemy")) return;

        //    float enemyAngle = playerViewAngle(other);

        //    Transform otherTransform = other.transform.GetChild(0);

        //    if (!list.Contains(otherTransform)
        //            && enemyAngle <= viewAngle)
        //        list.Add(otherTransform);
        //    else if (enemyAngle > viewAngle)
        //    {
        //        targetGroup?.RemoveMember(otherTransform);
        //        list.Remove(otherTransform);
        //    }
        //}

        //private void closestEnemy(Collider other)
        //{
        //    if (isTargetLocked) return;

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        calculatedDistance = Vector3.Distance(list[i].transform.position, this.transform.position);
        //        if ((System.Math.Abs(theDistance) < Mathf.Epsilon || calculatedDistance < theDistance))
        //        {
        //            theDistance = calculatedDistance;
        //            closestTarget = list[i];
        //            closestNumber = i;
        //        }
        //        else if (i == (list.Count - 1))
        //        {
        //            theDistance = 0f;
        //            if (!targetGroup.IsEmpty) targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
        //            targetGroup.AddMember(closestTarget, 100, 100);
        //        }
        //    }
        //}

        //public float playerViewAngle(Collider other)
        //{
        //    Vector3 targetDirection = other.transform.position - this.transform.position;

        //    return Vector3.Angle(targetDirection, transform.forward);
        //}
        //}
    }
}
