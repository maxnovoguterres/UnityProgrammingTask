using System.Collections.Generic;

public static class GameManagerUtils
{
    public static GameManager GameManager;

    public static void SetGameMamanger(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    
    public static CameraController CameraController
    {
        get { return GameManager.CameraController; }
    }
    
    public static GameplayUIManager GameplayUIManager
    {
        get { return GameManager.GameplayUIManager; }
    }
    
    public static List<Entity> AllEntities
    {
        get { return GameManager.AllEntities; }
    }
    
    public static void RegisterEntity(Entity entity)
    {
        GameManager.RegisterEntity(entity);
    }

    public static void DeregisterEntity(Entity entity)
    {
        GameManager.DeregisterEntity(entity);
    }
}