using UnityEngine;

[CreateAssetMenu(fileName = "LeraieWeaponBehaviorConfig", menuName = "Enemy/Weapon/BehaviorConfig/Leraie")]
public class LeraieWeaponBehaviorConfig : EnemyWeaponBehavior
{
    [Header("Idle Settings")]
    public Vector2 idleOffset;
    public float idleSwayAmplitude = 0.1f;
    public float idleSwaySpeed = 2f;
    public float idleRotationOffset;

    [Header("Walk Settings")]
    public Vector2 walkOffset;
    public float walkRotationOffset;

    [Header("Attack Settings")]
    public Vector2 attackOffset;
    public float attackSpeed;

    public override void Idle(Transform weaponTransform, bool isFlipped)
    {
        Vector2 basePosition = idleOffset;
        if (isFlipped)
        {
            basePosition.x = -idleOffset.x;
        }
        float sway = Mathf.Sin(Time.time * idleSwaySpeed) * idleSwayAmplitude;
        weaponTransform.localPosition = basePosition + new Vector2(0, sway);
        Quaternion currentRotation = weaponTransform.rotation;
        weaponTransform.rotation = currentRotation * Quaternion.Euler(0, 0, idleRotationOffset);
    }

    public override void Walk(Transform weaponTransform, bool isFlipped)
    {
        Vector2 basePosition = walkOffset;
        if (isFlipped)
        {
            basePosition.x = -walkOffset.x;
            weaponTransform.rotation = Quaternion.Euler(0, 0, 180);
        }
        weaponTransform.localPosition = basePosition;
        Quaternion currentRotation = weaponTransform.rotation;
        weaponTransform.rotation = currentRotation * Quaternion.Euler(0, 0, walkRotationOffset);
    }

    public override void Attack
    (
        Transform weaponTransform,
        bool isFlipped,
        Vector3 initialPosition,
        float attackStartTime,
        ref bool isAttacking,
        Vector2 directionToPlayer
    )
    {
        

        Quaternion currentRotation = weaponTransform.rotation;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = currentRotation * Quaternion.Euler(0, 0, angle);
    }
}
