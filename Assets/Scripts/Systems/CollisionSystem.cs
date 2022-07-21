using DOTS_Exercise.ECS.Components.Shields;
using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.Utils;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace DOTS_Exercise.ECS.Systems
{
    public class CollisionSystem : CustomSystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEcbSystem;
        private EntityQuery _collisionQuery;

        private readonly Dictionary<UnitTypes, List<UnitTypes>> _collisionWhitelistMapping = new Dictionary<UnitTypes, List<UnitTypes>>()
        {
            { UnitTypes.Player, new List<UnitTypes>(){ UnitTypes.PlayerProjectile, UnitTypes.Powerup } },
            { UnitTypes.PlayerProjectile, new List<UnitTypes>(){ UnitTypes.Player, UnitTypes.UFOProjectile, UnitTypes.Powerup } },
            { UnitTypes.Asteroid, new List<UnitTypes>(){ UnitTypes.UFO, UnitTypes.UFOProjectile, UnitTypes.Powerup } },
            { UnitTypes.UFO, new List<UnitTypes>(){ UnitTypes.Asteroid, UnitTypes.UFOProjectile, UnitTypes.Powerup } },
            { UnitTypes.UFOProjectile, new List<UnitTypes>(){ UnitTypes.Asteroid, UnitTypes.UFO, UnitTypes.PlayerProjectile, UnitTypes.Powerup } },
            { UnitTypes.Powerup, new List<UnitTypes>(){ UnitTypes.Asteroid, UnitTypes.UFO, UnitTypes.PlayerProjectile, UnitTypes.UFOProjectile } }
        };

        protected override void OnCreate()
        {
            _endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _collisionQuery = GetEntityQuery(typeof(WorldRenderBounds), ComponentType.ReadOnly<UnitComponent>());
        }

        protected override void OnUpdate()
        {
            var entities = _collisionQuery.ToEntityArray(Allocator.TempJob);
            var entitiesManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var ecb = _endSimulationEcbSystem.CreateCommandBuffer();
            List<int> entitiesRemoved = new List<int>();
            for (int i = 0; i < entities.Length; i++)
            {
                if (entitiesRemoved.Contains(i) || entitiesManager.HasComponent<ShieldComponent>(entities[i]))
                {
                    continue;
                }

                var renderBounds_original = entitiesManager.GetComponentData<WorldRenderBounds>(entities[i]);
                var unitComponent_original = entitiesManager.GetComponentData<UnitComponent>(entities[i]);

                if (renderBounds_original.Value.Center.Equals(float3.zero) || unitComponent_original.Invulnerability)
                {
                    continue;
                }

                for (int j = 0; j < entities.Length; j++)
                {
                    if (entitiesRemoved.Contains(j) || i == j || entitiesManager.HasComponent<ShieldComponent>(entities[j]))
                    {
                        continue;
                    }

                    var renderBounds_toCompare = entitiesManager.GetComponentData<WorldRenderBounds>(entities[j]);
                    var unitComponent_toCompare = entitiesManager.GetComponentData<UnitComponent>(entities[j]);

                    if (renderBounds_toCompare.Value.Center.Equals(float3.zero) || unitComponent_toCompare.Invulnerability)
                    {
                        continue;
                    }

                    if (unitComponent_original.UnitType == unitComponent_toCompare.UnitType)
                    {
                        continue;
                    }

                    if (_collisionWhitelistMapping.ContainsKey(unitComponent_original.UnitType) && _collisionWhitelistMapping[unitComponent_original.UnitType].Contains(unitComponent_toCompare.UnitType))
                    {
                        continue;
                    }

                    Bounds bounds_original = new Bounds(renderBounds_original.Value.Center, renderBounds_original.Value.Size);
                    Bounds bounds_toCompare = new Bounds(renderBounds_toCompare.Value.Center, renderBounds_toCompare.Value.Size);
                    if (bounds_original.Intersects(bounds_toCompare))
                    {
                        if (entitiesManager.HasComponent<PowerupTagComponent>(entities[i]))
                        {
                            removePowerupEntity(entities[i], entities[j], unitComponent_original, bounds_original);
                            return;
                        }
                        else if (entitiesManager.HasComponent<PowerupTagComponent>(entities[j]))
                        {
                            removePowerupEntity(entities[j], entities[i], unitComponent_toCompare, bounds_toCompare);
                            return;
                        }

                        removeEntity(entities[i], unitComponent_original, bounds_original);
                        removeEntity(entities[j], unitComponent_toCompare, bounds_toCompare);
                    }
                }

                void removeEntity(Entity entity, UnitComponent unitComponent, Bounds bounds)
                {
                    ecb.DestroyEntity(entity);
                    Observer.Trigger(Events.Trigger_OnUnitDied, new UnitDiedWithPositionDTO()
                    {
                        ECB = ecb,
                        UnitComponent = unitComponent,
                        Position = bounds.center
                    });
                    entitiesRemoved.Add(i);
                }

                void removePowerupEntity(Entity powerupEntity, Entity otherEntity, UnitComponent unitComponent, Bounds bounds)
                {
                    ecb.DestroyEntity(powerupEntity);
                    Observer.Trigger(Events.Trigger_OnUnitDied, new PowerupDestroyedWithPositionDTO()
                    {
                        ECB = ecb,
                        UnitComponent = unitComponent,
                        Position = bounds.center,
                        PowerupTagComponent = entitiesManager.GetComponentData<PowerupTagComponent>(powerupEntity),
                        OtherEntity = otherEntity
                    });
                    entitiesRemoved.Add(i);
                }
            }
        }
    }
}