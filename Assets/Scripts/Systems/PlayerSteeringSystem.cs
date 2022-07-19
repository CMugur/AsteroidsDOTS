using DOTS_Exercise.ECS.Components.Units;
using DOTS_Exercise.ECS.Components.Weapons;
using Unity.Entities;
using UnityEngine;

namespace DOTS_Exercise.ECS.Systems.Units
{
    public class PlayerSteeringSystem : CustomSystemBase
    {
        protected override void OnUpdate()
        {
            var horizontalAxis = Input.GetAxis("Horizontal");
            var verticalAxis = Input.GetAxis("Vertical");
            
            Entities.ForEach((ref UnitComponent unitComponent, in PlayerTagComponent playerComponent) =>
            {
                var direction = new Vector3(horizontalAxis, verticalAxis, 0);
                if (direction.magnitude > 1)
                {
                    direction = direction.normalized;
                }
                unitComponent.Direction = new Unity.Mathematics.float3(direction.x, direction.y, direction.z);
            }).Run();
        }
    }
}