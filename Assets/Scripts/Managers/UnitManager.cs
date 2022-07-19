using DOTS_Exercise.Data.Settings;
using DOTS_Exercise.Data.Settings.Interfaces;
using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Data.Weapons;
using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using DOTS_Exercise.Utils;
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
                    typeof(Rotation)
                };

                return _unitArchetypeComponents;
            }
        }
        
        public UnitManager(Observer observer) : base(observer) { }

        public override void Initialize()
        {
            SpawnPlayer(UnitSettings.Player);
            SpawnWave(UnitSettings.Waves.FirstOrDefault());

            _observer.AddRequest(Events.Request_SpawnProjectile, SpawnProjectile);
        }

        private void SpawnPlayer(PlayerUnitScriptableObject player)
        {
            if (player == null)
            {
                return;
            }

            var playerEntity = SpawnUnit(
                player, 
                new SpawnUnitDTO() { Direction = float3.zero, Position = float3.zero }, 
                typeof(PlayerTagComponent), 
                typeof(WeaponComponent));
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            entityManager.SetComponentData(playerEntity, new WeaponComponent() { ID = player.DefaultWeapon.ID });
        }

        private void SpawnWave(UnitWaveScriptableObject wave)
        {
            if (wave == null)
            {
                return;
            }

            var rnd = new Unity.Mathematics.Random((uint)System.DateTime.UtcNow.Ticks);
            wave.Units.ForEach(u => SpawnUnit(u,
                new SpawnUnitDTO()
                {
                    Position = rnd.NextFloat3(new float3(-5, -3, 0), new float3(5, 3, 0)),
                    Direction = rnd.NextFloat3(new float3(-1, -1, 0), new float3(1, 1, 0))
                }
            ));
        }

        private void SpawnProjectile(object arg)
        {
            if (!(arg is SpawnProjectileDTO dto))
            {
                return;
            }

            WeaponScriptableObject weapon = UnitSettings.GetWeapon(dto.WeaponID);
            if (weapon == null)
            {
                Debug.LogError($"No weapon found for this id: {dto.WeaponID}");
                return;
            }

            SpawnUnit(weapon.Projectile, dto);
        }

        private Entity SpawnUnit(UnitScriptableObject unit, SpawnUnitDTO dto, params ComponentType[] additionalComponents)
        {
            if (dto.ECB.HasValue)
            {
                return SpawnUnitWithECB(unit, dto, additionalComponents);                
            }

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var unitEntity = entityManager.CreateEntity(GetEntityArchetype(UnitArchetypeComponents.Concat(additionalComponents).ToArray()));

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
                MovementSpeed = unit.Speed, 
                CanRotate = unit.CanRotate 
            });

            return unitEntity;
        }

        private Entity SpawnUnitWithECB(UnitScriptableObject unit, SpawnUnitDTO dto, params ComponentType[] additionalComponents)
        {
            if (!dto.ECB.HasValue)
            {
                return SpawnUnit(unit, dto, additionalComponents);
            }

            var unitEntity = dto.ECB.Value.CreateEntity(GetEntityArchetype(UnitArchetypeComponents.Concat(additionalComponents).ToArray()));

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
                MovementSpeed = unit.Speed,
                CanRotate = unit.CanRotate
            });

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
    }
}