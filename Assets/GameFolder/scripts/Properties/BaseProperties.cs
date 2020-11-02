using UnityEngine;

public class BaseProperties : MonoBehaviour
{
    protected Animator animator;
    public GeneralStat defence;
    private GeneralStat attack_power;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    #region Properties
    public GeneralStat Defence { get { return defence;  } }
    
    public GeneralStat Attack_power
    {
        get { return attack_power; }
        set { attack_power = value; }
    }
    
    #endregion

}
