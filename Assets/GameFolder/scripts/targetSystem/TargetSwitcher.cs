using UnityEngine;
using Cinemachine;

namespace TargetSystem
{
    public class TargetSwitcher : MonoBehaviour, TargetUtilities
    {
        [SerializeField] Transform whereToLook;
        [SerializeField] CinemachineTargetGroup targetGroup;
        [SerializeField] float distance = 30f;
        [SerializeField] float lerpTime = 1f;
        [SerializeField] Animator animator;
        [SerializeField] float maxTargetDistance = 10f;
        private Vector3 endRight, endLeft;
        private float currentLerpTime = 0f;
        private bool found = true;
        private float viewSwitchAngle = 100f;
        private int switchPoint = 0;

        private void Update()
        {
            switchTarget();
            lookThisPoisition();
        }

        private void lookThisPoisition()
        {
            if (switchPoint == 0)
            {
                this.transform.localPosition =
                new Vector3(whereToLook.position.x, 1.02f, whereToLook.position.z);

                this.transform.rotation = Quaternion.Euler(0f, whereToLook.rotation.eulerAngles.y, 0f);

            }
        }

        private void switchTarget()
        {
            if (!animator.GetBool("isTargetLocked")) return;

            inputKeyManager();

            switch (switchPoint)
            {
                case 1:
                    lerping(endLeft);
                    break;
                case 2:
                    lerping(endRight);
                    break;
            }

            endingLerp();
        }

        private void OnTriggerEnter(Collider other)
        {
            findSwitchTarget(other);
        }

        private void inputKeyManager()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                endLeft = transform.localPosition + (-transform.right) * distance;
                switchPoint = 1;
                found = false;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                endRight = transform.localPosition + (transform.right) * distance;
                switchPoint = 2;
                found = false;
            }
        }

        private void findSwitchTarget(Collider other)
        {
            if (animator.GetBool("isTargetLocked") && other.tag.Equals("Enemy")
                    && playerViewAngle(other) <= viewSwitchAngle && !found)
            {
                targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
                targetGroup.AddMember(other.transform, 100, 1);
                found = true;
            }
        }

        private void lerping(Vector3 endPos)
        {
            currentLerpTime += Time.deltaTime;
            float timer = currentLerpTime / lerpTime;

            transform.position =
                Vector3.Lerp(this.transform.position, endPos, timer);
        }

        private void endingLerp()
        {
            if (transform.position == endLeft || transform.position == endRight)
            {
                switchPoint = 0;
                currentLerpTime = 0f;
            }
        }

        #region InterfaceImplementation
        public float playerViewAngle(Collider other)
        {
            Vector3 targetDirection = other.transform.position - this.transform.position;

            return Vector3.Angle(targetDirection, transform.forward);
        }
        #endregion
    }
}
