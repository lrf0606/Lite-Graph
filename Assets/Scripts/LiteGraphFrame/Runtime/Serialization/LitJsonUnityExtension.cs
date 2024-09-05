using System;
using System.Collections.Generic;

namespace LitJson
{
    public delegate string Object2StringDelegate(object obj);
    public delegate string Object2StringDelegate<T>(T obj);
    public delegate object String2ObjectDelegate(string str);
    public delegate T String2ObjectDelegate<T>(string str);

    public static class ValuePraseUtil
    {
        private static Dictionary<string, Object2StringDelegate> m_ParseObject2StringDict;
        private static Dictionary<string, String2ObjectDelegate> m_ParseString2ObjectDict;

        static ValuePraseUtil()
        {
            m_ParseObject2StringDict = new Dictionary<string, Object2StringDelegate>();
            m_ParseString2ObjectDict = new Dictionary<string, String2ObjectDelegate>();

            RegisterToString();
            RegisterToObject();
        }

        private static void RegisterToString()
        {
            // int
            RegisterObject2String((int val) => { return val.ToString(); });
            // float
            RegisterObject2String((float val) => { return val.ToString(); });
            // double
            RegisterObject2String((double val) => { return val.ToString(); });
            // string
            RegisterObject2String((string val) => { return string.IsNullOrEmpty(val) ? "" : val; });
            // bool
            RegisterObject2String((bool val) => { return val.ToString(); });
            // vector2
            RegisterObject2String((UnityEngine.Vector2 vec2) => { return $"{vec2.x},{vec2.y}"; });
            // vector3
            RegisterObject2String((UnityEngine.Vector3 vec3) => { return $"{vec3.x},{vec3.y},{vec3.z}"; });
        }

        private static void RegisterToObject()
        {
            // int
            RegisterString2Object("Int32", (string str) => { return int.Parse(str); });
            // float
            RegisterString2Object("Single", (string str) => { return float.Parse(str); });
            // double
            RegisterString2Object("Double", (string str) => { return double.Parse(str); });
            // string
            RegisterString2Object("String", (string str) => { return str; });
            // bool
            RegisterString2Object("Boolean", (string str) => { return bool.Parse(str); });
            // vector2
            RegisterString2Object("Vector2", (string str) => {
                string[] parts = str.Split(',');
                return new UnityEngine.Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            });
            // vector3
            RegisterString2Object("Vector3", (string str) => {
                string[] parts = str.Split(',');
                return new UnityEngine.Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            });
        }

        public static void RegisterObject2String<T>(Object2StringDelegate<T> func)
        {
            m_ParseObject2StringDict[typeof(T).Name] = (object obj) => { return func((T)obj); };
        }

        public static void RegisterString2Object<T>(String2ObjectDelegate<T> func)
        {
            m_ParseString2ObjectDict[typeof(T).Name] = (string str) => { return func(str); };
        }

        public static void RegisterObject2String(string type, Object2StringDelegate func)
        {
            m_ParseObject2StringDict[type] = func;
        }

        public static void RegisterString2Object(string type, String2ObjectDelegate func)
        {
            m_ParseString2ObjectDict[type] = func;
        }

        public static string ToString(string type, object obj)
        {
            if (m_ParseObject2StringDict.TryGetValue(type, out var func))
            {
                return func(obj);
            }
            else
            {
                throw new Exception($"ParseUtil.ToString: {type} is not register");
            }
        }

        public static object ToObject(string type, string str)
        {
            if (m_ParseString2ObjectDict.TryGetValue(type, out var func))
            {
                return func(str);
            }
            else
            {
                throw new Exception($"ParseUtil.ToObject: {type} is not register");
            }
        }
    }
}
