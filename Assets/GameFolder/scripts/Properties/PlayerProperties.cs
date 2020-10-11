using UnityEngine;

public class PlayerProperties : BaseProperties
{
    private GeneralStat intelligence = new GeneralStat(5);

    private void Start()
    {
        defence = new GeneralStat(10);
        Attack_power = new GeneralStat(908);
    }
    
    #region Properties
    public GeneralStat Intelligence  {
        get { return intelligence; }
    }
    public GeneralStat Dexerity { get; } = new GeneralStat(8);
    public GeneralStat Regeneration { get; } = new GeneralStat(0);

    #endregion

}
