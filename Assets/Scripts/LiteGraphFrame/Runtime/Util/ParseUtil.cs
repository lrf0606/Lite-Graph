using System;
using System.Collections.Generic;

namespace LiteGraphFrame
{
    static class ParseRuntimeUtil
    {
        internal delegate object String2ObjectDelegate(string str);
        private static Dictionary<string, String2ObjectDelegate> ParseString2ObjectDict;

        static ParseRuntimeUtil()
        {
            ParseString2ObjectDict = new Dictionary<string, String2ObjectDelegate>();
            // TODO 这里和Editor中的ParseUtil改为可拓展形式
            RegisterString2Object("Int32", (string str) => { return (object)String2Int(str); });
            RegisterString2Object("Single", (string str) => { return (object)String2Float(str); });
            RegisterString2Object("Double", (string str) => { return (object)String2Double(str); });
            RegisterString2Object("String", (string str) => { return (object)str; });
            RegisterString2Object("Boolean", (string str) => { return (object)String2Bool(str); });
            RegisterString2Object("Vector2", (string str) => { return (object)String2Vector2(str); });
            RegisterString2Object("Vector3", (string str) => { return (object)String2Vector3(str); });
        }

        public static void RegisterString2Object(string type, String2ObjectDelegate func)
        {
            ParseString2ObjectDict[type] = func;
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

        static int String2Int(string val)
        {
            return int.Parse(val);
        }

        static float String2Float(string str)
        {
            return float.Parse(str);
        }

        static double String2Double(string str)
        {
            return double.Parse(str);
        }

        static bool String2Bool(string str)
        {
            return bool.Parse(str);
        }

        static UnityEngine.Vector2 String2Vector2(string str)
        {
            string[] parts = str.Split(',');
            return new UnityEngine.Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }
        static UnityEngine.Vector3 String2Vector3(string str)
        {
            string[] parts = str.Split(',');
            return new UnityEngine.Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
        }
    }
}
