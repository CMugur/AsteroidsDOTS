using DOTS_Exercise.Data.Settings;
using DOTS_Exercise.Data.Settings.Interfaces;
using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Data.Weapons;
using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using DOTS_Exercise.Services;
using DOTS_Exercise.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_Exercise.Managers
{
    public class UnitManager : ManagerBase, IRequestRenderingSettings, IRequestUnitSettings
    {
        public RenderingSettingsScriptableObject RenderingSettings { get; set; }
        public UnitSettingsScriptableObject UnitSettings { get; set; }

        public void SetRenderingSettings(RenderingSettingsScriptableObject settings)
        {
            RenderingSettings = settings;
        }

        public void SetUnitSettings(UnitSettingsScriptableObject settings)
        {
            UnitSettings = settings;
        }

        // Workaournd for issues with the ECS. Having a readonly field would cause a null ref error in the editor.
        private ComponentType[] _unitArchetypeComponents;
        private ComponentType[] UnitArchetypeComponents
        {
            get
            {
                if (_unitArchetypeComponents != null)
                {
                    return _unitArchetypeComponents;
                }

                _unitArchetypeComponents = new ComponentType[]
                {
                    typeof(RenderMesh),
                    typeof(LocalToWorld),
                    typeof(Translation),
                    typeof(NonUniformScale),
                    typeof(RenderBounds),
                    typeof(UnitComponent),
                    typeof(Rotation),
                    typeof(TeleportComponent)
                };

                return _unitArchetypeComponents;
            }
        }

        // Mapping all the possible Archetypes when the manager is initialized because of issues with creating archetypes while using the ECB
        private Dictionary<UnitTypes, EntityArchetype> _unitArchetypes = new Dictionary<UnitTypes, EntityArchetype>();

        // Services
        private Dictionary<UnitTypes, UnitService> _unitsServices;

        public UnitManager(Observer observer) : base(observer) { }

        public override void Initialize()
        {
            SetUnitServices();
            SetUnitArchetypes();

            _observer.AddRequest(Events.Request_SpawnPlayerProjectile, (arg) => _unitsServices[UnitTypes.PlayerProjectile].SpawnUnit(arg as SpawnProjectileDTO));
            _observer.AddRequest(Events.Request_SpawnUFOProjectile, (arg) => _unitsServices[UnitTypes.UFOProjectile].SpawnUnit(arg as SpawnProjectileDTO));
            _observer.AddListener(Events.Trigger_OnGameStart, OnGameStart);
        }

        private void SetUnitServices()
        {
            _unitsServices = new Dictionary<UnitTypes, UnitService>
            {
                { UnitTypes.Player, new PlayerUnitService(SpawnUnit) },
                { UnitTypes.Asteroid, new AsteroidUnitService(SpawnUnit) },
                { UnitTypes.PlayerProjectile, new ProjectileUnitService(SpawnUnit) },
                { UnitTypes.UFO, new UFOUnitService(SpawnUnit) },
                { UnitTypes.UFOProjectile, new ProjectileUnitService(SpawnUnit) }
            };

            _unitsServices[UnitTypes.Player].SetUnitSettings(UnitSettings);
            _unitsServices[UnitTypes.Asteroid].SetUnitSettings(UnitSettings);
            _unitsServices[UnitTypes.PlayerProjectile].SetUnitSettings(UnitSettings);
            _unitsServices[UnitTypes.UFO].SetUnitSettings(UnitSettings);
            _unitsServices[UnitTypes.UFOProjectile].SetUnitSettings(UnitSettings);
        }

        private void SetUnitArchetypes()
        {
            _unitArchetypes.Add(UnitTypes.Unit, GetEntityArchetype(UnitArchetypeComponents));
            _unitArchetypes.Add(UnitTypes.Asteroid, GetEntityArchetype(UnitArchetypeComponents));
            var additionalPlayerComponentTypes = new ComponentType[] { typeof(PlayerTagComponent), typeof(WeaponComponent), typeof(InvulnerabilityComponent) };
            _unitArchetypes.Add(UnitTypes.Player, GetEntityArchetype(UnitArchetypeComponents.Concat(additionalPlayerComponentTypes).ToArray()));
            var additionalPlayerProjectileComponentTypes = new ComponentType[] { typeof(LifetimeComponent) };
            _unitArchetypes.Add(UnitTypes.PlayerProjectile, GetEntityArchetype(UnitArchetypeComponents.Concat(additionalPlayerProjectileComponentTypes).ToArray()));
            var additionalUFOComponentTypes = new ComponentType[] { typeof(WeaponComponent), typeof(UFOTagComponent) };
            _unitArchetypes.Add(UnitTypes.UFO, GetEntityArchetype(UnitArchetypeComponents.Concat(additionalUFOComponentTypes).ToArray()));
            var additionalUFOProjectileComponentTypes = new ComponentType[] { typeof(LifetimeComponent) };
            _unitArchetypes.Add(UnitTypes.UFOProjectile, GetEntityArchetype(UnitArchetypeComponents.Concat(additionalUFOProjectileComponentTypes).ToArray()));
        }

        #region Events
        private void OnGameStart(object arg)
        {
            _observer.RemoveListener(Events.Trigger_OnGameStart, OnGameStart);

            _observer.AddListener(Events.Trigger_OnUnitDied, OnUnitDied);
            _observer.AddListener(Events.Trigger_OnGameEnded, OnGameEnded);

            _unitsServices[UnitTypes.Player].SpawnUnit();
            _unitsServices[UnitTypes.Asteroid].SpawnUnit();
            _unitsServices[UnitTypes.UFO].SpawnUnit();
        }

        private void OnUnitDied(object args)
        {
            UnitDiedWithPositionDTO dto = (UnitDiedWithPositionDTO)args;
            if (_unitsServices.ContainsKey(dto.UnitComponent.UnitType))
            {
                _unitsServices[dto.UnitComponent.UnitType].OnUnitDestroyed(dto);
            }
        }

        private void OnGameEnded(object arg0)
        {
            _observer.RemoveListener(Events.Trigger_OnUnitDied, OnUnitDied);
            _observer.RemoveListener(Events.Trigger_OnGameEnded, OnGameEnded);
        }
        #endregion

        private Entity SpawnUnit(UnitScriptableObject unit, SpawnUnitDTO dto)
        {
            if (!_unitArchetypes.ContainsKey(unit.UnitType))
            {
                Debug.LogWarning($"No archetype found for unit type: {unit.UnitType}");
                return default;
            }

            if (!dto.ECB.HasValue)
            {
                return SpawnUnitWithEM(unit, dto);
            }
            else
            {
                return SpawnUnitWithECB(unit, dto);
            }
        }

        private Entity SpawnUnitWithEM(UnitScriptableObject unit, SpawnUnitDTO dto)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var unitEntity = entityManager.CreateEntity(_unitArchetypes[unit.UnitType]);

            entityManager.SetSharedComponentData(unitEntity, new RenderMesh 
            { 
                mesh = RenderingSettings.UnitMesh, 
                material = GetUnitMaterial(unit) 
            });
            entityManager.SetComponentData(unitEntity, new NonUniformScale { Value = SpriteHelper.GetQuadScale(unit.Sprite) });
            entityManager.SetComponentData(unitEntity, new Translation { Value = dto.Position });
            entityManager.SetComponentData(unitEntity, new UnitComponent 
            { 
                Direction = dto.Direction, 
                Health = unit.Health,
                Lives = unit.Lives, 
                MovementSpeed = unit.MovementSpeed,
                RotationSpeed = unit.RotationSpeed,
                CanRotate = unit.CanRotate,
                UnitType = unit.UnitType,
                ScoreAddedOnDeath = unit.ScoreAddedOnDeath,
                ID = unit.ID
            });
            entityManager.SetComponentData(unitEntity, new RenderBounds() { Value = new AABB() { Center = float3.zero, Extents = SpriteHelper.GetQuadScale(unit.Sprite) / 2f } });

            return unitEntity;
        }

        private Entity SpawnUnitWithECB(UnitScriptableObject unit, SpawnUnitDTO dto)
        {
            var unitEntity = dto.ECB.Value.CreateEntity(_unitArchetypes[unit.UnitType]);

            dto.ECB.Value.SetSharedComponent(unitEntity, new RenderMesh
            {
                mesh = RenderingSettings.UnitMesh,
                material = GetUnitMaterial(unit)
            });
            dto.ECB.Value.SetComponent(unitEntity, new NonUniformScale { Value = SpriteHelper.GetQuadScale(unit.Sprite) });
            dto.ECB.Value.SetComponent(unitEntity, new Translation { Value = dto.Position });
            dto.ECB.Value.SetComponent(unitEntity, new UnitComponent
            {
                Direction = dto.Direction,
                Health = unit.Health,
                Lives = unit.Lives,
                MovementSpeed = unit.MovementSpeed,
                RotationSpeed = unit.RotationSpeed,
                CanRotate = unit.CanRotate,
                UnitType = unit.UnitType,
                ScoreAddedOnDeath = unit.ScoreAddedOnDeath,
                ID = unit.ID
            });
            dto.ECB.Value.SetComponent(unitEntity, new RenderBounds() { Value = new AABB() { Center = float3.zero, Extents = SpriteHelper.GetQuadScale(unit.Sprite) / 2f } });

            return unitEntity;
        }

        private Material GetUnitMaterial(UnitScriptableObject unit)
        {
            Material unitMaterial = Material.Instantiate(RenderingSettings.UnitMaterial);
            unitMaterial.mainTextureOffset = SpriteHelper.GetTextureOffset(unit.Sprite);
            unitMaterial.mainTextureScale = SpriteHelper.GetTextureSize(unit.Sprite);
            return unitMaterial;
        }

        private EntityArchetype GetEntityArchetype(ComponentType[] components)
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.CreateArchetype(components);
        }

        public override void Dispose()
        {
            base.Dispose();
            _unitArchetypes.Clear();
            _unitsServices.Clear();
        }
    }
}