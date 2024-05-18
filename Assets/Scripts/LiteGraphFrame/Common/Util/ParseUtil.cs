using System;
using System.Collections.Generic;


namespace LiteGraphFrame
{
    internal delegate string Object2StringDelegate(object obj);
    public delegate string Object2StringDelegate<T>(T obj);
    internal delegate object String2ObjectDelegate(string str);
    public delegate T String2ObjectDelegate<T>(string str);

    // 可在另一个partial class LiteGraphParseUtil的进行自定义类型拓展
    public static partial class LiteGraphParseUtil
    {
        private static Dictionary<string, Object2StringDelegate> ParseObject2StringDict;
        private static Dictionary<string, String2ObjectDelegate> ParseString2ObjectDict;

        public static void RegisterObject2String<T>(Object2StringDelegate<T> func)
        {
            ParseObject2StringDict[typeof(T).FullName] = (object obj) => { return func((T)obj); };
        }

        public static void RegisterString2Object<T>(String2ObjectDelegate<T> func)
        {
            ParseString2ObjectDict[typeof(T).FullName] = (string str) => { return func(str); };
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

        static LiteGraphParseUtil()
        {
            ParseObject2StringDict = new Dictionary<string, Object2StringDelegate>();
            ParseString2ObjectDict = new Dictionary<string, String2ObjectDelegate>();
            // int
            RegisterObject2String<int>(Int2String);
            RegisterString2Object<int>(String2Int);
            // float 
            RegisterObject2String<float>(Float2String);
            RegisterString2Object<float>(String2Float);
            // string
            RegisterObject2String<string>(String2String);
            RegisterString2Object<string>(String2String);
            // bool
            RegisterObject2String<bool>(Bool2String);
            RegisterString2Object<bool>(String2Bool);
            // custom Extension
            CustomExtensionInit();
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
    }



}
