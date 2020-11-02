using System.Collections;
using UnityEngine;

namespace ControllerInputs
{ 
    public class InvincibilityRoll : MonoBehaviour
    {
        [SerializeField] float invTime = 1.3f;
        private float pushAmount;
        private Collider collider;
        private Animator animator;

        private void Start()
        {
            animator = GetComponentInParent<Animator>();
            collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (animator.GetBool(AnimatorAshesh.isTargetLocked))
            {
                if (Input.GetButtonDown("GamepadRoll") && Input.GetAxis("Vertical") < -0.6f)//back
                    rollController(1);
                else if (Input.GetButtonDown("GamepadRoll") && Input.GetAxis("Vertical") > 0.6f)//front
                    rollController(0);
                else if (Input.GetButtonDown("GamepadRoll") && Input.GetAxis("Horizontal") > -.6f)//right
                    rollController(2);
                else if (Input.GetButtonDown("GamepadRoll") && Input.GetAxis("Horizontal") < -.6f)//left
                    rollController(3);
            }
            else
                if (Input.GetButtonDown("GamepadRoll"))
                    rollController(0);
        }

        /*
        Deactivates the collider for the given 
        amount of time and reactivates it when 
        the time is over
        */
        private IEnumerator roll(float inv_time)
        {
            colliderSwitcher(false);

            animator.SetTrigger(AnimatorAshesh.isRolling);

            yield return new WaitForSeconds(inv_time);

            colliderSwitcher(true);
        }
        /*
        Allows to dynamically choose what
        roll you want to execute
        */
        private void rollController(int rollType)
        {
            animator.SetBool(AnimatorAshesh.isRollEnabled, true);
            animator.SetInteger(AnimatorAshesh.rollType, rollType);
            StartCoroutine(roll(invTime));
        }

        /*
        Turn on and off the collider,dependently
        by the given boolean
        */
        private void colliderSwitcher(bool isEnabled)
        {
            collider.enabled = isEnabled;
        }
    }
}











