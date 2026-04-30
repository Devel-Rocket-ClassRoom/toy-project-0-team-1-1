using UnityEngine;
using System.Collections.Generic;

public class StatTest : MonoBehaviour
{
    public Dictionary<string, StatContainer> stats = new Dictionary<string, StatContainer>();
    void Awake()
    {
        stats[StatName.Health] = new StatContainer(1000);
        stats[StatName.Attack] = new StatContainer(300);
        stats[StatName.Defense] = new StatContainer(50);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var healthModifier1 = new StatModifier
            {
                type = ModType.Flat,
                category = ModCategory.Default,
                value = 300f,
                source = this
            };
            var healthModifier2 = new StatModifier
            {
                type = ModType.Percent,
                category = ModCategory.Default,
                value = 0.1f,
                source = this
            };
            var healthModifier3 = new StatModifier
            {
                type = ModType.Percent,
                category = ModCategory.Default,
                value = 0.15f,
                source = this
            };
            stats[StatName.Health].AddModifier(healthModifier1);
            stats[StatName.Health].AddModifier(healthModifier2);
            stats[StatName.Health].AddModifier(healthModifier3);

            Debug.Log(stats[StatName.Health].FinalValue);
        }
    }
}
