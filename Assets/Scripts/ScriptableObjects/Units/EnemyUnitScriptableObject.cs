using System.Collections.Generic;
using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Enemy Unit Scriptable Object", menuName = "Scriptable Objects/Units/Enemy Unit")]
    public class EnemyUnitScriptableObject : UnitScriptableObject
    {
        public List<EnemyUnitScriptableObject> EnemiesToSpawnOnDeath;
    }
}