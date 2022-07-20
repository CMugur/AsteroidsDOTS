using DOTS_Exercise.ECS.Components.Units;
using Unity.Entities;

namespace DOTS_Exercise.ECS.Systems.Units
{
    public class LifetimeSystem : CustomSystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEcbSystem;

        protected override void OnCreate()
        {
            _endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, in LifetimeComponent lifetimeComponent) => 
            {
                if (!lifetimeComponent.IsAlive)
                {
                    EntityCommandBuffer ecb = _endSimulationEcbSystem.CreateCommandBuffer();
                    ecb.DestroyEntity(entity);
                }
            }).WithoutBurst().Run();
        }
    }
}