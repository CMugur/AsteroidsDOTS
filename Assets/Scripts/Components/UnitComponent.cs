using DOTS_Exercise.Utils;
using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_Exercise.ECS.Components.Units
{
    public struct UnitComponent : IComponentData
    {
        public int ID;
        public UnitTypes UnitType;
        public float MovementSpeed;
        public float RotationSpeed;
        public float3 Direction;
        public int Health;
        public int Lives;
        public bool CanRotate;
        public int ScoreAddedOnDeath;
        public bool Invulnerability;
    }
}