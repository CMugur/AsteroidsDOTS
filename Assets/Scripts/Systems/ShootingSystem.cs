using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using DOTS_Exercise.Utils;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_Exercise.ECS.Systems
{
    public class ShootingSystem : CustomSystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEcbSystem;
        private EntityQuery _playerQuery;

        protected override void OnCreate()
        {
            _endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTagComponent>(), ComponentType.ReadOnly<Translation>());
        }

        protected override void OnUpdate()
        {
            bool fire = Input.GetKeyDown(KeyCode.Space);
            EntityCommandBuffer ecb = _endSimulationEcbSystem.CreateCommandBuffer();
            Entities.ForEach((ref WeaponComponent weaponComponent, in Rotation rotation, in Translation translationComponent, in PlayerTagComponent playerTagComponent) =>
            {
                if (fire && weaponComponent.CanShoot)
                {
                    if (Observer == null)
                    {
                        Debug.LogError("Observer is not set.");
                        return;
                    }

                    SpawnProjectileDTO dto = new SpawnProjectileDTO() 
                    { 
                        Position = translationComponent.Value, 
                        Direction = math.mul(rotation.Value, new float3(0, 1, 0)), 
                        WeaponID = weaponComponent.ID,
                        ECB = ecb
                    };
                    Observer.TriggerRequest(Events.Request_SpawnPlayerProjectile, dto);
                    weaponComponent.LastShotTime = DateTime.Now;
                }
            }).WithoutBurst().Run();

            Entities.ForEach((ref WeaponComponent weaponComponent, in Translation translationComponent, in UFOTagComponent playerTagComponent) =>
            {
                if (weaponComponent.CanShoot)
                {
                    if (Observer == null)
                    {
                        Debug.LogError("Observer is not set.");
                        return;
                    }

                    var players = _playerQuery.ToEntityArray(Allocator.TempJob);
                    if (players.Length == 0)
                    {
                        return;
                    }

                    var entitiesManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                    var playerTranslation = entitiesManager.GetComponentData<Translation>(players[0]);

                    SpawnProjectileDTO dto = new SpawnProjectileDTO() 
                    { 
                        Position = translationComponent.Value, 
                        Direction = ((Vector3)(playerTranslation.Value - translationComponent.Value)).normalized, 
                        WeaponID = weaponComponent.ID,
                        ECB = ecb
                    };
                    Observer.TriggerRequest(Events.Request_SpawnUFOProjectile, dto);
                    weaponComponent.LastShotTime = DateTime.Now;
                }
            }).WithoutBurst().Run();


        }
    }
}