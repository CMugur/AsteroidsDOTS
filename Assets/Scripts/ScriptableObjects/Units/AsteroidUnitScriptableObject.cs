using System.Collections.Generic;
using UnityEngine;
using DOTS_Exercise.Utils;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Asteroid Unit Scriptable Object", menuName = "Scriptable Objects/Units/Asteroid Unit")]
    public class AsteroidUnitScriptableObject : UnitScriptableObject
    {
        public List<AsteroidUnitScriptableObject> AsteroidsToSpawnOnDeath;

        public void OnValidate()
        {
            UnitType = UnitTypes.Asteroid;
        }
    }
}