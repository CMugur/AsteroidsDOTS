using DOTS_Exercise.ECS.Systems;
using DOTS_Exercise.ECS.Systems.Units;
using DOTS_Exercise.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace DOTS_Exercise.Managers
{
    public class SystemsManager : ManagerBase
    {
        // Not ideal for a large scale app
        private readonly List<Type> _systemTypes = new List<Type>() 
        { 
            typeof(PlayerInputSystem),
            typeof(UnitMovementSystem),
            typeof(ShootingSystem),
            typeof(CollisionSystem),
            typeof(LifetimeSystem),
            typeof(TeleportSystem)
        };

        private List<CustomSystemBase> _systems = new List<CustomSystemBase>();

        public SystemsManager(Observer observer) : base(observer) { }

        public override void Initialize()
        {
            _systemTypes.ForEach(s => _systems.Add(World.DefaultGameObjectInjectionWorld.GetExistingSystem(s) as CustomSystemBase));
            _systems.ForEach(s => s.SetObserver(_observer));
        }

        protected void ToggleSystems(bool enabled, params CustomSystemBase[] systems)
        {
            systems.ToList().ForEach(s => s.Enabled = enabled);
        }
    }
}