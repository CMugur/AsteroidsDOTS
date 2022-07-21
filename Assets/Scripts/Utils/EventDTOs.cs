using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_Exercise.Utils
{
    public class BaseStrcturalChangesDTO
    {
        public EntityCommandBuffer? ECB;
    }

    public class SpawnUnitDTO : BaseStrcturalChangesDTO
    {
        public float3 Position;
        public float3 Direction;        
    }

    public class SpawnProjectileDTO : SpawnUnitDTO
    {
        public int WeaponID;        
    }

    public class SpawnPlayerDTO : SpawnUnitDTO
    {
        public int Lifes;
    }

    public class UnitDiedDTO : BaseStrcturalChangesDTO
    {
        public UnitComponent UnitComponent; 
    }

    public class UnitDiedWithPositionDTO : UnitDiedDTO
    {
        public Vector3 Position;
    }

    public class PowerupDestroyedWithPositionDTO : UnitDiedWithPositionDTO
    {
        public PowerupTagComponent PowerupTagComponent;
        public Entity OtherEntity;
    }
}