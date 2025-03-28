using UnityEngine;

public class JobRunner : MonoBehaviour
{
    private static JobRunner _instance;
    public static JobRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("JobRunner").AddComponent<JobRunner>();
            }

            return _instance;
        }
    }
}