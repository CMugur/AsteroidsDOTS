using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Asteroid Wave Scriptable Object", menuName = "Scriptable Objects/Units/Asteroid Wave")]
    public class AsteroidWaveScriptableObject : ScriptableObject
    {
        public int ID;
        public List<AsteroidUnitScriptableObject> Asteroids;
        public float SpawnAreaOffsetFromCameraSize = 0.5f;
        public float PlayerSafeRadius = 1f;
        public int NextWaveID;

        // Works only with 1 level deep
        public int TotalAsteroids => Asteroids.Sum(a => a.AsteroidsToSpawnOnDeath.Count) + Asteroids.Count;
    }
}