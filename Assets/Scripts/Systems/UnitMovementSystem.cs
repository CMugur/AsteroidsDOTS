using DOTS_Exercise.ECS.Components.Units;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_Exercise.ECS.Systems.Units
{
    public class UnitMovementSystem : CustomSystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            Entities.ForEach((ref UnitComponent unitComponent, ref Translation translation, ref Rotation rotation) =>
            {
                translation.Value += deltaTime * unitComponent.MovementSpeed * unitComponent.Direction;
                
                if (unitComponent.CanRotate && !unitComponent.Direction.Equals(Unity.Mathematics.float3.zero))
                { 
                    float angle = Mathf.Atan2((-1f) * unitComponent.Direction.x, unitComponent.Direction.y) * Mathf.Rad2Deg;
                    var endRotationValue = Quaternion.AngleAxis(angle, Vector3.forward);
                    var lerpRotationValue = Quaternion.Lerp(rotation.Value, endRotationValue, deltaTime * unitComponent.MovementSpeed);
                    rotation.Value = new Unity.Mathematics.quaternion(lerpRotationValue.x, lerpRotationValue.y, lerpRotationValue.z, lerpRotationValue.w);
                }
            }).Schedule();
        }
    }
}