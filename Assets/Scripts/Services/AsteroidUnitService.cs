using DOTS_Exercise.Data.Units;
using DOTS_Exercise.Utils;
using System;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_Exercise.Services
{
    public class AsteroidUnitService : UnitService
    {
        private AsteroidWaveScriptableObject _currentWave;
        private int _asteroidsDiedThisWave = 0;

        public AsteroidUnitService(Func<UnitScriptableObject, SpawnUnitDTO, Entity> unitFactory) : base(unitFactory) { }

        public override void SpawnUnit(SpawnUnitDTO dto = null)
        {
            SpawnNextWave();
        }

        private void SpawnNextWave()
        {
            if (_currentWave == null)
            {
                _currentWave = UnitSettings.Waves.FirstOrDefault();
            }
            else
            {
                _currentWave = UnitSettings.GetWave(_currentWave.NextWaveID);
            }

            _asteroidsDiedThisWave = 0;
            SpawnWave(_currentWave);
        }

        private void SpawnWave(AsteroidWaveScriptableObject wave)
        {
            if (wave == null)
            {
                return;
            }

            var rnd = new Unity.Mathematics.Random((uint)System.DateTime.UtcNow.Ticks);
            wave.Asteroids.ForEach(u => _unitFactory(u, GetSpawnUnitDTO(wave, ref rnd)));
        }

        private SpawnUnitDTO GetSpawnUnitDTO(AsteroidWaveScriptableObject wave, ref Unity.Mathematics.Random rnd)
        {
            var minPosition = new float3((Camera.main.orthographicSize - wave.SpawnAreaOffsetFromCameraSize) * -1f, 
                (Camera.main.orthographicSize - wave.SpawnAreaOffsetFromCameraSize) * -1f, 0);
            var maxPosition = new float3((Camera.main.orthographicSize - wave.SpawnAreaOffsetFromCameraSize), 
                (Camera.main.orthographicSize - wave.SpawnAreaOffsetFromCameraSize), 0);

            var dto = new SpawnUnitDTO()
            {
                Position = rnd.NextFloat3(minPosition, maxPosition),
                Direction = rnd.NextFloat3(new float3(-1, -1, 0), new float3(1, 1, 0))
            };

            while (math.abs(dto.Position.x) < wave.PlayerSafeRadius)
            {
                dto.Position.x += wave.PlayerSafeRadius;
            }

            while (math.abs(dto.Position.y) < wave.PlayerSafeRadius)
            {
                dto.Position.y += wave.PlayerSafeRadius;
            }

            return dto;
        }

        public override void OnUnitDestroyed(UnitDiedWithPositionDTO dto)
        {
            Debug.Log(dto.Position);

            _asteroidsDiedThisWave++;
            if (_asteroidsDiedThisWave == _currentWave.TotalAsteroids)
            {
                OnWaveCleared();
                return;
            }

            AsteroidUnitScriptableObject unit = UnitSettings.Waves.FirstOrDefault().Asteroids.FirstOrDefault(a => a.ID == dto.UnitComponent.ID);
            if (unit == null)
            {
                return;
            }

            var rnd = new Unity.Mathematics.Random((uint)System.DateTime.UtcNow.Ticks);
            unit.AsteroidsToSpawnOnDeath.ForEach(a => _unitFactory(a,
                new SpawnUnitDTO()
                {
                    Position = rnd.NextFloat3(new float3(-0.3f, -0.3f, 0), new float3(0.3f, 0.3f, 0)) + (float3)dto.Position,
                    Direction = rnd.NextFloat3(new float3(-1, -1, 0), new float3(1, 1, 0)),
                    ECB = dto.ECB
                }));
        }

        private void OnWaveCleared()
        {
            SpawnNextWave();
        }
    }
}