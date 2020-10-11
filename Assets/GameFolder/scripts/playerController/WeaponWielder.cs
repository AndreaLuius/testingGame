using UnityEngine;

public class WeaponWielder : MonoBehaviour
{
    [SerializeField] Transform sword, ueqSword, eqSword;
    private Animator animator;
    public bool isEquipped;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isEquipped)
        {
            sword.position = ueqSword.position;
            sword.rotation = ueqSword.rotation;
        }
        else
        {
            sword.position = eqSword.position;
            sword.rotation = eqSword.rotation;
        }
    }

    public void swordEquipping()
    {
        isEquipped = true;
    }

    public void swordUnequip()
    {
        isEquipped = false;
    }
}
