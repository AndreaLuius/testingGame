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
        if (other.tag.Equals("rightCollider")
            || other.tag.Equals("leftCollider")
            || other.tag.Equals("backCollider"))
        {
            int currentAttackType = animator.GetInteger(AnimatorAshesh.attackType);

            if (!animator.GetBool(AnimatorAshesh.controlTimerHandler)
                && currentAttackType > 0)
            {
                animator.SetBool(AnimatorAshesh.controlTimerHandler, true);

                var prop = other.transform.GetComponentInParent<Health>();
                var property = other.GetComponentInParent<EnemyProperties>();

                switch (currentAttackType)
                {
                    case 1:
                        prop.takeDamage2(properties.Attack_power.Value, property, other.tag);
                        break;
                    case 2:
                        prop.takeDamage2(properties.Attack_power.Value * 2, property, other.tag);
                        break;
                }
            }
        }
    }
    #endregion


}
