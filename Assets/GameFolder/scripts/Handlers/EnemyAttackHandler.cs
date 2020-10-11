using Properties;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    private Animator animator;
    private EnemyProperties properties;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        properties = FindObjectOfType<EnemyProperties>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        hitController(other);
    }

    #region InterfaceImpl
    public void hitController(Collider other)
    {
        int currentAttackType = animator.GetInteger(AnimatorAshesh.attackType);

        if (other.tag.Equals("damager") && currentAttackType > 0)
        {
            var prop = other.transform.GetComponent<Health>();
            var property = other.GetComponentInParent<PlayerProperties>();

            switch (currentAttackType)
            {
                case 1:
                    prop.takeDamage(properties.Attack_power.Value,property);
                    break;
                case 2:
                    prop.takeDamage(properties.Attack_power.Value * 2,property);
                    break;
                case 3:
                    prop.takeDamage(properties.Attack_power.Value * 3,property);
                    break;
                case 4:
                    prop.takeDamage(properties.Attack_power.Value * 5,property);
                    break;
            }
        }
    }
    #endregion

}
