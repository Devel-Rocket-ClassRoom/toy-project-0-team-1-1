using System;
using System.Collections.Generic;
using System.Linq;

public class StatContainer
{
    private float baseValue;  // 기초값
    private List<StatModifier> modifiers = new List<StatModifier>();  // 모디파이어 리스트
    private float cachedValue;  // 최종값 저장
    private bool isDirty = false;  // 모디파이어 변경 플래그

    public event Action<float> OnValueChanged;  // 스탯 변경 이벤트

    public StatContainer(float value)  // 초기화
    {
        this.baseValue = value;
        this.cachedValue = value;
        isDirty = false;
    }
    
    public float FinalValue // 최종값 계산
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

    public void AddModifier(StatModifier mod)  // 모디파이어 추가
    {
        modifiers.Add(mod);
        isDirty = true;
        OnValueChanged?.Invoke(FinalValue);        
    }

    public void RemoveBySource(object source)  // 모디파이어 제거
    {
        modifiers.RemoveAll(m => m.source == source);
        isDirty = true;
        OnValueChanged?.Invoke(FinalValue);
    }

    private float ReCalculate()  // 스탯 계산
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
