using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Properties
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] Slider manaBar;
        [SerializeField] float stopRegTime = 1.5f;
        private bool isHandlingStarted = false;
        private float staminaMax = 100f;
        private float minStamina = 0f;
        private float regPerSec = .2f;
        private Animator _animator;
        private bool isRegPoss;
        private bool isStaminaOut;

        private void Start()
        {
            _animator = GetComponentInParent<Animator>();
            isRegPoss = true;
        }

        /**
        Consumes the amount of specified stamina
        and decreases the stamina of the specified amount
        */
        public void consuming(float amount)
        {
            if (manaBar.value >= .1f)
                manaBar.value -= (amount / 100);
            else
                StartCoroutine(tiringController());
        }

        /**
        Check if the player finish the stamina
        and if does,the stamina will stop regenerating
        for 1.5 seconds
        */
        private IEnumerator tiringController()
        {
            manaBar.value = 0;

            isStaminaOut = true;
            isRegPoss = false;

            yield return new WaitForSeconds(stopRegTime);

            isRegPoss = true;
        }

        /**
        Regenerates the stamina over time based
        on the stat of the given Agent
        */
        public void regenerating(GeneralStat stat)
        {
            if (manaBar.value <= staminaMax)
                manaBar.value += (regPerSec + stat.Value) * Time.deltaTime;
        }

        /**
        Filter the Stamina usage dependently by
        the attack that the player is going to execute
        */
        public void staminaDetector(Animator animator)
        {
            switch (_animator.GetInteger(AnimatorAshesh.attackType))
            {
                case 1:
                    staminaController(animator, 12);
                    break;
                case 2:
                    staminaController(animator, 20);
                    break;
            }
        }

        /**
        This method controls if the stamina reached the end
        and if it did stops the possibility to attack till 
        it reaches the 25% of the staminaBar 
        */
        public void tiringHandler()
        {
            if (manaBar.value < .25f && isStaminaOut)
                animator.SetBool(AnimatorAshesh.canAttack, false);
            else
            {
                animator.SetBool(AnimatorAshesh.canAttack, true);
                isStaminaOut = false;
            }
        }

        /**
        Checks if an attack got called and 
        consume the usage of the stamina 
        resetting the staminaSucker

        @params: StaminaSucker will be flagged to true
                only if an attack enters the state
         */
        private void staminaController(Animator animator, float amount)
        {
            if (animator.GetBool(AnimatorAshesh.staminaSucker))
            {
                consuming(amount);
                animator.SetBool(AnimatorAshesh.staminaSucker, false);
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