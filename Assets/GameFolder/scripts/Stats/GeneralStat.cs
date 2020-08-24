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


    /**
    Adds a modifier to the stats list
    */
    public void addModifier(StatsModifier stat)
    {
        stats.Add(stat);
        stats.Sort(sortingController);
        isModified = true;
    }

    /**
    Remove a modifier from the stats list
    */
    public void removeModifier(StatsModifier stat)
    {
        stats.Remove(stat);
        isModified = true;
    }

    /**
    Calculates the stat value to increase/decrease
    base on the specified StatType, it is gonna
    execute through every items of the stats list
    */
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

    /**
    Removes all the modifiers from the list
    */
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

    /**
    Sorts the stats list based on
    the Order value
    */
    public int sortingController(StatsModifier a, StatsModifier b)
    {
        if (a.Order > b.Order)
            return 1;
        else if (a.Order < b.Order)
            return -1;
        else return 0;
    }

    #region Properties
    /**
    Retrieve the baseValue checking
    if it was modified or if it is
    different from the last value*/
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