using UnityEngine;

public class ModifiedCharacterController : MonoBehaviour
{
    protected Entity _entity;

    public void Setup(Entity entity)
    {
        _entity = entity;
    }
}