using UnityEngine;

public interface IEnemyWeaponBehavior
{
    void Idle(Transform weaponTransform, bool isFlipped);
    void Walk(Transform weaponTransform, bool isFlipped);
    void Attack
    (
        Transform weaponTransform,
        bool isFlipped,
        Vector3 initialPosition,
        float attackStartTime,
        ref bool isAttacking,
        Vector2 directionToPlayer
    );
}
