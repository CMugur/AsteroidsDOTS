using DOTS_Exercise.Data.Settings.Attributes;
using DOTS_Exercise.Data.Settings.Interfaces;
using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Data.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DOTS_Exercise.Data.Settings
{
    [Settings(Interface = typeof(IRequestUnitSettings), SetSettingsMethodName = "SetUnitSettings"),
        CreateAssetMenu(fileName = "Unit Settings Scriptable Object", menuName = "Scriptable Objects/Settings/Unit Settings")]
    public class UnitSettingsScriptableObject : SettingsScriptableObject
    {
        public List<AsteroidWaveScriptableObject> Waves;
        public PlayerUnitScriptableObject Player;
        public List<WeaponScriptableObject> Weapons;
        public UFOUnitScriptableObject UFO;

        public AsteroidWaveScriptableObject GetWave(int id)
        {
            return Waves.FirstOrDefault(w => w.ID == id);
        }

        public WeaponScriptableObject GetWeapon(int id)
        {
            return Weapons.FirstOrDefault(w => w.ID == id);
        }
    }
}