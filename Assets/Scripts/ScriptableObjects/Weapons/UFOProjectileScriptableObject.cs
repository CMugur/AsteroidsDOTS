using DOTS_Exercise.Utils;
using UnityEngine;

namespace DOTS_Exercise.Data.Weapons
{
    [CreateAssetMenu(fileName = "UFO Projectile Scriptable Object", menuName = "Scriptable Objects/Projectile/UFO")]
    public class UFOProjectileScriptableObject : PlayerProjectileScriptableObject
    {
        public override void OnValidate()
        {
            UnitType = UnitTypes.UFOProjectile;
        }
    }
}