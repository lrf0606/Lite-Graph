using System;
using System.IO;
using System.Text;

namespace LiteGraphFrame
{
    static class NodeTypeGenerator
    {
        public static void Generate()
        {
            GenerateNodeTypeCode(true);
            GenerateNodeTypeCode(false);
        }

        private static void GenerateNodeTypeCode(bool isBuiltin)
        {
            string directory = isBuiltin ? CodeGenerateConfig.BuiltinNodeRuntimeDirectory : CodeGenerateConfig.CustomNodeRuntimeDirectory;
            string path = $"{directory}/{CodeGenerateConfig.NodeFactoryFileName}";
            var code = CreateCodeString(isBuiltin);
            if (File.Exists(path))
            {
                var source = File.ReadAllText(path);
                code = CodeGenerateConfig.ReplaceStringByStartAndEnd(source, code, CodeGenerateConfig.GenerateStart, CodeGenerateConfig.GenerateEnd);
            }
            else
            {
                code = $"{CodeGenerateConfig.UsingNamespace}{Environment.NewLine}{code}"; // 加个using防止后续编辑器自动不全using到错误位置
            }
            File.WriteAllText(path, code);
        }

        private static string CreateCodeString(bool isBuiltin)
        {
            var code = new StringBuilder();
            string head = CodeGenerateConfig.GenerateStart;
            string tail = CodeGenerateConfig.GenerateEnd;
            string funcName = isBuiltin ? "InitBuiltinFactory" : "InitCustomFactory";
            var baseType = typeof(NodeDataBase);
            var builtinNodeType = typeof(IBuiltinNode);
            var assembly = typeof(NodeTypeGenerator).Assembly;
            code.AppendLine(head);
            code.AppendLine($"namespace {typeof(NodeTypeGenerator).Namespace}");
            code.AppendLine("{");
            code.AppendLine($"    public static partial class {typeof(LiteGraphNodeFactory).Name}");
            code.AppendLine("    {");
            code.AppendLine($"        public static void {funcName}()");
            code.AppendLine("        {");

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType))
                {
                    if (isBuiltin && !builtinNodeType.IsAssignableFrom(type))
                    {
                        continue;
                    }
                    if (!isBuiltin && builtinNodeType.IsAssignableFrom(type))
                    {
                        continue;
                    }
                    var typeName = type.Name;
                    code.AppendLine($"            RegisterCreateNodeFunc(\"{typeName}\", () => {{ return new {typeName}(); }});");
                }
            }

            code.AppendLine("        }");
            code.AppendLine("    }");
            code.AppendLine("}");
            code.Append(tail);
            return code.ToString();

        }
    }
}
