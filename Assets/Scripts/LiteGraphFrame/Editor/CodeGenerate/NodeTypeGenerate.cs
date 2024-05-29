using System.IO;
using System.Text;

namespace LiteGraphFrame
{
    static class NodeTypeGenerator
    {
        public static void Generate()
        {
            string path = CodeGenerateConfig.NodeTypeFactorPath;
            var directory =  Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            { 
                Directory.CreateDirectory(directory);
            }

            var baseType = typeof(NodeDataBase);
            var noGenerateType = typeof(INoGenerate);
            var registerCode = new StringBuilder();
            var assembly = typeof(NodeTypeGenerator).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType) && !noGenerateType.IsAssignableFrom(type))
                {
                    var typeName = type.Name;
                    registerCode.AppendLine($"            RegisterCreateNodeFunc(\"{typeName}\", () => {{ return new {typeName}(); }});");
                }
            }

            var code = GetCodeString(registerCode.ToString());
            if (File.Exists(path))
            {
                var source = File.ReadAllText(path);
                code = CodeGenerateConfig.ReplaceStringByStartAndEnd(source, code, CodeGenerateConfig.GenerateStart, CodeGenerateConfig.GenerateEnd);
            }
            else
            {
                code = CodeGenerateConfig.UsingNamespace + code; // 加个using防止后续编辑器自动不全using到错误位置
            }
            File.WriteAllText(path, code);
        }

        private static string GetCodeString(string registerCode)
        {
            var code = new StringBuilder();
            string head = CodeGenerateConfig.GenerateStart;
            string tail = CodeGenerateConfig.GenerateEnd;
            code.AppendLine(head);
            code.AppendLine($"namespace {typeof(NodeTypeGenerator).Namespace}");
            code.AppendLine("{");
            code.AppendLine($"    public static partial class {typeof(LiteGraphNodeFactory).Name}");
            code.AppendLine("    {");
            code.AppendLine("        public static void InitCreateFuncDict()");
            code.AppendLine("        {");
            code.AppendLine(registerCode);
            code.AppendLine("        }");
            code.AppendLine("    }");
            code.AppendLine("}");
            code.AppendLine(tail);
            return code.ToString();

        }
    }
}
