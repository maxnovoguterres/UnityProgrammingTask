using System;
using UnityEngine;

public class PlayerCharacterController : ModifiedCharacterController
{
    public Rigidbody Rigidbody;
    public float SmoothInputSpeed;

    private Vector2 _currentInput;
    private Vector3 _moveDirection;
    private Vector2 _smoothInputVelocity;
    
    private void FixedUpdate()
    {
        if (_entity.HealthComponent.IsDead)
        {
            return;
        }
        
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
            Rigidbody.linearVelocity = _moveDirection * (stats.MoveSpeed * (1 + stats.MoveSpeedModifier));
        }
        else
        {
            Rigidbody.linearVelocity = Vector3.zero;
        }
    }
}