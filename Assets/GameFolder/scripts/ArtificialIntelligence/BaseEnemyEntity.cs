using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

namespace Art_Intellifence
{
    public abstract class BaseEnemyEntity : MonoBehaviour
    {
        protected Animator _animator;
        protected NavMeshAgent _navMesh;
        protected AIStates _states;
        protected float _speed = 1;
        protected int pointsDestination = 0;

        [SerializeField] protected List<PatrolPoints> _patrolPoints = new List<PatrolPoints>();


        private void Start()
        {
            _animator = GetComponent<Animator>();
            _navMesh = GetComponent<NavMeshAgent>();
            _states = AIStates.Patrol;
        }

        public void velocyBlendAdjust()
        {
            _speed = navMesh.velocity.magnitude;
            animator.SetFloat(AnimatorAshesh.enemySpeed, speed);
        }

        public void facePlayer(Collider other)
        {
            Vector3 direction = (other.transform.position - transform.position);
            if (direction.x != 0)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.localRotation = Quaternion.Slerp(transform.rotation, lookRotation, 4f);
            }
        }

        protected virtual void patrol()
        {
            pointsDestination = (pointsDestination == _patrolPoints.Count) ? 0 : pointsDestination;

            if (pointsDestination == 0) StartCoroutine(patrolTime(3f, true));

            if (Vector3.Distance(transform.position, _patrolPoints[pointsDestination].transform.position)
                                        <= (_navMesh.stoppingDistance))
            {
                StartCoroutine(patrolTime(3f, false));
                pointsDestination++;
            }
        }

        public virtual void pursue(Collider other)
        {
            navMesh.SetDestination(other.transform.position);
            facePlayer(other);
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
        public List<PatrolPoints> patrolPoints { get { return _patrolPoints; } set { _patrolPoints = value; } }
        #endregion
    }
}