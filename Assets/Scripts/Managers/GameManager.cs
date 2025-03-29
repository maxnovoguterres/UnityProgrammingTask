using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Runtime Variables")]
    [ReadOnly] public GameplayUIManager GameplayUIManager;
    [ReadOnly] public Entity PlayerEntity;
    [ReadOnly] public CameraController CameraController;
    [ReadOnly] public List<Entity> AllEntities = new List<Entity>();
    
    private void Awake()
    {
        GameManagerUtils.SetGameMamanger(this);
        if (QualitySettings.vSyncCount > 0)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = -1;
        }
        
        PlayerEntity = FindAnyObjectByType<PlayerCharacterController>().GetComponent<Entity>();
        CameraController = FindAnyObjectByType<CameraController>();
        CameraController?.Setup(PlayerEntity);
        GameplayUIManager = FindAnyObjectByType<GameplayUIManager>();
        GameplayUIManager?.Setup(PlayerEntity);
        GC.Collect();
    }
    
    public void RegisterEntity(Entity entity)
    {
        AllEntities.Add(entity);
    }
    
    public void DeregisterEntity(Entity entity)
    {
        var allEntitiesIndex = AllEntities.IndexOf(entity);
        if (allEntitiesIndex != -1)
        {
            AllEntities.RemoveAt(allEntitiesIndex);
        }
    }
}