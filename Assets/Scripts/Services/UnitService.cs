using DOTS_Exercise.Data.Settings;
using DOTS_Exercise.Data.Settings.Interfaces;
using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Utils;
using System;
using Unity.Entities;

namespace DOTS_Exercise.Services
{
    public abstract class UnitService : IRequestUnitSettings
    {
        public UnitSettingsScriptableObject UnitSettings { get; set; }

        public void SetUnitSettings(UnitSettingsScriptableObject settings)
        {
            UnitSettings = settings;
        }

        protected Func<UnitScriptableObject, SpawnUnitDTO, Entity> _unitFactory;
        public UnitService(Func<UnitScriptableObject, SpawnUnitDTO, Entity> unitFactory)
        {
            _unitFactory = unitFactory;
        }

        public abstract void SpawnUnit(SpawnUnitDTO dto = null);

        public virtual void OnUnitDestroyed(UnitDiedWithPositionDTO dto) { }
    }
}