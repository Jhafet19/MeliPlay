using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    private EnemyAI _brain;
    private Transform _currentTarget;

    public EnemyPatrolState(EnemyAI brain)
    {
        _brain = brain;
    }

    public void Enter()
    {
        _currentTarget = _brain.pointA;
    }

    public void Tick()
    {
        if (_currentTarget != null)
        {
            float dirX = Mathf.Sign(_currentTarget.position.x - _brain.transform.position.x);
            
            // Si estamos muy cerca, cambiamos de objetivo
            if (Mathf.Abs(_brain.transform.position.x - _currentTarget.position.x) < 0.5f)
            {
                _currentTarget = (_currentTarget == _brain.pointA) ? _brain.pointB : _brain.pointA;
                dirX = 0;
            }

            _brain.rb.linearVelocity = new Vector2(dirX * _brain.speed, _brain.rb.linearVelocity.y);
            
            // Girar sprite
            if (dirX != 0) _brain.sr.flipX = dirX < 0;
        }

        // 2. DECISIÓN: ¿HAY JUGADOR CERCA?
        float distanceToPlayer = Vector2.Distance(_brain.transform.position, _brain.playerTarget.position);
        
        if (distanceToPlayer < _brain.detectionRange)
        {
            // ¡CAMBIO DE ESTADO! -> PERSEGUIR
            _brain.ChangeState(new EnemyChaseState(_brain));
        }
    }

    public void Exit() { }
}
