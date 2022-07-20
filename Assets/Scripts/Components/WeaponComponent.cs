using System;
using Unity.Entities;

namespace DOTS_Exercise.ECS.Components.Weapons
{
    public struct WeaponComponent : IComponentData
    {
        public int ID;
        public float CooldownSeconds;
        public DateTime LastShotTime;
        public bool CanShoot => (DateTime.Now - LastShotTime).TotalSeconds >= CooldownSeconds;
    }
}