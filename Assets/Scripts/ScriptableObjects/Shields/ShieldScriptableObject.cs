using UnityEngine;

namespace DOTS_Exercise.Data.Shields
{
    [CreateAssetMenu(fileName = "Shield Scriptable Object", menuName = "Scriptable Objects/Shield")]
    public class ShieldScriptableObject : ScriptableObject
    {
        public int ID;
        public float LifetimeSeconds;
    }
}