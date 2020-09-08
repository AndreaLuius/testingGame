using UnityEngine;
using Properties;

public class PlayerProperties : Health
{
    private GeneralStat defence = new GeneralStat(40);
    private GeneralStat attack_power = new GeneralStat(50);
    private GeneralStat intelligence = new GeneralStat(5);
    private GeneralStat dexerity = new GeneralStat(8);
    private GeneralStat regeneration = new GeneralStat(0);
    private Stamina stamina;

    public void Start()
    {
        stamina = GetComponent<Stamina>();
    }

    private void Update()
    {
        if (dynamicStaminaStop() && stamina.IsRegPoss)
            stamina.regenerating(regeneration);

        stamina.staminaDetector(stamina.animator);

        stamina.tiringHandler();
    }

    override public void takeDamage(float value)
    {
        if (HealthBar.value > 0)
        {
            HealthBar.value -= (value / defence.Value);
            HealthBar.value =
                Mathf.Clamp(HealthBar.value, HealthBar.minValue, HealthBar.maxValue);

            IsHit = true;
        }
        else
            die();
    }

    override public void die()
    {
        //TODO: implements method
    }

    /*
    When The player is executing an attack 
    or a roll the stamina will stop regenarate 
    till the executon is not over
    */
    private bool dynamicStaminaStop()
    {
        return (!stamina.animator.GetBool(AnimatorAshesh.isAttacking) &&
                !stamina.animator.GetBool(AnimatorAshesh.isCurrentlyRolling));
    }
}