using UnityEngine;
using Properties;

public class PlayerProperties : Health
{
    private GeneralStat defence = new GeneralStat(40);
    private GeneralStat attack_power = new GeneralStat(50);
    private GeneralStat intelligence = new GeneralStat(5);
    private GeneralStat dexerity = new GeneralStat(8);

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
}