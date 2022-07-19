using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using DOTS_Exercise.Utils;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_Exercise.ECS.Systems
{
    public class ShootingSystem : CustomSystemBase
    {
        EndSimulationEntityCommandBufferSystem _endSimulationEcbSystem;

        protected override void OnCreate()
        {
            _endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            bool fire = Input.GetKeyDown(KeyCode.Space);
            EntityCommandBuffer ecb = _endSimulationEcbSystem.CreateCommandBuffer();
            Entities.ForEach((in UnitComponent unitComponent, in WeaponComponent weaponComponent, in Translation translationComponent) =>
            {
                if (fire)
                {
                    if (_observer == null)
                    {
                        Debug.LogError("Observer is not set.");
                        return;
                    }

                    SpawnProjectileDTO dto = new SpawnProjectileDTO() 
                    { 
                        Position = translationComponent.Value, 
                        Direction = unitComponent.Direction, 
                        WeaponID = weaponComponent.ID,
                        ECB = ecb
                    };
                    _observer.TriggerRequest(Events.Request_SpawnProjectile, dto);
                }
            }).WithoutBurst().Run();
        }
    }
}