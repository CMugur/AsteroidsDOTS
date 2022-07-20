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
            { UnitTypes.Player, new List<UnitTypes>(){ UnitTypes.PlayerProjectile } },
            { UnitTypes.PlayerProjectile, new List<UnitTypes>(){ UnitTypes.Player, UnitTypes.UFOProjectile } },
            { UnitTypes.Asteroid, new List<UnitTypes>(){ UnitTypes.UFO, UnitTypes.UFOProjectile } },
            { UnitTypes.UFO, new List<UnitTypes>(){ UnitTypes.Asteroid, UnitTypes.UFOProjectile } },
            { UnitTypes.UFOProjectile, new List<UnitTypes>(){ UnitTypes.Asteroid, UnitTypes.UFO, UnitTypes.PlayerProjectile } }
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
                if (entitiesRemoved.Contains(i))
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
                    if (entitiesRemoved.Contains(j) || i == j)
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
                        ecb.DestroyEntity(entities[i]);
                        Observer.Trigger(Events.Trigger_OnUnitDied, new UnitDiedWithPositionDTO() 
                        { 
                            ECB = ecb, 
                            UnitComponent = unitComponent_original,
                            Position = bounds_original.center
                        });
                        entitiesRemoved.Add(i);
                        ecb.DestroyEntity(entities[j]);
                        Observer.Trigger(Events.Trigger_OnUnitDied, new UnitDiedWithPositionDTO() 
                        { 
                            ECB = ecb, 
                            UnitComponent = unitComponent_toCompare,
                            Position = bounds_toCompare.center
                        });
                        entitiesRemoved.Add(j);
                    }
                }
            }
        }
    }
}