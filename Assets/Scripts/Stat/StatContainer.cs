using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;

public class StatContainer
{
    private float baseValue;
    private List<StatModifier> modifiers = new List<StatModifier>();
    private float cachedValue;
    private bool isDirty = false;

    public event Action<float> OnValueChanged;

    public StatContainer(float value)
    {
        this.baseValue = value;
        this.cachedValue = value;
        isDirty = false;
    }
    
    public float FinalValue
    {
        get
        {
            if (isDirty)
            {
                cachedValue = ReCalculate();
                isDirty = false;
            }
            return cachedValue;
        }
    }

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        isDirty = true;
        OnValueChanged?.Invoke(FinalValue);        
    }

    public void RemoveBySource(object source)
    {
        modifiers.RemoveAll(m => m.source == source);
        isDirty = true;
        OnValueChanged?.Invoke(FinalValue);
    }

    private float ReCalculate()
    {
        float finalValue = baseValue;

        var flatValues = modifiers.Where(m => m.type == ModType.Flat).ToList();
        var defaultValues = modifiers.Where(m => m.type == ModType.Percent && m.category == ModCategory.Default).ToList();
        var mulValues = modifiers.Where(m => m.type == ModType.Percent && m.category == ModCategory.Multiply).ToList();

        foreach (var stat in flatValues)
        {
            finalValue += stat.value;
        }

        float percent = 0f;
        foreach (var stat in defaultValues)
        {
            percent += stat.value;
        }
        finalValue *= (1f + percent);

        foreach (var stat in mulValues)
        {
            finalValue *= (1f + stat.value);
        }

        return finalValue;
    }
}
