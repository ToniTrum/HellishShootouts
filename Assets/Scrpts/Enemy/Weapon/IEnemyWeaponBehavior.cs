using UnityEngine;

public interface IEnemyWeaponBehavior
{
    void Idle(Transform weaponTransform, bool isFlipped);
    void Walk(Transform weaponTransform, bool isFlipped);
}
