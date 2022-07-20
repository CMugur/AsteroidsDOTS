using Unity.Entities;

namespace DOTS_Exercise.ECS.Components.Units
{
    public struct TeleportComponent : IComponentData
    {
        public bool InCameraBounds;
    }
}