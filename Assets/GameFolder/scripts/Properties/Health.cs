using UnityEngine;
using UnityEngine.UI;

namespace Properties
{
    public class Health : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] int maxHealth = 100;
        [SerializeField] int minHealt = 0;
        private Animator animator;
        public float currentHealth;

        /*TODO:
         *1)finish the hit damage and die system/animation
         *2)make the player attack movement ifk check on darksouls to make it a little closer to it
         *3)check the optimization of everything,
          4)check th AI'S.*/
        private void Start()
        {
            currentHealth = maxHealth;
            animator = GetComponentInParent<Animator>();
        }

        public void takeDamage(float value,BaseProperties property)
        {
            if (currentHealth > 0)
            {
                animator.SetTrigger("hitRight");

                currentHealth -= (value / property.Defence.Value);
                healthBar.value = currentHealth ;
                HealthBar.value =
                            Mathf.Clamp(HealthBar.value, HealthBar.minValue, HealthBar.maxValue);
            }
            else
                die();
        }

        public void takeDamage2(float value, BaseProperties property,string direction)
        {
            if (currentHealth > 0)
            {
                switch(direction)
                {
                    //todo:check this 
                    case "rightCollider":
                        animator.SetBool(AnimatorAshesh.hitRight, false);
                        animator.SetBool(AnimatorAshesh.hitRight,true);
                        break;
                    case "leftCollider":
                        animator.SetBool(AnimatorAshesh.hitLeft, false);
                        animator.SetBool(AnimatorAshesh.hitLeft,true);
                        break;
                    case "centerCollider":
                        animator.SetBool(AnimatorAshesh.hitCenter, false);
                        animator.SetBool(AnimatorAshesh.hitCenter,true);
                        break;
                    case "backCollider":
                        animator.SetBool(AnimatorAshesh.hitBack, false);
                        animator.SetBool(AnimatorAshesh.hitBack,true);
                        break;
                }

                currentHealth -= (value / property.Defence.Value);
                healthBar.value = currentHealth;
                HealthBar.value =
                            Mathf.Clamp(HealthBar.value, HealthBar.minValue, HealthBar.maxValue);
            }
            else
                die();
        }

        public void die()
        {
            //TODO: put some particle
            animator.SetBool("isDead2",true);
            Destroy(transform.parent.gameObject,2f);
        }

        #region Properties
        public Slider HealthBar { get { return healthBar; } }
        public int MaxHealth { get { return maxHealth; } }
        public int MinHealth { get { return minHealt; } }
        #endregion
    }
}