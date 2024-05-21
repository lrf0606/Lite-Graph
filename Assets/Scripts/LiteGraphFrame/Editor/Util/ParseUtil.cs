using System;
using System.Collections.Generic;
using UnityEngine;


namespace LiteGraphFrame
{
    public static partial class LiteGraphParseEditorUtil
    {
        internal delegate string Object2StringDelegate(object obj);
        public delegate string Object2StringDelegate<T>(T obj);
        internal delegate object String2ObjectDelegate(string str);
        public delegate T String2ObjectDelegate<T>(string str);

        private static Dictionary<string, Object2StringDelegate> ParseObject2StringDict;
        private static Dictionary<string, String2ObjectDelegate> ParseString2ObjectDict;

        public static void RegisterObject2String<T>(Object2StringDelegate<T> func)
        {
            ParseObject2StringDict[typeof(T).Name] = (object obj) => { return func((T)obj); };
        }

        public static void RegisterString2Object<T>(String2ObjectDelegate<T> func)
        {
            ParseString2ObjectDict[typeof(T).Name] = (string str) => { return func(str); };
        }

        public static string ToString(string type, object obj)
        {
            if (ParseObject2StringDict.TryGetValue(type, out var func))
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
            if (ParseString2ObjectDict.TryGetValue(type, out var func))
            {
                return func(str);
            }
            else
            {
                throw new Exception($"ParseUtil.ToObject: {type} is not register");
            }
        }

        static LiteGraphParseEditorUtil()
        {
            ParseObject2StringDict = new Dictionary<string, Object2StringDelegate>();
            ParseString2ObjectDict = new Dictionary<string, String2ObjectDelegate>();
            // int
            RegisterObject2String<int>(Int2String);
            RegisterString2Object<int>(String2Int);
            // float 
            RegisterObject2String<float>(Float2String);
            RegisterString2Object<float>(String2Float);
            // doule 
            RegisterObject2String<double>(Double2String);
            RegisterString2Object<double>(String2Double);
            // string
            RegisterObject2String<string>(String2String);
            RegisterString2Object<string>(String2String);
            // bool
            RegisterObject2String<bool>(Bool2String);
            RegisterString2Object<bool>(String2Bool);
            // vector2
            RegisterObject2String<Vector2>(Vector22String);
            RegisterString2Object<Vector2>(String2Vector2);
            // vector3
            RegisterObject2String<Vector3>(Vector32String);
            RegisterString2Object<Vector3>(String2Vector3);
        }

        static string Int2String(int val)
        {
            return val.ToString();
        }

        static int String2Int(string val)
        {
            return int.Parse(val);
        }

        static string Float2String(float val)
        {
            return val.ToString();
        }

        static float String2Float(string str)
        {
            return float.Parse(str);
        }

        static string Double2String(double val)
        {
            return val.ToString();
        }

        static double String2Double(string str)
        {
            return double.Parse(str);
        }

        static string String2String(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return str;
        }

        static string Bool2String(bool val)
        {
            return val.ToString();
        }

        static bool String2Bool(string str)
        {
            return bool.Parse(str);
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