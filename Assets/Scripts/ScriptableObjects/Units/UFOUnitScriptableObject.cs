using DOTS_Exercise.Utils;
using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "UFO Unit Scriptable Object", menuName = "Scriptable Objects/Units/UFO Unit")]
    public class UFOUnitScriptableObject : PlayerUnitScriptableObject
    {
        public override void OnValidate()
        {
            UnitType = UnitTypes.UFO;
        }
    }
}