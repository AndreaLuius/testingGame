using Properties;
using UnityEngine;

namespace EnemiesProperties
{
    public class SkeltonEnemyProperty : Health
    {
        private GeneralStat defence = new GeneralStat(5);
        private GeneralStat attack_power = new GeneralStat(84);

        [SerializeField] Animator animator;

        override public void takeDamage(float value)
        {
            if (HealthBar.value > 0)
            {
                HealthBar.value -= (value / defence.Value);
                HealthBar.value = Mathf.Clamp(HealthBar.value, HealthBar.minValue, HealthBar.maxValue);
            }
            else
                die();
        }

        override public void die()
        {
            //TODO: implements method
        }

        private void OnTriggerEnter(Collider other)
        {
            hitController(other);
        }

        private void hitController(Collider other)
        {
            int currentAttackType = animator.GetInteger(AnimatorAshesh.attackType);

            if (other.tag.Equals("damager") && currentAttackType > 0)
            {
                var prop = other.transform.GetComponent<PlayerProperties>();

                switch (currentAttackType)
                {
                    case 1:
                        prop.takeDamage(attack_power.Value);
                        break;
                    case 2:
                        prop.takeDamage(attack_power.Value * 2);
                        break;
                    case 3:
                        prop.takeDamage(attack_power.Value * 3);
                        break;
                    case 4:
                        prop.takeDamage(attack_power.Value * 5);
                        break;
                }
            }
        }
    }
}

