using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Utils;
using System;
using Unity.Entities;

namespace DOTS_Exercise.Services
{
    public class PowerupUnitService : UnitService
    {
        public PowerupUnitService(Func<UnitScriptableObject, SpawnUnitDTO, Entity> unitFactory) : base(unitFactory) { }

        public override void SpawnUnit(SpawnUnitDTO dto = null)
        {
            
        }
    }
}