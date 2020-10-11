using System.Collections;
using UnityEngine;

namespace Art_Intelligence
{
    public class SkeltonEnemy : BaseEnemyEntity, NormalEnemy, EnemyUtilities,EnemyTimeProcessUtility
    { 
        private System.Random rnd = new System.Random();
        private bool isStrafing;
        private Vector3 strafe_dir;
        private int casualStrafe;
        private float percentual_attack;
        private bool isAttackStarted;
        private bool isCloseEnough = true;
        private bool isAlertControllerStopped = false;

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
                case AIStates.Alert:
                    StartCoroutine("alertController",5f);
                    break;
            }
        }

        #region NormalEnemyInterfaceImpl
        private double time = .5f;
        public void attack(Collider other)
        {
            transform.LookAt(other.transform.position);

            Vector3 offsetPlayer = other.transform.position - transform.position;
            //allows to take the right (right or left depending on 
            // the magnitude between the vector of the player and enemy)
            strafe_dir = Vector3.Cross(offsetPlayer, Vector3.right);

            time += 0.1 * Time.deltaTime;

            if (animator.GetInteger(AnimatorAshesh.attackType) == 0)
            {
                if (time >= .5)
                {
                    casualStrafe = rnd.Next(0, 2);

                    switch(casualStrafe)
                    {
                        case 0:
                            AnimatorAshesh.animSwitcher(animator, AnimatorAshesh.isStrafingRight, AnimatorAshesh.isStrafingLeft);
                            break;
                        case 1:
                            AnimatorAshesh.animSwitcher(animator, AnimatorAshesh.isStrafingLeft, AnimatorAshesh.isStrafingRight);
                            break;
                    }

                    time = 0;
                }
                navMesh.SetDestination(transform.position  + (strafe_dir));
            }
            if (!isAttackStarted) StartCoroutine(attackTimingController(.8f, other));
        }

        public void die()
        {
            //TODO: implement death
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
            if (other.tag.Equals("Player") && states == AIStates.Alert)
            {
                triggerBackAttention();
                StopCoroutine("alertController");
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                distance_attack(other);

                switch (states)
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
                StartCoroutine(waitStartAlert(2f));
            
        }
        #endregion

        #region UtilityFunc
        override protected void distance_attack(Collider other)
        {
            distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance <= MaxDistancePlayer && isCloseEnough)
                states = AIStates.Attack;
            else
            {
                time = 6f;
                _states = AIStates.Pursuing;
                isCloseEnough = (distance <= navMesh.stoppingDistance) ? true : false;
            }
        }

        public void alertSwitcher(bool isNavStopped,AIStates state)
        {
            navMesh.isStopped = isNavStopped;
            animator.SetBool(AnimatorAshesh.alertMode, isNavStopped);
            states = state;
        }


        public void setAttack(int attackType)
        {
            animator.SetBool(AnimatorAshesh.isInProcess, true);
            animator.SetInteger(AnimatorAshesh.attackType, attackType);
            animator.SetTrigger(AnimatorAshesh.attack);
        }
        /*
            If the mob is in alertMode state and
            it sees a player this will trigger method
            will reactivate the navMesh and stops the alert
        */
        private void triggerBackAttention()
        {
            animator.SetBool(AnimatorAshesh.alertMode, false);
            _states = AIStates.Pursuing;
            navMesh.isStopped = false;
        }
        #endregion

        #region Cooroutines

        public IEnumerator waitStartAlert(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _states = AIStates.Alert;
        }

        public IEnumerator alertController(float alertTime)
        {       
            alertSwitcher(true,AIStates.Alert);

            yield return new WaitForSeconds(alertTime);

            if (_states == AIStates.Alert)
                alertSwitcher(false,AIStates.Patrol);

        }

        public IEnumerator attackTimingController(float waitTime, Collider other)
        {   
             isAttackStarted = true;

            while (_states == AIStates.Attack)
            {
                percentual_attack = Random.value;

                if (percentual_attack > .3f && !animator.GetBool(AnimatorAshesh.isInProcess))
                {
                    AnimatorAshesh.boolAnimatorToggler(animator, false,
                        AnimatorAshesh.isStrafingLeft, AnimatorAshesh.isStrafingRight);
                    time = .5f;

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
            float value = Random.value;

            if (value <= .2f)
                setAttack(4);
            else if (value <= .25f)
                setAttack(2);
            else if (value <= .3f)
                setAttack(3);
            else if (value <= .5f)
                setAttack(1);
        }
        #endregion

        #region EditorDebugMethods
        /*
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
