using UnityEngine;

public abstract class EnemyWeaponBehavior: ScriptableObject
{
    public abstract void Idle(Transform weaponTransform, bool isFlipped);
    public abstract void Walk(Transform weaponTransform, bool isFlipped);
    public abstract void Attack
    (
        Transform weaponTransform,
        bool isFlipped,
        Vector3 initialPosition,
        float attackStartTime,
        ref bool isAttacking,
        Vector2 directionToPlayer
    );
}
