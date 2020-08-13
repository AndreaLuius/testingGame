using System.Collections;
using UnityEngine;

namespace Art_Intellifence
{
    public class SkeltonEnemy : BaseEnemyEntity, NormalEnemy
    {
        private float distance = 0f;
        private System.Random rnd = new System.Random();
        private bool isStrafing;
        private Vector3 strafe_dir;
        private int casualStrafe;
        private float percentual_attack;

        //YOU CAN DIRECTLY USE THE STOPPING DISTANCE
        [SerializeField] float start_attack_distance;

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
                        strafe_switcher(AnimatorAshesh.isStrafingRight, AnimatorAshesh.isStrafingLeft);
                    else
                        strafe_switcher(AnimatorAshesh.isStrafingLeft, AnimatorAshesh.isStrafingRight);

                    time = 0;
                }

                navMesh.SetDestination(transform.position + strafe_dir);
            }

            if (!isAttackStarted) StartCoroutine(attackThingy(2f, other));
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
        private void OnTriggerStay(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                //checking attack distance
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
        private void alertSwitcher(bool hasToRun, AIStates state)
        {
            navMesh.isStopped = hasToRun;
            animator.SetBool(AnimatorAshesh.alertMode, hasToRun);
            states = state;
        }

        private void distance_attack(Collider other)
        {
            distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance <= start_attack_distance)
                states = AIStates.Attack;
            else
            {
                //TODO: change
                time = 6f;
                _states = AIStates.Pursuing;
            }
        }

        private void strafe_switcher(int animToStop, int animToStart)
        {
            animator.SetBool(animToStop, false);
            animator.SetBool(animToStart, true);
        }
        #endregion

        #region Cooroutines
        private IEnumerator alertController(int alertTime)
        {
            alertSwitcher(true, AIStates.Alert);

            yield return new WaitForSeconds(alertTime);

            alertSwitcher(false, AIStates.Patrol);
        }

        /*CHECK OR TO DELETE*/
        private IEnumerator enemyStrafe()
        {
            if (!isStrafing)
            {
                var casualStafe = rnd.Next(0, 2);

                if (casualStafe == 0)
                    strafe_switcher(AnimatorAshesh.isStrafingRight, AnimatorAshesh.isStrafingLeft);
                else
                    strafe_switcher(AnimatorAshesh.isStrafingLeft, AnimatorAshesh.isStrafingRight);

                navMesh.SetDestination(transform.position + strafe_dir);
                isStrafing = true;

                yield return new WaitForSeconds(rnd.Next(5, 8));

                isStrafing = false;
            }
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
                    /*
                    If the attack hits the player
                    the mob continues to attack, executing combos, if mob
                    notices that the stamina of the player is low, the mob starts
                    attacking, if the mob gets hit, it gonna try to defend itself 
                    */
                    animator.SetInteger("attackType", ((UnityEngine.Random.value > .4) ? 1 : 3));
                    animator.SetTrigger("attack");
                    ////////////////////////////////////////////////////////
                    var sec = other.GetComponentInChildren<PlayerProperties>();

                }

                yield return new WaitForSeconds(waitTime);
            }
            isAttackStarted = false;
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