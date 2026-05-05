using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    private int _level = 1;
    private float _maxExp = 100f;
    private float _currentExp = 0f;
    private float _expMultiplier = 1.2f;
    public event Action OnGainExp;
    public event Action OnLevelUp;

    public int Level => _level;
    public float ExpRatio => _currentExp / _maxExp;

    public void GainExp(float amount)
    {
        _currentExp += amount;
        OnGainExp?.Invoke();
        if (_currentExp > _maxExp)
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        _level++;
        _currentExp -= _maxExp;
        _maxExp *= _expMultiplier;
        OnLevelUp?.Invoke();

        if(_currentExp >= _maxExp)
        {
            LevelUp();
        }
    }
}
