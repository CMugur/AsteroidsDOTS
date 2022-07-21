using DOTS_Exercise.Utils;
using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Unit Scriptable Object", menuName = "Scriptable Objects/Units/Unit")]
    public class UnitScriptableObject : ScriptableObject
    {
        public int ID;
        public UnitTypes UnitType;
        public Sprite Sprite;
        public int Lives = 1;
        public float MovementSpeed = 1;
        public float RotationSpeed = 1;
        public bool CanRotate = false;
        public int ScoreAddedOnDeath = 0;
    }
}