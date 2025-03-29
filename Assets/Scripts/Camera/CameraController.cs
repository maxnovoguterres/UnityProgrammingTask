using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 Offset;
    public Vector3 Angle;
    public float FollowSpeed;
    
    private Transform _targetTransform;
    private Vector3 _desiredPosition;
    
    public void Setup(Entity targetEntity)
    {
        transform.eulerAngles = Angle;
        _targetTransform = targetEntity.transform;
    }

    private void LateUpdate()
    {
        if (_targetTransform == null)
        {
            return;
        }

        _desiredPosition = _targetTransform.position + Offset;
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, FollowSpeed * Time.deltaTime);
    }
}