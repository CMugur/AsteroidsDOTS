using DOTS_Exercise.Data.Units;
using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using DOTS_Exercise.Utils;
using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_Exercise.Services
{
    public class PlayerUnitService : UnitService
    {
        public PlayerUnitService(Func<UnitScriptableObject, SpawnUnitDTO, Entity> unitFactory) : base(unitFactory) { }

        public override void SpawnUnit(SpawnUnitDTO dto = null)
        {
            if (UnitSettings.Player == null)
            {
                return;
            }

            var playerEntity = _unitFactory(UnitSettings.Player, new SpawnUnitDTO() { Direction = float3.zero , Position = new float3(0.001f, 0f, 0f) });
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var unitComponent = entityManager.GetComponentData<UnitComponent>(playerEntity);
            if (dto == null || !dto.ECB.HasValue)
            {
                entityManager.SetComponentData(playerEntity, new WeaponComponent() 
                { 
                    ID = UnitSettings.Player.DefaultWeapon.ID,
                    CooldownSeconds = UnitSettings.Player.DefaultWeapon.CooldownSeconds,
                    LastShotTime = DateTime.Now
                });
                entityManager.SetComponentData(playerEntity, new InvulnerabilityComponent() 
                { 
                    CreatedTime = DateTime.Now, 
                    CreatedInvulnerabilitySeconds = UnitSettings.Player.InvulnerabilityOnSpawnSeconds 
                });
                if (dto != null)
                {
                    entityManager.SetComponentData(playerEntity, new UnitComponent() 
                    {
                        Lives = ((SpawnPlayerDTO)dto).Lifes,
                        MovementSpeed = unitComponent.MovementSpeed,
                        RotationSpeed = unitComponent.RotationSpeed,
                        CanRotate = unitComponent.CanRotate,
                        UnitType = unitComponent.UnitType,
                        ID = unitComponent.ID,
                        Invulnerability = true
                    });
                }
            }
            else if (dto != null && dto.ECB.HasValue)
            {
                dto.ECB.Value.SetComponent(playerEntity, new WeaponComponent() 
                { 
                    ID = UnitSettings.Player.DefaultWeapon.ID,
                    CooldownSeconds = UnitSettings.Player.DefaultWeapon.CooldownSeconds,
                    LastShotTime = DateTime.Now
                });
                dto.ECB.Value.SetComponent(playerEntity, new InvulnerabilityComponent() 
                { 
                    CreatedTime = DateTime.Now, 
                    CreatedInvulnerabilitySeconds = UnitSettings.Player.InvulnerabilityOnSpawnSeconds 
                });
                dto.ECB.Value.SetComponent(playerEntity, new UnitComponent()
                {
                    Lives = ((SpawnPlayerDTO)dto).Lifes,
                    MovementSpeed = unitComponent.MovementSpeed,
                    RotationSpeed = unitComponent.RotationSpeed,
                    CanRotate = unitComponent.CanRotate,
                    UnitType = unitComponent.UnitType,
                    ID = unitComponent.ID,
                    Invulnerability = true
                });
            }
        }

        public override void OnUnitDestroyed(UnitDiedWithPositionDTO dto)
        {
            if (dto.UnitComponent.Lives == 1)
            {
                Debug.LogError("GAME OVER");
                return;
            }

            SpawnPlayerDTO spawnDTO = new SpawnPlayerDTO()
            {
                ECB = dto.ECB,
                Position = float3.zero,
                Lifes = dto.UnitComponent.Lives - 1
            };

            SpawnUnit(spawnDTO);
        }
    }
}