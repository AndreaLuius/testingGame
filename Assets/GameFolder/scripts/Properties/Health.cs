using UnityEngine;
using UnityEngine.UI;

namespace Properties
{
    public class Health : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] int maxHealth = 100;
        [SerializeField] int minHealt = 0;
        private bool isHit;
        
        public float currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }
        
        public void takeDamage(float value,BaseProperties property)
        {
            if (currentHealth > 0)
            {
                currentHealth -= (value / property.Defence.Value);
                healthBar.value = currentHealth ;
                HealthBar.value =
                            Mathf.Clamp(HealthBar.value, HealthBar.minValue, HealthBar.maxValue);
            }
            else
                die();
        }

        public void die()
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