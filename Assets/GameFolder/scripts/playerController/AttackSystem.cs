using UnityEngine;

namespace ControllerInputs
{
    public class AttackSystem : MonoBehaviour
    {
        private Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            wieldingWeapon();
            attacking();
        }

        /*
        Checks if the player is allowed to attack
        and if he is, starts the attack process
        */
        private void setAttack(int attackType)
        {
            if (animator.GetBool(AnimatorAshesh.canAttack))
            {
                animator.SetInteger(AnimatorAshesh.attackType, attackType);
                animator.SetTrigger(AnimatorAshesh.attack);
                animator.SetBool(AnimatorAshesh.isAttacking, true);
            }
        }

        private void attacking()
        {
            //TODO: check the heavu button
            if (!animator.GetBool(AnimatorAshesh.arming)) return;

            if (Input.GetButtonDown("GamepadLightAttack"))
                setAttack(1);
            else if (Input.GetAxis("GamepadHeavyAttack") > .9f)
                setAttack(2);
        }

        /*
        Allow the player to Wield a weapon
        and trigger its animation*/
        private void wieldingWeapon()
        {
            if (animator.GetBool(AnimatorAshesh.isTargetLocked)) return;

            if (Input.GetButtonDown("GamepadWeaponWield"))
                animator.SetBool(AnimatorAshesh.arming, !animator.GetBool(AnimatorAshesh.arming));
        }
    }
}