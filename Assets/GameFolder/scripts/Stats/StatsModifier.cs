using UnityEngine;

public class StatsModifier
{
    private float value;
    private StatModality statType;
    private object source;
    private int oder;

    public StatsModifier(float value, StatModality statType)
    {
        this.value = value;
        this.statType = statType;
    }

    public StatsModifier(float value, StatModality statType, object source)
        : this(value, statType)
    {
        this.source = source;
    }

    public StatsModifier(float value, StatModality statType, object source, int order)
       : this(value, statType, source)
    {
        this.oder = order;
    }


    #region Properties
    public float Value { get { return value; } }
    public StatModality StatType { get { return statType; } }
    public object Source { get { return source; } }
    public int Order { get { return Order; } }
    #endregion
}