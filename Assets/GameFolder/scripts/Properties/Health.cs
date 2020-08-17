using UnityEngine;
using UnityEngine.UI;
using System;

namespace Properties
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] int maxHealth = 100;
        [SerializeField] int minHealt = 0;
        private bool isHit;

        public virtual void takeDamage(float value)
        {//in case just use a delegate
         //so that it gets a little more dinamyc
            if (healthBar.value > 0)
            {
                healthBar.value -= (value);
                healthBar.value = Mathf.Clamp(healthBar.value, healthBar.minValue, healthBar.maxValue);
            }
            else
                die();
        }

        public virtual void die()
        {
            //TODO: implements method
        }

        #region Properties
        public Slider HealthBar { get { return healthBar; } }
        public int MaxHealth { get { return maxHealth; } }
        public int MinHealth { get { return minHealt; } }
        public bool IsHit { get { return isHit; } set { isHit = value; } }
        #endregion
    }
}