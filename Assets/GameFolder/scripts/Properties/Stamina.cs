using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Properties
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] Slider manaBar;
        private bool isHandlingStarted = false;
        protected float manaMax = 100f;
        protected float minMaxa = 0;
        protected float regPerSec = .2f;
        public Animator animator;

        private void Start()
        {
            animator = GetComponentInParent<Animator>();
        }

        public void consuming(float amount)
        {
            manaBar.value -= amount * Time.deltaTime;
        }

        public void regenerating(GeneralStat stat)
        {
            if (manaBar.value <= manaMax)
                manaBar.value += (regPerSec + stat.Value) * Time.deltaTime;
        }

        public void staminaController(Animator animator, float amount)
        {
            if (animator.GetBool(AnimatorAshesh.staminaSucker))
            {
                consuming(amount);
                animator.SetBool(AnimatorAshesh.staminaSucker, false);
            }
        }
    }
}