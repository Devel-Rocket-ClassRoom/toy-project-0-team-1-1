using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Dictionary<string, StatContainer> stats = new Dictionary<string, StatContainer>();
    public event Action<float> OnHpChange;
    private Animator _animator;
    private float _currentHp;
    public bool IsDead => _currentHp <= 0;

    private void Awake()
    {
        stats[StatName.Health] = new StatContainer(100f);
        stats[StatName.Defense] = new StatContainer(10f);
        _currentHp = stats[StatName.Health].FinalValue;
        _animator = gameObject.GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        float defense = stats[StatName.Defense].FinalValue;
        float finalDamage = Mathf.Max(0, damage - defense);
        _currentHp -= finalDamage;
        OnHpChange?.Invoke(_currentHp);
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        _currentHp = Mathf.Min(_currentHp + amount, stats[StatName.Health].FinalValue);
        OnHpChange?.Invoke(_currentHp);
    }

    private void Die()
    {
        _animator.SetTrigger("Die");
    }
}
