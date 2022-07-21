using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Data.Weapons;
using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.Utils;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_Exercise.Services
{
    public class ProjectileUnitService : UnitService
    {
        public ProjectileUnitService(Func<UnitScriptableObject, SpawnUnitDTO, Entity> unitFactory) : base(unitFactory) { }

        public override void SpawnUnit(SpawnUnitDTO dto = null)
        {
            if (dto == null || !(dto is SpawnProjectileDTO pDTO))
            {
                Debug.LogError("Cannot spawn projectile");
                return;
            }

            WeaponScriptableObject weapon = UnitSettings.GetWeapon(pDTO.WeaponID);
            if (weapon == null)
            {
                Debug.LogError($"No weapon found for this id: {pDTO.WeaponID}");
                return;
            }

            int projectileCount = weapon.ProjectileCount;
            int angleCount = 0;
            List<float3> directions = new List<float3>();
            
            while (angleCount < projectileCount)
            {
                directions.Add(Quaternion.Euler(0, 0, weapon.AngleBetweenProjectiles * angleCount) * dto.Direction);
                angleCount++;
            }

            while (projectileCount > 0)
            {
                pDTO.Direction = directions[projectileCount - 1];
                var entity = _unitFactory(weapon.Projectile, pDTO);
                if (!dto.ECB.HasValue)
                {
                    World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, new LifetimeComponent
                    {
                        CreatedTime = DateTime.Now,
                        Lifespan = weapon.Projectile.Lifetime
                    });
                }
                else
                {
                    dto.ECB.Value.SetComponent(entity, new LifetimeComponent
                    {
                        CreatedTime = DateTime.Now,
                        Lifespan = weapon.Projectile.Lifetime
                    });
                }

                projectileCount--;
            }
        }
    }
}