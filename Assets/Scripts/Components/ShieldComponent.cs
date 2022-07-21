using System;
using Unity.Entities;

namespace DOTS_Exercise.ECS.Components.Shields
{
    public struct ShieldComponent : IComponentData
    {
        public DateTime CreatedTime;
        public float Lifespan;
        public bool IsValid => (DateTime.Now - CreatedTime).TotalSeconds <= Lifespan;
    }
}