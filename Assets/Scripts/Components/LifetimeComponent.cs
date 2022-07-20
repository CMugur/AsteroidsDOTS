using System;
using Unity.Entities;

namespace DOTS_Exercise.ECS.Components.Units
{
    public struct LifetimeComponent : IComponentData
    {
        public DateTime CreatedTime;
        public float Lifespan;
        public bool IsAlive => (DateTime.Now - CreatedTime).TotalSeconds <= Lifespan;
    }
}