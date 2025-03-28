using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ResourcesUtils
{
    private static readonly Dictionary<int, Stack<GameObject>> _prefabPoolByPrefabId;
    private static readonly Dictionary<int, int> _prefabIdByInstanceId;

    static ResourcesUtils()
    {
        _prefabPoolByPrefabId = new Dictionary<int, Stack<GameObject>>(100);
        _prefabIdByInstanceId = new Dictionary<int, int>(100);
        SceneManager.sceneUnloaded += scene =>
        {
            _prefabPoolByPrefabId.Clear();
            _prefabIdByInstanceId.Clear();
        };
    }

    private static bool TryGetPrefabInstanceOrPrefab(string prefabPath, out int prefabId, out GameObject instance)
    {
        var prefab = Resources.Load<GameObject>(prefabPath);
        return TryGetPrefabInstanceOrPrefab(prefab, out prefabId, out instance);
    }

    private static bool TryGetPrefabInstanceOrPrefab(GameObject prefab, out int prefabId, out GameObject instance)
    {
        if (prefab == null)
        {
            Debug.LogError($"TryGetPrefabInstanceOrPrefab called with null prefab!");
            prefabId = default(int);
            instance = default(GameObject);
            return false;
        }

        prefabId = prefab.GetInstanceID();
        if (!_prefabPoolByPrefabId.TryGetValue(prefabId, out var prefabPool))
        {
            prefabPool = new Stack<GameObject>();
            _prefabPoolByPrefabId[prefabId] = prefabPool;
        }

        if (prefabPool.Count > 0)
        {
            instance = prefabPool.Pop();
            return true;
        }

        instance = prefab;
        return false;
    }

    public static void GetPrefabInstance(string prefabPath, Transform parent, out GameObject instance)
    {
        var prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Tried loading prefab at path [{prefabPath}] but no prefab was found!");
            instance = null;
            return;
        }
        GetPrefabInstance(prefab, parent, out instance);
    }

    public static void GetPrefabInstance(GameObject prefab, Transform parent, out GameObject instance)
    {
        if (!TryGetPrefabInstanceOrPrefab(prefab, out var prefabId, out var prefabOrInstance))
        {
            instance = Object.Instantiate(prefabOrInstance, parent);
        }
        else
        {
            instance = prefabOrInstance;
            var transform = instance.transform;
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            instance.gameObject.SetActive(true);
        }

        _prefabIdByInstanceId[instance.GetInstanceID()] = prefabId;
    }

    public static void GetPrefabInstance(string prefabPath, Vector3 position, Quaternion rotation, Transform parent, out GameObject instance)
    {
        var prefab = Resources.Load<GameObject>(prefabPath);
        GetPrefabInstance(prefab, position, rotation, parent, out instance);
    }

    public static void GetPrefabInstance(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, out GameObject instance)
    {
        if (!TryGetPrefabInstanceOrPrefab(prefab, out var prefabId, out var prefabOrInstance))
        {
            instance = Object.Instantiate(prefabOrInstance, position, rotation, parent);
        }
        else
        {
            instance = prefabOrInstance;
            var transform = instance.transform;
            transform.position = position;
            transform.rotation = rotation;
            transform.parent = parent;
            instance.gameObject.SetActive(true);
        }

        _prefabIdByInstanceId[instance.GetInstanceID()] = prefabId;
    }

    public static void GetPrefabInstance(string prefabPath, Vector3 position, Quaternion rotation, out GameObject instance)
    {
        var prefab = Resources.Load<GameObject>(prefabPath);
        GetPrefabInstance(prefab, position, rotation, out instance);
    }

    public static void GetPrefabInstance(GameObject prefab, Vector3 position, Quaternion rotation, out GameObject instance)
    {
        if (!TryGetPrefabInstanceOrPrefab(prefab, out var prefabId, out var prefabOrInstance))
        {
            instance = Object.Instantiate(prefabOrInstance, position, rotation);
        }
        else
        {
            instance = prefabOrInstance;
            var transform = instance.transform;
            transform.parent = null;
            transform.position = position;
            transform.rotation = rotation;
            instance.gameObject.SetActive(true);
        }

        _prefabIdByInstanceId[instance.GetInstanceID()] = prefabId;
    }

    public static void StorePrefab(GameObject instance)
    {
        var instanceId = instance.GetInstanceID();
        if (!_prefabIdByInstanceId.TryGetValue(instanceId, out var prefabId))
        {
            Debug.LogError($"Trying to store an instance, but couldnt find the PrefabId to store it! Destroying object.");
            Object.Destroy(instance);
            return;
        }

        instance.gameObject.SetActive(false);
        _prefabPoolByPrefabId[prefabId].Push(instance);
        _prefabIdByInstanceId.Remove(instanceId);
    }
}

public static class ResourcesUtils<T> where T : MonoBehaviour
{
    public static Dictionary<int, Stack<T>> PrefabPoolByPrefabId;
    public static Dictionary<int, int> PrefabIdByInstanceId;

    static ResourcesUtils()
    {
        PrefabPoolByPrefabId = new Dictionary<int, Stack<T>>(100);
        PrefabIdByInstanceId = new Dictionary<int, int>(100);
        SceneManager.sceneUnloaded += scene =>
        {
            PrefabPoolByPrefabId.Clear();
            PrefabIdByInstanceId.Clear();
        };
    }

