using System.Collections;
using UnityEngine;

namespace Art_Intelligence
{
    public class ArmoredSkeleton : BaseEnemyEntity,NormalEnemy ,EnemyUtilities, EnemyTimeProcessUtility,EnemyAngleHandlerUtility
    {
        private bool isCloseEnough;
        private bool isAttackStarted;
        private float percentual_attack;
  
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
                    _navMesh.isStopped = true;
                    StartCoroutine("alertController", 5f);
                    break;
            }

            if(animator.GetInteger(AnimatorAshesh.attackType) == 0 
                    && _states != AIStates.Alert)
                navMesh.isStopped = false;
        }

        #region BaseOverrides
        public override void velocyBlendAdjust()
        {
            _speed = navMesh.velocity.magnitude;
            _speed = (_states == AIStates.Pursuing) ? _speed : _speed / 2;
            animator.SetFloat(AnimatorAshesh.enemySpeed, _speed);
        }

        override protected void distance_attack(Collider other)
        {
            distance = Vector3.Distance(transform.position, other.transform.position);
            
            if (distance <= MaxDistancePlayer && isCloseEnough)
                states = AIStates.Attack;
            else
            {
                _states = AIStates.Pursuing;
                isCloseEnough = (distance <= navMesh.stoppingDistance) ? true : false;
            }
        }
        #endregion

        #region NormalEnemyInterface
        public void patrolling()
        {
            base.patrol();
        }

        public void attack(Collider other)
        {
            animator.SetBool(AnimatorAshesh.isTurning, (!animator.GetBool(AnimatorAshesh.isInProcess)));

            if (!isAttackStarted)
                StartCoroutine(attackTimingController(.2f, other));
        }
 
        public void pursuing(Collider other)
        {
            base.pursue(other);
        }

        public void die()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region OnUnityCall
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player") && states == AIStates.Alert)
            {
                triggerBackAttention();
                StopCoroutine("alertController");
            }
        }

        private float angle = 0;
        private void OnTriggerStay(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                distance_attack(other);

                enemyTurnAngle(other);

                switch (states)
                {
                    case AIStates.Pursuing:
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

        #region whereAreTheySituated
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

        public IEnumerator alertController(float alertTime)
        {
            alertSwitcher(true, AIStates.Alert);

            yield return new WaitForSeconds(alertTime);

            if (_states == AIStates.Alert)
                alertSwitcher(false, AIStates.Patrol);
        }

        public void alertSwitcher(bool isNavStopped, AIStates state)
        {
            navMesh.isStopped = isNavStopped;
            animator.SetBool(AnimatorAshesh.alertMode, isNavStopped);
            states = state;
        }

        public IEnumerator attackTimingController(float waitTime, Collider other)
        {
            isAttackStarted = true;

            while (_states == AIStates.Attack)
            {                
                percentual_attack = Random.value;

                if (percentual_attack > .3f && !animator.GetBool(AnimatorAshesh.isInProcess))
                    randomAttackCalculator();

                yield return new WaitForSeconds(waitTime);
            }
            isAttackStarted = false;
        }
     
        public void randomAttackCalculator()
        {
            float value = Random.value;
            navMesh.isStopped = true;

            if (value <= .25f)
                setAttack(1);
            else if (value <= .3f)
                setAttack(2);
            else if (value <= .5f)
                setAttack(3);
        }

        public void setAttack(int attackTypes)
        {
            animator.SetBool(AnimatorAshesh.isInProcess, true);
            animator.SetInteger(AnimatorAshesh.attackType, attackTypes);
            animator.SetTrigger(AnimatorAshesh.attack);
        }

        public IEnumerator waitStartAlert(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _states = AIStates.Alert;
        }

        public void enemyTurnAngle(Collider other)
        {
            if (animator.GetBool(AnimatorAshesh.isTurning)
                && animator.GetFloat(AnimatorAshesh.enemySpeed) < .2f)
            {
                Vector3 direction = other.transform.position - transform.position;
                angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);

                animator.SetBool(AnimatorAshesh.turnRight, (angle <= -40 && angle > -130 && !animator.GetBool(AnimatorAshesh.turnRight)) ? angleTurnControl(other, 5f) : false);
                animator.SetBool(AnimatorAshesh.turnLeft, (angle >= 40 && angle < 130 && !animator.GetBool(AnimatorAshesh.turnLeft)) ? angleTurnControl(other, 5f) : false);
                animator.SetBool(AnimatorAshesh.bigTurnRight, (angle >= 140 && !animator.GetBool(AnimatorAshesh.bigTurnRight)));
                animator.SetBool(AnimatorAshesh.bigTurnLeft, (angle <= -140 && !animator.GetBool(AnimatorAshesh.bigTurnLeft)));

            }
        }

        public bool angleTurnControl(Collider other, float turnSpeed)
        {
            base.facePlayer(other, turnSpeed);
            return true;
        }

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