using UnityEngine;

[CreateAssetMenu(fileName = "FurcasWeaponBehaviorConfig", menuName = "Enemy/Weapon/BehaviorConfig/Furcas")]
public class FurcasWeaponBehaviorConfig : EnemyWeaponBehavior
{
    [Header("Idle Settings")]
    public Vector2 idleOffset;
    public float idleSwayAmplitude = 0.1f;
    public float idleSwaySpeed = 2f;

    [Header("Walk Settings")]
    public Vector2 walkOffset;
    public float walkRotationOffset;

    [Header("Attack Settings")]
    public Vector2 attackOffset;
    public float attackSpeed;
    public float attackRotationOffset;
    public float backDistance = 0.2f;
    public float forwardDistance = 0.5f;
    public float backDuration = 0.2f;
    public float forwardDuration = 0.3f;
    public float totalAttackDuration = 0.7f;

    public override void Idle(Transform weaponTransform, bool isFlipped)
    {
        Vector2 basePosition = idleOffset;
        if (isFlipped)
        {
            basePosition.x = -idleOffset.x;
        }
        float sway = Mathf.Sin(Time.time * idleSwaySpeed) * idleSwayAmplitude;
        weaponTransform.localPosition = basePosition + new Vector2(0, sway);
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
        float elapsedTime = Time.time - attackStartTime;
        Vector2 basePosition = attackOffset;
        if (isFlipped)
        {
            basePosition.x = -attackOffset.x;
        }
        initialPosition += (Vector3)basePosition;

        if (elapsedTime < backDuration)
        {
            float t = elapsedTime / backDuration;
            Vector3 offset = directionToPlayer * backDistance;
            Vector3 targetPosition = initialPosition - offset;
            weaponTransform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
        }
        else if (elapsedTime < backDuration + forwardDuration)
        {
            float t = (elapsedTime - backDuration) / forwardDuration;
            Vector3 backPosition = initialPosition - (Vector3)(directionToPlayer * backDistance);
            Vector3 forwardPosition = initialPosition + (Vector3)(directionToPlayer * forwardDistance);
            weaponTransform.localPosition = Vector3.Lerp(backPosition, forwardPosition, t);
        }
        else if (elapsedTime < totalAttackDuration)
        {
            float t = (elapsedTime - (backDuration + forwardDuration)) / (totalAttackDuration - (backDuration + forwardDuration));
            Vector3 forwardPosition = initialPosition + (Vector3)(directionToPlayer * forwardDistance);
            weaponTransform.localPosition = Vector3.Lerp(forwardPosition, initialPosition, t);
        }
        else
        {
            weaponTransform.localPosition = initialPosition;
            isAttacking = false;
        }

        Quaternion currentRotation = weaponTransform.rotation;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + attackRotationOffset;
        weaponTransform.rotation = currentRotation * Quaternion.Euler(0, 0, angle);
    }
}