using UnityEngine;

public static class Global
{
    public static InventoryCollectionSO InventoryCollection;

    public static void Setup()
    {
        InventoryCollection = Resources.Load<InventoryCollectionSO>(path:"Configs/InventoryCollection");
    }
}