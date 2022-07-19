using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Unit Scriptable Object", menuName = "Scriptable Objects/Units/Unit")]
    public class UnitScriptableObject : ScriptableObject
    {
        public Sprite Sprite;
        public int Health = 1;
        public int Lives = 1;
        public float Speed = 1;
        public bool CanRotate = false;
    }
}