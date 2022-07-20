using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Utils;
using UnityEngine;

namespace DOTS_Exercise.Data.Weapons
{
    [CreateAssetMenu(fileName = "Player Projectile Scriptable Object", menuName = "Scriptable Objects/Projectile/Player")]
    public class PlayerProjectileScriptableObject : UnitScriptableObject
    {
        public float Lifetime = 5f;

        public virtual void OnValidate()
        {
            UnitType = UnitTypes.PlayerProjectile;
        }
    }
}