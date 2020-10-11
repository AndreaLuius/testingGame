using UnityEngine;

    public class EnemyProperties : BaseProperties
    {
        private void Start()
        {
            defence = new GeneralStat(6);
            Attack_power = new GeneralStat(183);
        }
    }


