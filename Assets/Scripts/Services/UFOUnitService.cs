using DOTS_Exercise.Data.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using DOTS_Exercise.Utils;
using System;
using Unity.Entities;
using UnityEngine;

namespace DOTS_Exercise.Services
{
    public class UFOUnitService : UnitService
    {
        public UFOUnitService(Func<UnitScriptableObject, SpawnUnitDTO, Entity> unitFactory) : base(unitFactory) { }

        public override void SpawnUnit(SpawnUnitDTO dto = null)
        {
            var ufoEntity = _unitFactory(UnitSettings.UFO, new SpawnUnitDTO()
            {
                Position = new Unity.Mathematics.float3(Camera.main.orthographicSize * Camera.main.aspect * (-0.9f), Camera.main.orthographicSize * 2f / 3f, 0),
                Direction = new Unity.Mathematics.float3(UnitSettings.UFO.MovementSpeed * -1f, 0, 0)
            });

            if (dto == null || !dto.ECB.HasValue)
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entityManager.SetComponentData(ufoEntity, new WeaponComponent() 
                { 
                    ID = UnitSettings.UFO.DefaultWeapon.ID,
                    CooldownSeconds = UnitSettings.UFO.DefaultWeapon.CooldownSeconds,
                    LastShotTime = DateTime.Now
                });
            }
            else if(dto != null && dto.ECB.HasValue)
            {
                dto.ECB.Value.SetComponent(ufoEntity, new WeaponComponent() 
                { 
                    ID = UnitSettings.UFO.DefaultWeapon.ID,
                    CooldownSeconds = UnitSettings.UFO.DefaultWeapon.CooldownSeconds,
                    LastShotTime = DateTime.Now
                });
            }
        }
    }
}