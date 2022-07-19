using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_Exercise.Utils
{
    public class SpawnUnitDTO
    {
        public float3 Position;
        public float3 Direction;
        public EntityCommandBuffer? ECB;
    }

    public class SpawnProjectileDTO : SpawnUnitDTO
    {
        public int WeaponID;        
    }
}