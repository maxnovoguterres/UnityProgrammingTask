using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct Stats
{
    public float MaximumHealth;
    public float Damage;
    public float MoveSpeed;
    [InfoBox("0 is 0% and 1 is 100%")]
    [Range(0, 1)] public float Armor;
    [InfoBox("0 is 0% and 1 is 100%")]
    [Range(0, 1)] public float CriticalChance;
    [InfoBox("0 is 0% and 1 is 100%")]
    public float CriticalDamage;
    [Header("Modifiers")]
    [InfoBox("0 is 0% and 1 is 100%")]
    public float DamageModifier;
    [InfoBox("0 is 0% and 1 is 100%")]
    public float MoveSpeedModifier;
}

public class StatsComponent : MonoBehaviour
{
    [ChangePainter] public Stats Stats;
    
    private Entity _entity;

    public void Setup(Entity entity)
    {
        _entity = entity;
    }
    
    public void AddStats(Stats statsToAdd)
    {
        if (_entity.HealthComponent != null)
        {
            var currentHealthPercent = GetCurrentHealthPercent();
            Stats.MaximumHealth += statsToAdd.MaximumHealth;
            _entity.HealthComponent.CurrentHealth = Stats.MaximumHealth * currentHealthPercent;
        }
        
        Stats.Damage += statsToAdd.Damage;
        Stats.MoveSpeed += statsToAdd.MoveSpeed;
        Stats.Armor += statsToAdd.Armor;
        Stats.CriticalChance += statsToAdd.CriticalChance;
        Stats.CriticalDamage += statsToAdd.CriticalDamage;
        Stats.DamageModifier += statsToAdd.DamageModifier;
        Stats.MoveSpeedModifier += statsToAdd.MoveSpeedModifier;
    }

    public void RemoveStats(Stats statsToRemove)
    {
        if (_entity.HealthComponent != null)
        {
            var currentHealthPercent = GetCurrentHealthPercent();
            Stats.MaximumHealth -= statsToRemove.MaximumHealth;
            _entity.HealthComponent.CurrentHealth = Stats.MaximumHealth * currentHealthPercent;
        }
        
        Stats.Damage -= statsToRemove.Damage;
        Stats.MoveSpeed -= statsToRemove.MoveSpeed;
        Stats.Armor -= statsToRemove.Armor;
        Stats.CriticalChance -= statsToRemove.CriticalChance;
        Stats.CriticalDamage -= statsToRemove.CriticalDamage;
        Stats.DamageModifier -= statsToRemove.DamageModifier;
        Stats.MoveSpeedModifier -= statsToRemove.MoveSpeedModifier;
    }

    public float GetCurrentHealthPercent()
    {
        return _entity.HealthComponent.CurrentHealth / Stats.MaximumHealth;
    }
}