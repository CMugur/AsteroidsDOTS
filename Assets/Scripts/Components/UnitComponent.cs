using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_Exercise.ECS.Components.Units
{
    public struct UnitComponent : IComponentData
    {
        public float MovementSpeed;
        public float3 Direction;
        public int Health;
        public int Lives;
        public bool CanRotate;
    }
}