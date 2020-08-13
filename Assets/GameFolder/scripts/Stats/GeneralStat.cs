using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class GeneralStat
{
    private float baseValue;
    private bool isModified;
    private float lastBaseValue = float.MinValue;
    private float value;
    private List<StatsModifier> stats;

    public GeneralStat(float baseValue)
    {
        this.baseValue = baseValue;
        stats = new List<StatsModifier>();
    }

    public void addModifier(StatsModifier stat)
    {
        stats.Add(stat);
        stats.Sort(sortingContrller);
        isModified = true;
    }

    public void removeModifier(StatsModifier stat)
    {
        stats.Remove(stat);
        isModified = true;
    }

    private float finalValueCalculator()
    {
        float finalValue = baseValue;
        float sumPercentAdd = 0f;

        for (int i = 0; i < stats.Count; i++)
        {
            StatsModifier modifier = stats[i];

            switch (modifier.StatType)
            {
                case StatModality.Flat:
                    finalValue += modifier.Value;
                    break;
                case StatModality.PercentAdd:
                    sumPercentAdd += modifier.Value;
                    if ((i + 1) >= stats.Count ||
                        stats[i + 1].StatType != StatModality.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                    break;
                case StatModality.PercentMulti:
                    finalValue *= 1 + modifier.Value;
                    break;
            }
        }
        return (float)Math.Round(finalValue, 4);
    }

    public void removeAllModifiers(object source)
    {
        for (int i = (stats.Count - 1); i >= 0; i--)
        {
            if (stats[i].Source == source)
            {
                stats.RemoveAt(i);
                isModified = true;
            }
        }
    }

    public int sortingContrller(StatsModifier a, StatsModifier b)
    {
        if (a.Order > b.Order)
            return 1;
        else if (a.Order < b.Order)
            return -1;
        else return 0;
    }

    #region Properties
    public float Value
    {
        get
        {
            if (isModified || lastBaseValue != baseValue)
            {
                lastBaseValue = baseValue;
                value = finalValueCalculator();
                isModified = false;
            }
            return value;
        }
    }
    #endregion
}