    private static bool TryGetPrefabInstanceOrPrefab(string prefabPath, out int prefabId, out T instance)
    {
        var prefab = Resources.Load<T>(prefabPath);
        return TryGetPrefabInstanceOrPrefab(prefab, out prefabId, out instance);
    }

    private static bool TryGetPrefabInstanceOrPrefab(T prefab, out int prefabId, out T instance)
    {
        prefabId = prefab.GetInstanceID();
        if (!PrefabPoolByPrefabId.TryGetValue(prefabId, out var prefabPool))
        {
            prefabPool = new Stack<T>();
            PrefabPoolByPrefabId[prefabId] = prefabPool;
        }

        if (prefabPool.Count > 0)
        {
            var inst = default(T);
            while (prefabPool.Count > 0)
            {
                inst = prefabPool.Pop();
                if (inst == null)
                {
                    Debug.LogWarning($"Somehow prefabPool for prefab [{prefab}] had a null object stored! Ignoring it..");
                    continue;
                }

                break;
            }

            if (inst == null)
            {
                instance = prefab;
                return false;
            }

            instance = inst;
            return true;
        }

        instance = prefab;
        return false;
    }

    public static void GetPrefabInstance(string prefabPath, Vector3 position, Quaternion rotation, Transform parent, out T instance)
    {
        var prefab = Resources.Load<T>(prefabPath);
        GetPrefabInstance(prefab, position, rotation, parent, out instance);
    }

    public static void GetPrefabInstance(T prefab, Vector3 position, Quaternion rotation, Transform parent, out T instance)
    {
        if (prefab == null)
        {
            Debug.LogError($"Tried to get prefab instance but the prefab is null!");
            instance = null;
            return;
        }

        if (!TryGetPrefabInstanceOrPrefab(prefab, out var prefabId, out var prefabOrInstance))
        {
            instance = Object.Instantiate(prefabOrInstance, position, rotation, parent);
        }
        else
        {
            instance = prefabOrInstance;
            var transform = instance.transform;
            transform.position = position;
            transform.rotation = rotation;
            transform.parent = parent;
            instance.gameObject.SetActive(true);
        }

        PrefabIdByInstanceId[instance.GetInstanceID()] = prefabId;
    }

    public static void PreloadPool(string prefabPath, int count)
    {
        var prefab = Resources.Load<T>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Could not find a prefab at path [{prefabPath}]!");
            return;
        }

        var prefabId = prefab.GetInstanceID();
        if (!PrefabPoolByPrefabId.TryGetValue(prefabId, out var prefabPool))
        {
            prefabPool = new Stack<T>(count * 10);
            PrefabPoolByPrefabId[prefabId] = prefabPool;
        }

        for (var i = 0; i < count; i++)
        {
            var instance = Object.Instantiate(prefab);
            instance.gameObject.SetActive(false);
            prefabPool.Push(instance);
        }
    }
    
    public static T PreloadPool(T prefab, int count)
    {
        if (prefab == null)
        {
            Debug.LogError($"Trying to preload pool with null prefab!");
            return null;
        }

        var prefabId = prefab.GetInstanceID();
        if (!PrefabPoolByPrefabId.TryGetValue(prefabId, out var prefabPool))
        {
            prefabPool = new Stack<T>(count * 10);
            PrefabPoolByPrefabId[prefabId] = prefabPool;
        }

        for (var i = 0; i < count; i++)
        {
            var instance = Object.Instantiate(prefab);
            instance.gameObject.SetActive(false);
            prefabPool.Push(instance);
        }
        
        return prefab;
    }

    public static void GetPrefabInstance(string prefabPath, Vector3 position, Quaternion rotation, out T instance)
    {
        var prefab = Resources.Load<T>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Could not find a prefab at path [{prefabPath}]!");
            instance = null;
            return;
        }

        GetPrefabInstance(prefab, position, rotation, out instance);
    }

    public static void GetPrefabInstance(T prefab, Vector3 position, Quaternion rotation, out T instance)
    {
        if (!TryGetPrefabInstanceOrPrefab(prefab, out var prefabId, out var prefabOrInstance))
        {
            instance = Object.Instantiate(prefabOrInstance, position, rotation);
        }
        else
        {
            instance = prefabOrInstance;
            var transform = instance.transform;
            transform.parent = null;
            transform.position = position;
            transform.rotation = rotation;
            instance.gameObject.SetActive(true);
        }

        PrefabIdByInstanceId[instance.GetInstanceID()] = prefabId;
    }

    public static void StorePrefab(T instance)
    {
        if (instance == null)
        {
            return;
        }

        var instanceId = instance.GetInstanceID();
        if (!PrefabIdByInstanceId.TryGetValue(instanceId, out var prefabId))
        {
            Debug.LogError($"Trying to store an instance, but couldnt find the PrefabId to store it! Destroying object named [{instance}].");
            Object.Destroy(instance.gameObject);
            return;
        }

        instance.gameObject.SetActive(false);
        PrefabPoolByPrefabId[prefabId].Push(instance);
        PrefabIdByInstanceId.Remove(instanceId);
    }
}
