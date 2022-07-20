using DOTS_Exercise.Data.Weapons;
using DOTS_Exercise.Utils;
using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Player Unit Scriptable Object", menuName = "Scriptable Objects/Units/Player Unit")]
    public class PlayerUnitScriptableObject : UnitScriptableObject
    {
        public WeaponScriptableObject DefaultWeapon;
        public float InvulnerabilityOnSpawnSeconds = 1f;

        public virtual void OnValidate()
        {
            UnitType = UnitTypes.Player;
        }
    }
}