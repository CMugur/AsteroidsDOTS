using UnityEngine;
using DOTS_Exercise.Data.Units;

namespace DOTS_Exercise.Data.Weapons
{
    [CreateAssetMenu(fileName = "Weapon Scriptable Object", menuName = "Scriptable Objects/Weapon")]
    public class WeaponScriptableObject : ScriptableObject
    {
        public int ID;
        public UnitScriptableObject Projectile;
        public int ProjectileCount = 1;
    }
}