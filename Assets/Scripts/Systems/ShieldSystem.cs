using DOTS_Exercise.ECS.Components.Shields;
using DOTS_Exercise.Utils;
using Unity.Entities;

namespace DOTS_Exercise.ECS.Systems.Units
{
    public class ShieldSystem : CustomSystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEcbSystem;

        protected override void OnCreate()
        {
            _endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entities.ForEach((Entity entity, in ShieldComponent shieldComponent) =>
            {
                if (!shieldComponent.IsValid)
                {
                    EntityCommandBuffer ecb = _endSimulationEcbSystem.CreateCommandBuffer();
                    ecb.RemoveComponent<ShieldComponent>(entity);
                    Observer?.Trigger(Events.Trigger_OnPlayerShieldRemoved, null);
                }
            }).WithoutBurst().Run();
        }
    }
}