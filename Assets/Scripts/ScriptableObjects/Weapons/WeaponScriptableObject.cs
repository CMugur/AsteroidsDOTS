using UnityEngine;

namespace DOTS_Exercise.Data.Weapons
{
    [CreateAssetMenu(fileName = "Weapon Scriptable Object", menuName = "Scriptable Objects/Weapon")]
    public class WeaponScriptableObject : ScriptableObject
    {
        public int ID;
        public PlayerProjectileScriptableObject Projectile;
        public int ProjectileCount = 1;
        public float CooldownSeconds = 0;
    }
}