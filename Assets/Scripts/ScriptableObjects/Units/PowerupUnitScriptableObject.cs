using DOTS_Exercise.Utils;
using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Powerup Unit Scriptable Object", menuName = "Scriptable Objects/Units/Powerup Unit")]
    public class PowerupUnitScriptableObject : UnitScriptableObject
    {
        public virtual void OnValidate()
        {
            UnitType = UnitTypes.Powerup;
        }
    }
}