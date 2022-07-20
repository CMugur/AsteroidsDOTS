using DOTS_Exercise.ECS.Components.Units;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_Exercise.ECS.Systems.Units
{
    public class PlayerInputSystem : CustomSystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            Entities.ForEach((ref Rotation rotation, ref UnitComponent unitComponent, in PlayerTagComponent playerComponent) =>
            {
                int directionOfRotation = 0;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    directionOfRotation = 1;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    directionOfRotation = -1;
                }

                if (directionOfRotation != 0)
                {
                    Quaternion q = rotation.Value;
                    rotation.Value = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, q.eulerAngles.z + deltaTime * unitComponent.RotationSpeed * directionOfRotation);
                }

                float3 targetDirection;
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    targetDirection = math.mul(rotation.Value, new float3(0, 1, 0));
                }
                else
                {
                    targetDirection = float3.zero;
                }

                unitComponent.Direction = Vector3.Lerp(unitComponent.Direction, targetDirection, deltaTime);
            }).Run();
        }
    }
}