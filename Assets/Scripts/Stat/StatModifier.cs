using UnityEngine;

public enum ModType
{
    Flat = 0, // 기초 스탯에 더함
    Percent   // Flat 결과에 곱함
}

public enum ModCategory
{
    Default = 0, // 합연산
    Multiply     // 무조건 곱연산
}

public struct StatModifier<T>
{
    ModType type;
    ModCategory category;
    T value;        // 스탯 증감량
    object source;  // 스탯을 제공한 주체
}
