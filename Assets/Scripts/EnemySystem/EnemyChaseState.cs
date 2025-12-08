using UnityEngine;

public class EnemyChaseState: IEnemyState
{
    private EnemyAI _brain;

    public EnemyChaseState(EnemyAI brain)
    {
        _brain = brain;
    }

    public void Enter()
    {
        _brain.sr.color = Color.red; 
    }

    public void Tick()
    {
        float dirX = Mathf.Sign(_brain.playerTarget.position.x - _brain.transform.position.x);
        
        _brain.rb.linearVelocity = new Vector2(dirX * _brain.chaseSpeed, _brain.rb.linearVelocity.y);
        
        if (dirX != 0) _brain.sr.flipX = dirX < 0;

        float distanceToPlayer = Vector2.Distance(_brain.transform.position, _brain.playerTarget.position);

        if (distanceToPlayer > _brain.lostRange)
        {
            _brain.ChangeState(new EnemyPatrolState(_brain));
        }
    }

    public void Exit()
    {
        _brain.sr.color = Color.white;
        _brain.rb.linearVelocity = Vector2.zero;
    }
}