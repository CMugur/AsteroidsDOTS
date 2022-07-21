using DOTS_Exercise.Data.Units;
using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
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
            PowerupUnitScriptableObject powerup = UnitSettings.GetPowerup(UnitSettings.UFO.PowerupToSpawnOnDeath.ID);
            var powerupEntity = _unitFactory(powerup, dto);

            if (dto == null || !dto.ECB.HasValue)
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entityManager.SetComponentData(powerupEntity, new PowerupTagComponent() { ID = powerup.ID });
            }
            else if (dto != null && dto.ECB.HasValue)
            {
                dto.ECB.Value.SetComponent(powerupEntity, new PowerupTagComponent() { ID = powerup.ID });
            }
        }

        public override void OnUnitDestroyed(UnitDiedWithPositionDTO dto)
        {
            PowerupDestroyedWithPositionDTO pDTO = dto as PowerupDestroyedWithPositionDTO;
            PowerupUnitScriptableObject powerup = UnitSettings.GetPowerup(pDTO.PowerupTagComponent.ID);

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (entityManager.HasComponent<WeaponComponent>(pDTO.OtherEntity))
            {
                if (!dto.ECB.HasValue)
                {
                    entityManager.SetComponentData(pDTO.OtherEntity, new WeaponComponent()
                    {
                        ID = powerup.WeaponUpgrade.ID,
                        CooldownSeconds = powerup.WeaponUpgrade.CooldownSeconds
                    });
                }
                else
                {
                    dto.ECB.Value.SetComponent(pDTO.OtherEntity, new WeaponComponent()
                    {
                        ID = powerup.WeaponUpgrade.ID,
                        CooldownSeconds = powerup.WeaponUpgrade.CooldownSeconds
                    });
                }
            }
        }
    }
}