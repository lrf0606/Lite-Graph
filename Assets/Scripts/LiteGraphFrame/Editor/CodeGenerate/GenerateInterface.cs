using System.Diagnostics;
using System.IO;

namespace LiteGraphFrame
{
    // 不进行代码生成的node继承此接口
    public interface INoGenerate
    {

    }

    public static class CodeGenerateConfig
    {
        public const string NodeTypeFactorPath = "Assets/LiteGraphFiles/NodeFactory.cs";
        public const string NodeRuntimeDirectory = "Assets/LiteGraphFiles/Nodes/";

        public const string GenerateStart = "// === LiteGraphFrame Code Generate Start ===";
        public const string GenerateEnd = "// === LiteGraphFrame Code Generate End ===";
        public const string ExecuteLogicStart = "        // === Execute Logic Start ===";
        public const string ExecuteLogicEnd = "        // === Execute Logic End ===";
        public const string UsingNamespace = "using System;";

        // 获取以target替换source中以start开头end结尾的内容
        public static string ReplaceStringByStartAndEnd(string source, string target, string start, string end)
        {
            int startIndex = source.IndexOf(start);
            if (startIndex < 0)
            {
                return source;
            }
            int endIndex = source.LastIndexOf(end);
            if (endIndex < 0)
            {
                return source;
            }
            string head = source[..startIndex];
            string tail = source[(endIndex + end.Length)..];
            return head + target + tail;
        }

        // 获取source中以start开头end结尾的内容
        public static string GetStringByStartAndEnd(string source, string start, string end, bool delStartAndEnd = false)
        {
            int startIndex = source.IndexOf(start);
            if (startIndex < 0)
            {
                return "";
            }
            int endIndex = source.LastIndexOf(end);
            if (endIndex < 0)
            {
                return "";
            }
            if (delStartAndEnd)
            {
                return source[(startIndex + start.Length)..endIndex];
            }
            else
            {
                return source[startIndex..(endIndex + end.Length)];
            }
        }
    }

}
