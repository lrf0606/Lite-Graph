using UnityEngine;

namespace LiteGraphFrame
{
    // 自定义拓展Parse部分
    public static partial class LiteGraphParseUtil
    {
        public static void CustomExtensionInit()
        {
            // vector2
            RegisterObject2String<Vector2>(Vector22String);
            RegisterString2Object<Vector2>(String2Vector2);
            // vector3
            RegisterObject2String<Vector3>(Vector32String);
            RegisterString2Object<Vector3>(String2Vector3);
        }

        static string Vector22String(Vector2 vec2)
        {
            return $"{vec2.x},{vec2.y}";
        }

        static Vector2 String2Vector2(string str)
        {
            string[] parts = str.Split(',');
            return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }

        static string Vector32String(Vector3 vec3)
        {
            return $"{vec3.x},{vec3.y},{vec3.z}";
        }

        static Vector3 String2Vector3(string str)
        {
            string[] parts = str.Split(',');
            return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
        }
    }
}