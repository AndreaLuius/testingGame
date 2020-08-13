using UnityEngine;
using UnityEngine.UI;

namespace Properties
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] int maxHealth = 100;
        [SerializeField] int minHealt = 0;
        public bool isHit;


        public virtual bool takeDamage(float value)
        {//in case just use a delegate
         //so that it gets a little more dinamyc
            if (healthBar.value > 0)
            {
                healthBar.value -= (value);
                healthBar.value = Mathf.Clamp(healthBar.value, healthBar.minValue, healthBar.maxValue);
                isHit = true;
            }
            else
                die();

            return isHit;
        }

        public virtual void die()
        {
            //TODO: implements method
        }

        #region Properties
        public Slider HealthBar { get { return healthBar; } }
        public int MaxHealth { get { return maxHealth; } }
        public int MinHealth { get { return minHealt; } }
        #endregion
    }
}