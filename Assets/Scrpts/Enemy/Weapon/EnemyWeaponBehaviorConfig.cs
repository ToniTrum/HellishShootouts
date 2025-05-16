using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWeaponBehaviorConfig", menuName = "Enemy/Weapon/EnemyWeaponBehaviorConfig")]
public class WeaponBehaviorConfig : ScriptableObject, IEnemyWeaponBehavior
{
    [Header("Idle Settings")]
    public Vector2 idleOffset;
    public float idleSwayAmplitude = 0.1f;
    public float idleSwaySpeed = 2f;

    [Header("Walk Settings")]
    public Vector2 walkOffset;
    public float walkRotationOffset;

    public void Idle(Transform weaponTransform, bool isFlipped)
    {
        Vector2 basePosition = idleOffset;
        if (isFlipped)
        {
            basePosition.x = -idleOffset.x;
        }
        float sway = Mathf.Sin(Time.time * idleSwaySpeed) * idleSwayAmplitude;
        weaponTransform.localPosition = basePosition + new Vector2(0, sway);
    }

    public void Walk(Transform weaponTransform, bool isFlipped)
    {
        Vector2 basePosition = walkOffset;
        if (isFlipped)
        {
            basePosition.x = -walkOffset.x;
        }
        weaponTransform.localPosition = basePosition;
        Quaternion currentRotation = weaponTransform.rotation;
        weaponTransform.rotation = currentRotation * Quaternion.Euler(0, 0, walkRotationOffset);
    }
}