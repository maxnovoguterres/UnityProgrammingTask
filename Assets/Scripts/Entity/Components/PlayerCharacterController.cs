using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCharacterController : ModifiedCharacterController
{
    public Rigidbody Rigidbody;
    public float SmoothInputSpeed;
    public Vector3 Gravity = new Vector3(0, -9.81f, 0);
    public float StepOffset = 0.5f;
    public float StepRayDistance = 0.5f;
    public CapsuleCollider CapsuleCollider;
    public LayerMask GroundMask;
    public QueryTriggerInteraction TriggerInteraction;
    
    private Vector2 _currentInput;
    private Vector3 _moveDirection;
    private Vector2 _smoothInputVelocity;
    
    private void FixedUpdate()
    {
        if (_entity.HealthComponent.IsDead)
        {
            return;
        }

        var velocity = Rigidbody.linearVelocity;
        var speed = velocity.magnitude;
        var direction = speed > 0f ? velocity / speed : Vector3.zero;
        var distance = speed * Time.deltaTime;
        FindGround(direction, distance);
        var input = InputManager.Instance.MoveAction.ReadValue<Vector2>();
        _currentInput = Vector2.SmoothDamp(_currentInput, input, ref _smoothInputVelocity, SmoothInputSpeed);
        _moveDirection = new Vector3
        {
            x = _currentInput.x,
            y = 0f,
            z = _currentInput.y
        }.ToIso();

        if (_moveDirection != Vector3.zero)
        {
            var stats = _entity.StatsComponent.Stats;
            velocity = _moveDirection * (stats.MoveSpeed * (1 + stats.MoveSpeedModifier));
        }
        else
        {
            velocity = Vector3.zero;
        }

        velocity.y = Rigidbody.linearVelocity.y;
        velocity += Gravity * Time.deltaTime;
        Rigidbody.linearVelocity = velocity;
    }
    
    private void FindGround(Vector3 direction, float distance = Mathf.Infinity, float backstepDistance = 0.05f)
    {
        var radius = CapsuleCollider.radius;
        var height = Mathf.Max(0.0f, CapsuleCollider.height * 0.5f - radius);
        var center = CapsuleCollider.center - Vector3.up * height;
        var origin = transform.TransformPoint(center);
        var up = transform.up;
        if (!SphereCast(origin, radius, direction, out var hitInfo, distance, backstepDistance) || Vector3.Angle(hitInfo.normal, up) >= 89.0f)
        {
            return;
        }

        var p = transform.position - transform.up * hitInfo.distance;
        StepClimb(p);
    }
    
    private bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float distance, float backstepDistance = 0.05f)
    {
        origin -= direction * backstepDistance;
        var hit = Physics.SphereCast(origin, radius, direction, out hitInfo, distance + backstepDistance, GroundMask, TriggerInteraction);
        if (hit)
        {
            hitInfo.distance -= backstepDistance;
        }

        return hit;
    }
    
    private void StepClimb(Vector3 moveDirection)
    {
        var rayOriginLower = Rigidbody.position + Vector3.up * 0.01f;
        if (Physics.Raycast(rayOriginLower, moveDirection, out _, StepRayDistance))
        {
            var rayOriginHigh = Rigidbody.position + Vector3.up * StepOffset;
            if (!Physics.Raycast(rayOriginHigh, moveDirection, out _, StepRayDistance))
            {
                var stepCheckOrigin = Rigidbody.position + moveDirection * StepRayDistance + Vector3.up * StepOffset;
                if (Physics.Raycast(stepCheckOrigin, Vector3.down, out var hitDown, StepOffset))
                {
                    var stepHeightDifference = hitDown.point.y - Rigidbody.position.y;
                    if (stepHeightDifference > 0f && stepHeightDifference <= StepOffset)
                    {
                        var newPosition = new Vector3(Rigidbody.position.x, Rigidbody.position.y + stepHeightDifference, Rigidbody.position.z);
                        transform.position = newPosition;
                        Rigidbody.MovePosition(newPosition);
                    }
                }
            }
        }
    }
}