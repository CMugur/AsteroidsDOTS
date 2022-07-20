using DOTS_Exercise.ECS.Components.Units;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_Exercise.ECS.Systems.Units
{
    public class TeleportSystem : CustomSystemBase
    {
        private Bounds _cameraWorldPositionBounds;

        protected override void OnCreate()
        {
            CalculateCameraWorldPositionBounds();
        }

        private void CalculateCameraWorldPositionBounds()
        {
            _cameraWorldPositionBounds = new Bounds(Camera.main.transform.position,
                new Vector3(Camera.main.aspect * Camera.main.orthographicSize * 2f, Camera.main.orthographicSize * 2f, Camera.main.farClipPlane + Camera.main.transform.position.z));
        }

        protected override void OnUpdate()
        {
            var bounds = _cameraWorldPositionBounds;
            Entities.ForEach((ref Translation translation, ref TeleportComponent teleportComponent) => 
            {
                if (bounds.Contains(translation.Value) && !teleportComponent.InCameraBounds)
                {
                    teleportComponent.InCameraBounds = true;
                }
                else if (!bounds.Contains(translation.Value) && teleportComponent.InCameraBounds)
                {
                    teleportComponent.InCameraBounds = false;
                    if (math.abs(translation.Value.x) > bounds.extents.x)
                    {
                        translation.Value.x *= -1f;
                    }
                    else if (math.abs(translation.Value.y) > bounds.extents.y)
                    {
                        translation.Value.y *= -1f;
                    }
                }
            }).Run();
        }
    }
}