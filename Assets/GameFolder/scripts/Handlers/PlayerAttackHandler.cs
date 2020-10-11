using Properties;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour,AttackProvider
{
    private Animator animator;
    private PlayerProperties properties;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        properties = GetComponentInParent<PlayerProperties>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        hitController(other);
    }

    #region InterfaceImpl
    public void hitController(Collider other)
    {
        int currentAttackType = animator.GetInteger(AnimatorAshesh.attackType);

        if (other.tag.Equals("Enemy") && currentAttackType > 0)
        {
            var prop = other.transform.GetComponent<Health>();
            var property = other.GetComponentInParent<EnemyProperties>();

            switch (currentAttackType)
            {
                case 1:
                    prop.takeDamage(properties.Attack_power.Value,property);
                    break;
                case 2:
                    prop.takeDamage(properties.Attack_power.Value * 2,property);
                    break;
            }
        }
    }
    #endregion
}
