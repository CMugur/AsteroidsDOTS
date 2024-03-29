using Unity.Mathematics;
using UnityEngine;

namespace DOTS_Exercise.Utils
{
    // Code taken from: https://www.patrykgalach.com/2019/07/08/unity-ecs-sprite-rendering/
    public static class SpriteHelper
    {
        public static Vector2 GetTextureOffset(Sprite sprite)
        {
            return Vector2.Scale(sprite.rect.position, new Vector2(1f / sprite.texture.width, 1f / sprite.texture.height));
        }

        public static Vector2 GetTextureSize(Sprite sprite)
        {
            return Vector2.Scale(sprite.rect.size, new Vector2(1f / sprite.texture.width, 1f / sprite.texture.height));
        }

        public static float3 GetQuadScale(Sprite sprite)
        {
            return new float3(sprite.rect.width / sprite.pixelsPerUnit, sprite.rect.height / sprite.pixelsPerUnit, 1);
        }
    }
}