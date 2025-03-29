using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool IsPlayer;
    public bool ForceRegisterEntity;
    public AnimationComponent AnimationComponent;
    public ModifiedCharacterController CharacterController;
    public HealthComponent HealthComponent;
    public InteractableComponent InteractableComponent;
    public InteractionComponent InteractionComponent;
    public InventoryComponent InventoryComponent;
    public LootComponent LootComponent;
    public StatsComponent StatsComponent;
    public EquipmentComponent EquipmentComponent;
    public DialogueComponent DialogueComponent;
    
    private bool _hasBeenSetUp;

    public void Reset()
    {
        GetComponents();
    }

    [ContextMenu("Get Entity Components")]
    public void GetComponents()
    {
        AnimationComponent = GetComponent<AnimationComponent>();
        CharacterController = GetComponent<ModifiedCharacterController>();
        HealthComponent = GetComponent<HealthComponent>();
        InteractableComponent = GetComponent<InteractableComponent>();
        InteractionComponent = GetComponent<InteractionComponent>();
        InventoryComponent = GetComponent<InventoryComponent>();
        LootComponent = GetComponent<LootComponent>();
        StatsComponent = GetComponent<StatsComponent>();
        EquipmentComponent = GetComponent<EquipmentComponent>();
        DialogueComponent = GetComponent<DialogueComponent>();
    }

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        if (_hasBeenSetUp)
        {
            return;
        }

        if (StatsComponent != null)
        {
            StatsComponent.Setup(this);
        }

        if (HealthComponent != null)
        {
            HealthComponent.Setup(this);
        }

        if (CharacterController != null)
        {
            CharacterController.Setup(this);
        }
        
        var shouldRegisterEntity = ForceRegisterEntity;
        if (shouldRegisterEntity)
        {
            GameManagerUtils.RegisterEntity(this);
        }
        
        _hasBeenSetUp = true;
    }

    public void Recycle()
    {
        var shouldDeregisterEntity = ForceRegisterEntity;
        if (shouldDeregisterEntity)
        {
            GameManagerUtils.DeregisterEntity(this);
        }
        
        _hasBeenSetUp = false;
    }
}