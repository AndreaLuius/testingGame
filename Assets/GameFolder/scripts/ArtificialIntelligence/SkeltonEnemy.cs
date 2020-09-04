using System.Collections;
using UnityEngine;

namespace Art_Intelligence
{
    /*
        1)check the animation wait for the alert if the player
        gets back in the trigger while its in alert mode,
        2)and then check the distance from the enemy 
        in lock mode
    */
    public class SkeltonEnemy : BaseEnemyEntity, NormalEnemy, EnemyUtilities
    {
        private System.Random rnd = new System.Random();
        private bool isStrafing;
        private Vector3 strafe_dir;
        private int casualStrafe;
        private float percentual_attack;
        private bool isAttackStarted;

        private void LateUpdate()
        {
            base.velocyBlendAdjust();
        }

        private void Update()
        {
            switch (_states)
            {
                case AIStates.Patrol:
                    patrolling();
                    break;
            }

        }

        #region NormalEnemyInterfaceImpl
        private double time = 6f;
        public void attack(Collider other)
        {
            transform.LookAt(other.transform.position);

            Vector3 offsetPlayer = other.transform.position - transform.position;
            //allows me to take the right (right or left depending on 
            // the magnitude between the vector of the player and enemy)
            strafe_dir = Vector3.Cross(offsetPlayer, Vector3.up);

            time += 0.1 * Time.deltaTime;

            if (animator.GetInteger(AnimatorAshesh.attackType) == 0)
            {
                if (time >= 6)
                {
                    casualStrafe = rnd.Next(0, 2);

                    if (casualStrafe == 0)
                        AnimatorAshesh.animSwitcher(animator, AnimatorAshesh.isStrafingRight, AnimatorAshesh.isStrafingLeft);
                    else
                        AnimatorAshesh.animSwitcher(animator, AnimatorAshesh.isStrafingLeft, AnimatorAshesh.isStrafingRight);

                    time = 0;
                }
                navMesh.SetDestination(transform.position + strafe_dir);
            }
            if (!isAttackStarted) StartCoroutine(attackThingy(1.6f, other));
        }

        public void die()
        {
        }

        public void patrolling()
        {
            base.patrol();
        }

        public void pursuing(Collider other)
        {
            base.pursue(other);
        }
        #endregion

        #region UnityCalls
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player") && _states == AIStates.Alert)
            {
                _states = AIStates.Pursuing;
                animator.SetBool("isAlerted", false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                distance_attack(other);

                switch (_states)
                {
                    case AIStates.Pursuing:
                        AnimatorAshesh.boolAnimatorToggler(animator, false,
                            AnimatorAshesh.isStrafingLeft, AnimatorAshesh.isStrafingRight);
                        pursuing(other);
                        break;
                    case AIStates.Attack:
                        attack(other);
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _states = AIStates.Alert;
                StartCoroutine(alertController(5));
            }
        }
        #endregion

        #region UtilityFunc
        override protected void distance_attack(Collider other)
        {
            distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance <= navMesh.stoppingDistance)
                states = AIStates.Attack;
            else
            {
                time = 6f;
                _states = AIStates.Pursuing;
            }
        }

        private void alertSwitcher(bool hasToRun, AIStates state)
        {
            navMesh.isStopped = hasToRun;
            animator.SetBool(AnimatorAshesh.alertMode, hasToRun);
            states = state;
        }

        private void setAttack(int attackType)
        {
            animator.SetInteger(AnimatorAshesh.attackType, attackType);
            animator.SetTrigger(AnimatorAshesh.attack);
        }
        #endregion

        #region Cooroutines
        private IEnumerator alertController(int alertTime)
        {
            alertSwitcher(true, AIStates.Alert);

            yield return new WaitForSeconds(alertTime);

            alertSwitcher(false, AIStates.Patrol);
        }

        private IEnumerator attackThingy(float waitTime, Collider other)
        {
            isAttackStarted = true;

            while (_states == AIStates.Attack)
            {
                percentual_attack = UnityEngine.Random.value;

                if (percentual_attack > .5f)
                {
                    AnimatorAshesh.boolAnimatorToggler(animator, false,
                        AnimatorAshesh.isStrafingLeft, AnimatorAshesh.isStrafingRight);
                    time = 6f;

                    randomAttackCalculator();
                }
                yield return new WaitForSeconds(waitTime);
            }
            isAttackStarted = false;
        }
        #endregion

        #region EnemyUtilitiesImpl
        public void randomAttackCalculator()
        {
            float value = UnityEngine.Random.value;

            if (value <= .2f)
                setAttack(4);
            else if (value <= .25f)
                setAttack(2);
            else if (value <= .3f)
                setAttack(3);
            else if (value <= .6f)
                setAttack(1);
        }
        #endregion

        #region EditorDebugMethods
        /**
            Draws on the editor the position 
            of the wayPoints the enemy patrols to
        */
        private void OnDrawGizmos()
        {
            for (int i = 0; i < _patrolPoints.Count; i++)
            {
                int j = (i != _patrolPoints.Count - 1) ? i + 1 : 0;
                Gizmos.DrawWireSphere(_patrolPoints[i].transform.position, 1f);
                Gizmos.DrawLine(_patrolPoints[i].transform.position, _patrolPoints[j].transform.position);
            }
        }
        #endregion
    }
}
