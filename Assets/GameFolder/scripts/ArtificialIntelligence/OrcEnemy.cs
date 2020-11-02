using System.Collections;
using UnityEngine;

namespace Art_Intelligence
{
    public class OrcEnemy : BaseEnemyEntity, NormalEnemy, EnemyUtilities,EnemyTimeProcessUtility,EnemyAngleHandlerUtility
    {
        private float patrolSpeed = 1f;
        private float pursueSpeed = 2.2f;
        private bool isAttackStarted;
        private float percentual_attack;
        private bool isCloseEnough;
        private bool isNavAlertFree = true;
        private bool isAlertControllerStopped = false;


        private void Update()
        {
            switch (_states)
            {
                case AIStates.Patrol:
                    patrolling();
                    break;
                case AIStates.Alert:
                    StartCoroutine("alertController", 5f);
                    break;
            }

            if (!animator.GetBool(AnimatorAshesh.jumpTowards) 
                    && !animator.GetBool(AnimatorAshesh.dodgeBack)
                    && !animator.GetBool(AnimatorAshesh.turnLeft)
                    && !animator.GetBool(AnimatorAshesh.turnRight)
                    && isNavAlertFree && animator.GetInteger(AnimatorAshesh.attackType) == 0)
                navMesh.isStopped = false;
          }

        private void LateUpdate()
        {
            this.velocyBlendAdjust();
        }

        /*TODO:
         * #ADD hit animation and death  
        */
        #region BaseOverrides
        public override void velocyBlendAdjust()
        {
            _speed = navMesh.velocity.magnitude;
            _speed = (_states == AIStates.Pursuing) ? _speed: _speed/2;
            animator.SetFloat(AnimatorAshesh.enemySpeed, _speed);
        }

        override protected void distance_attack(Collider other)
        {
            distance = Vector3.Distance(transform.position, other.transform.position);

            jumpOrDodgeController();

            if (distance <= MaxDistancePlayer && isCloseEnough)
                states = AIStates.Attack;
            else
            {
                _states = AIStates.Pursuing;
                isCloseEnough = (distance <= navMesh.stoppingDistance) ? true : false;
            }
        }
        #endregion

        #region NormalEnemyImpl
        public void patrolling()
        {
            navMesh.speed = patrolSpeed;
            base.patrol();
        }

        public void pursuing(Collider other)
        {
            navMesh.speed = pursueSpeed;
            if(!animator.GetBool(AnimatorAshesh.jumpTowards)) base.facePlayer(other, 3f);

            navMesh.SetDestination(other.transform.position);
        }

        public void attack(Collider other)
        {
            animator.SetBool(AnimatorAshesh.isTurning, (!animator.GetBool(AnimatorAshesh.isInProcess)));

            if (!isAttackStarted && !animator.GetBool(AnimatorAshesh.dodgeBack))
                StartCoroutine(attackTimingController(.25f, other));
        }

        public void die()
        {
            //TODO: add die
            throw new System.NotImplementedException();
        }
        #endregion

        #region OnUnityCall
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player") && states == AIStates.Alert)
            {
                animator.SetBool(AnimatorAshesh.alertMode, false);
                StartCoroutine(backIn(3.8f));
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
                        if(angle <= -13f || angle >= 13)
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

        #region AngleUtilities
        public void enemyTurnAngle(Collider other)
        {
            Vector3 direction = other.transform.position - transform.position;
            angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);

            if (animator.GetBool(AnimatorAshesh.isTurning)
                  && !animator.GetBool(AnimatorAshesh.dodgeBack)
                  && !animator.GetBool(AnimatorAshesh.jumpTowards)
                  && animator.GetFloat(AnimatorAshesh.enemySpeed) < .2f)
            {
                animator.SetBool(AnimatorAshesh.turnLeft, (angle >= 15f) ? angleTurnControl(other,2.7f) : false);

                animator.SetBool(AnimatorAshesh.turnRight, (angle <= -15f) ? angleTurnControl(other,2.3f) : false);

                //todo: use this in case there is a little of no
                //animation a the beginning of the turn
                //if(animator.GetBool(AnimatorAshesh.turnLeft) ||
                //    animator.GetBool(AnimatorAshesh.turnLeft))
                //{
                //    base.facePlayer(other, 2.5f);
                //}
            }
        }

        /*TODO:Check this when creating a new mobAI
          if you need this method just put this in 
          the enemyAngleUtility interface and override it*/
        public bool angleTurnControl(Collider other,float turnSpeed)
        {
            navMesh.isStopped = true;
            base.facePlayer(other, turnSpeed);
            return true;
        }
       
        #endregion

        #region TimeUtilities
        public IEnumerator alertController(float alertTime)
        {
            alertSwitcher(true, AIStates.Alert);
            isNavAlertFree = false;

            yield return new WaitForSeconds(alertTime);
            isNavAlertFree = true;

            if (_states == AIStates.Alert)
                alertSwitcher(false, AIStates.Patrol);
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

        public IEnumerator waitStartAlert(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _states = AIStates.Alert;
        }
        #endregion

        #region Utilities
        public void randomAttackCalculator()
        {
            float value = Random.value;
            navMesh.isStopped = true;

            if (value <= .25f)
                setAttack(4);
            else if (value <= .3f)
                setAttack(2);
            else if (value <= .5f)
                setAttack(1);
        }

        public void setAttack(int attackTypes)
        {
            animator.SetBool(AnimatorAshesh.isInProcess, true);
            animator.SetInteger(AnimatorAshesh.attackType, attackTypes);
            animator.SetTrigger(AnimatorAshesh.attack);
        }

        public void alertSwitcher(bool isNavStopped,AIStates state)
        {
             navMesh.isStopped = isNavStopped;
             animator.SetBool(AnimatorAshesh.alertMode, isNavStopped);
             states = state;
        }

        /*Waits for the alertAnimation to finish 
        to reactivate the navmesh*/
        private IEnumerator backIn(float time)
        {
            StopCoroutine("alertController");
            yield return new WaitForSeconds(time);

            isNavAlertFree = true;
        }

        /*
         * Dodge or jump towards
         * the player based on the
         * distance from the player.
         * Remember the JumpTowards 
         * is gonna be Randomized to make
         * it more realistic!        
        */
        private void jumpOrDodgeController()
        {
            if (distance <= .8f)
                stopNav_Evaluate(true, AnimatorAshesh.dodgeBack);
            else if (distance >= 3 && distance <= 3.5 && Random.value >= .99f)
                stopNav_Evaluate(true, AnimatorAshesh.jumpTowards);
        }

        /*
         * Stops or activates the nav
         * by the @isNavStopped
         * and evaluates a animation
         * by specifying its hash @anim
        */
        private void stopNav_Evaluate(bool isNavStopped,int anim)
        {
            navMesh.isStopped = isNavStopped;
            animator.SetBool(anim, true);
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
