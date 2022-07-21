using DOTS_Exercise.Data.Units;
using DOTS_Exercise.ECS.Components.Shields;
using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using DOTS_Exercise.Utils;
using System;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Events;

namespace DOTS_Exercise.Services
{
    public class PowerupUnitService : UnitService
    {
        public UnityEvent OnShieldAdded = new UnityEvent();

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
            if (powerup.WeaponUpgrade != null && entityManager.HasComponent<WeaponComponent>(pDTO.OtherEntity))
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

            if (powerup.Shield != null)
            {
                if (!entityManager.HasComponent<ShieldComponent>(pDTO.OtherEntity))
                {
                    if (!dto.ECB.HasValue)
                    {
                        entityManager.AddComponent(pDTO.OtherEntity, typeof(ShieldComponent));
                    }
                    else
                    {
                        dto.ECB.Value.AddComponent(pDTO.OtherEntity, typeof(ShieldComponent));
                    }
                }

                if (!dto.ECB.HasValue)
                {
                    entityManager.SetComponentData(pDTO.OtherEntity, new ShieldComponent()
                    {
                        CreatedTime = DateTime.Now,
                        Lifespan = powerup.Shield.LifetimeSeconds
                    });
                }
                else
                {
                    dto.ECB.Value.SetComponent(pDTO.OtherEntity, new ShieldComponent() 
                    { 
                        CreatedTime = DateTime.Now,
                        Lifespan = powerup.Shield.LifetimeSeconds
                    });
                }

                OnShieldAdded?.Invoke();
            }
        }
    }
}