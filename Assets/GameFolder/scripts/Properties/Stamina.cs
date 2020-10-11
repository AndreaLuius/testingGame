using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Properties
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] Slider manaBar;
        [SerializeField] float stopRegTime = 1.5f;
        [SerializeField] float lightAttackStamina = 12f;
        [SerializeField] float heavyAttackStamina = 20f;
        [SerializeField] float rollStamina = 11f;
        private PlayerProperties playerProperties;
        private bool isHandlingStarted = false;
        private float staminaMax = 100f;
        private float minStamina = 0f;
        private float regPerSec = .16f;
        private Animator _animator;
        private bool isRegPoss;
        private bool isStaminaOut;
        

        private void Start()
        {
            playerProperties = GetComponent<PlayerProperties>();
            _animator = GetComponent<Animator>();
            isRegPoss = true;
        }

        private void Update()
        {
            if (dynamicStaminaStop() && IsRegPoss)
                regenerating(playerProperties.Regeneration);

            staminaDetector(animator);

            tiringHandler();
        }
        
        /*
            When The player is executing an attack 
            or a roll the stamina will stop regenarate 
            till the executon is not over
        */
        private bool dynamicStaminaStop()
        {
            return (!animator.GetBool(AnimatorAshesh.isAttacking) &&
                    !animator.GetBool(AnimatorAshesh.isCurrentlyRolling));
        }

        /*
        Consumes the amount of specified stamina
        and decreases the stamina of the specified amount
        */
        public void consuming(float amount)
        {
            if (manaBar.value >= .107f)
                manaBar.value -= (amount / 100);
            else
                StartCoroutine(tiringController(stopRegTime));  
        }

        /*
        Check if the player finish the stamina
        and if he does,the stamina will stop regenerating
        for 1.5 seconds
        */
        private IEnumerator tiringController(float stopTime)
        {
            manaBar.value = 0;

            isStaminaOut = true;
            isRegPoss = false;

            yield return new WaitForSeconds(stopTime);

            isRegPoss = true;
        }

        /*
        Regenerates the stamina over time based
        on the stat of the given Agent
        */
        public void regenerating(GeneralStat stat)
        {
            if (manaBar.value <= staminaMax)
                manaBar.value += (regPerSec + stat.Value) * Time.deltaTime;
        }

        /*
        Filter the Stamina usage dependently by
        the attack that the player is going to execute
        */
        public void staminaDetector(Animator animator)
        {
            attackStaminaDetector();
            rollStaminaDetector();
        }

        /*
        This method controls if the stamina reached the end
        and if it did stops the possibility to attack till 
        it reaches the 25% of the staminaBar 
        */
        public void tiringHandler()
        {
            if (manaBar.value < .25f && isStaminaOut)
                AnimatorAshesh.boolAnimatorToggler(animator, false, AnimatorAshesh.canAttack, AnimatorAshesh.isRollEnabled);
            else
            {
                AnimatorAshesh.boolAnimatorToggler(animator, true, AnimatorAshesh.canAttack, AnimatorAshesh.isRollEnabled);
                isStaminaOut = false;
            }
        }

        /*
        Checks if an attack got called and 
        consume the usage of the stamina 
        resetting the staminaSucker

        @params: StaminaSucker will be flagged to true
                only if an attack enters the state
         */
        private void staminaController(float amount)
        {
            if (_animator.GetBool(AnimatorAshesh.staminaSucker))
            {
                consuming(amount);
               _animator.SetBool(AnimatorAshesh.staminaSucker, false);
            }
        }
           
        /*
        Detects the stamina usage for the roll*/
        private void rollStaminaDetector()
        {
            if (animator.GetBool(AnimatorAshesh.isRollEnabled))
                staminaController(rollStamina);
        }

        /*
        Detects the usage of stamina of
        light and heavy attack*/
        private void attackStaminaDetector()
        {
            switch (_animator.GetInteger(AnimatorAshesh.attackType))
            {
                case 1:
                    staminaController(lightAttackStamina);
                    break;
                case 2:
                    staminaController(heavyAttackStamina);
                    break;
            }
        }

        #region Properties
        public Animator animator { get { return _animator; } }
        public Slider ManaBar { get { return manaBar; } }
        public bool IsStaminaOut { get { return isStaminaOut; } set { isStaminaOut = value; } }
        public bool IsRegPoss { get { return isRegPoss; } set { isRegPoss = value; } }
        #endregion
    }
}