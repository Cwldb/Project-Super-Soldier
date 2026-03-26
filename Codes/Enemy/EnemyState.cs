using Unity.Behavior;

namespace _01_Scripts.Enemy
{
    [BlackboardEnum]
    public enum EnemyState
    {
        IDLE = 1,
        PATROL = 2,
        CHASE = 3,
        ATTACK = 4,
        HIT = 5,
        DEAD = 6
    }
}