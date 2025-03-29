using Sirenix.OdinInspector;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Runtime Variables")]
    [ReadOnly] public float CurrentHealth;
    [ReadOnly] public bool IsDead;
    
    private Entity _entity;
    private bool _isSetup;

    public void Setup(Entity entity)
    {
        if (!_isSetup)
        {
            _entity = entity;
        }
        
        CurrentHealth = _entity.StatsComponent.Stats.MaximumHealth;
        IsDead = false;
        _isSetup = true;
    }
}