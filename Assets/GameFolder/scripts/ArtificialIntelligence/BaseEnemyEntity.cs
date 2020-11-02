using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

namespace Art_Intelligence
{
    public abstract class BaseEnemyEntity : MonoBehaviour
    {
        protected Animator _animator;
        protected NavMeshAgent _navMesh;
        protected AIStates _states;
        protected float _speed = 1;
        protected int pointsDestination = 0;
        protected float distance = 0f;
        [SerializeField] float maxDistancePlayer = 3f;
        [SerializeField] protected List<PatrolPoints> _patrolPoints = new List<PatrolPoints>();
        [SerializeField] float delayTime = 3;
        private float baseDistancePatrol = 2f;



        private void Start()
        {
            _animator = GetComponent<Animator>();
            _navMesh = GetComponent<NavMeshAgent>();
            _states = AIStates.Patrol;
            //Physics.IgnoreCollision(transform.GetChild(0).GetComponent<Collider>(), transform.GetChild(0).GetChild(4).GetComponent<Collider>(),true);

        }

        public virtual void velocyBlendAdjust()
        {
            _speed = navMesh.velocity.magnitude;
            animator.SetFloat(AnimatorAshesh.enemySpeed, _speed);
        }

        public void facePlayer(Collider other,float delayTime)
        {
            Vector3 direction = (other.transform.position - transform.position);

            if (System.Math.Abs(direction.x) > Mathf.Epsilon)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.right);
                transform.localRotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * delayTime);
            }
        }

        protected virtual void patrol()
        {
            pointsDestination = (pointsDestination == _patrolPoints.Count) ? 0 : pointsDestination;

            if (pointsDestination == 0) StartCoroutine(patrolTime(3f, true));
          
            if (Vector3.Distance(transform.position, _patrolPoints[pointsDestination].transform.position)
                                        <= baseDistancePatrol)
            {
                StartCoroutine(patrolTime(3f, false));
                pointsDestination++;
            }
        }

        public virtual void pursue(Collider other)
        {
            facePlayer(other,delayTime);
            navMesh.SetDestination(other.transform.position);
        }

        protected virtual void distance_attack(Collider other)
        {
            distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance <= maxDistancePlayer)
                states = AIStates.Attack;
            else
                _states = AIStates.Pursuing;
        }

        protected virtual IEnumerator patrolTime(float waitTime, bool isItFirst)
        {
            if (!isItFirst)
            {
                yield return new WaitForSeconds(waitTime);
                _navMesh.SetDestination(_patrolPoints[pointsDestination].transform.position);
            }
            else
            {
                _navMesh.SetDestination(_patrolPoints[pointsDestination].transform.position);
                yield return new WaitForSeconds(waitTime);
            }
        }

        #region Properties
        public Animator animator { get { return _animator; } }
        public NavMeshAgent navMesh { get { return _navMesh; } }
        public AIStates states { get { return _states; } set { _states = value; } }
        public float speed { get { return _speed; } }
        public float MaxDistancePlayer { get { return maxDistancePlayer; } }
        public List<PatrolPoints> patrolPoints { get { return _patrolPoints; } set { _patrolPoints = value; } }
        #endregion
    }
}