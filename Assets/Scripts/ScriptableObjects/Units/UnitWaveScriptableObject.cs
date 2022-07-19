using System.Collections.Generic;
using UnityEngine;

namespace DOTS_Exercise.Data.Units
{
    [CreateAssetMenu(fileName = "Unit Wave Scriptable Object", menuName = "Scriptable Objects/Units/Unit Wave")]
    public class UnitWaveScriptableObject : ScriptableObject
    {
        public int ID;
        public List<UnitScriptableObject> Units;
    }
}