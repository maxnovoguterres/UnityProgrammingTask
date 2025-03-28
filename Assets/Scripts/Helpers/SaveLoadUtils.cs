using UnityEngine;

public static class SaveLoadUtils
{
    private static string PersistentDataPath
    {
        get
        {
            var persistentPath = Application.persistentDataPath;
            return persistentPath;
        }
    }
}