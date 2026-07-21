using UnityEngine;


public enum EnemyStateId
{
    Patrol,
    Chase, 
    Attack,
    Dead
}
public interface IEnemyState 
{
    void Enter();
    EnemyStateId? Tick(float deltaTime);
    void Exit();
}
