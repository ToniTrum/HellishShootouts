using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWeaponConfig", menuName = "Enemy/Weapon/EnemyWeaponConfig")]
public class EnemyWeaponConfig : ScriptableObject
{
    public Sprite sprite;
    public EnemyWeaponBehaviorConfig behavior;
}
