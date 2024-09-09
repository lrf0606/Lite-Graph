using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;


namespace LiteGraphFrame
{
    static class NodeRuntimeGenerator
    {
        private const string ExecuteLogicStart = "        // === Execute Logic Start ===";
        private const string ExecuteLogicEnd = "        // === Execute Logic End ===";

        public static void Generate()
        {
            GenerateNodeRuntimeCode(true);
            GenerateNodeRuntimeCode(false);
        }

        private static string GetFilePath(bool isBuiltin, Type type)
        {
            string directory = isBuiltin ? LiteGraphFrameConfig.BuiltinNodeRuntimeDirectory : LiteGraphFrameConfig.CustomNodeRuntimeDirectory;
            return $"{directory}/{type.Name}.cs";
        }

        private static void GenerateNodeRuntimeCode(bool isBuiltin)
        {
            var baseType = typeof(NodeDataBase);
            var builtinNodeType = typeof(IBuiltinNode);
            var assembly = typeof(NodeRuntimeGenerator).Assembly;
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
                    var code = GetCodeString(isBuiltin, type);
                    var filePath = GetFilePath(isBuiltin, type);
                    if (File.Exists(filePath)) 
                    {
                        string source = File.ReadAllText(filePath);
                        code = CodeGenerateUtil.ReplaceStringByStartAndEnd(source, code, CodeGenerateUtil.GenerateStart, CodeGenerateUtil.GenerateEnd);
                    }
                    else
                    {
                        code = $"{CodeGenerateUtil.UsingNamespace}{Environment.NewLine}{code}"; // 加个using防止后续编辑器自动不全using到错误位置
                        
                    }
                    File.WriteAllText(filePath, code);
                }
            }
        }

        private static string GetCodeString(bool isBuiltin, Type type)
        {
            var code = new StringBuilder();
            string head = CodeGenerateUtil.GenerateStart;
            string tail = CodeGenerateUtil.GenerateEnd;
            code.AppendLine(head);
            code.AppendLine($"namespace {type.Namespace}");
            code.AppendLine("{");
            code.AppendLine($"    public partial class {type.Name} : {typeof(NodeRuntime).Name}");
            code.AppendLine("    {");
            var fieldCode = GetTypeFieldAndFucntionCode(type);
            if (!String.IsNullOrEmpty(fieldCode))
            {
                code.AppendLine(fieldCode);
            }
            if (type.IsSubclassOf(typeof(EventNodeData)))
            {
                code.AppendLine(GetTypeEventIdCode(type));
            }
            if (!type.IsSubclassOf(typeof(EventNodeData)))
            {
                code.AppendLine(GetTypeExecuteLogicCode(isBuiltin, type));
            }
            code.AppendLine("    }");
            code.AppendLine("}");

            code.Append(tail);

            return code.ToString();
        }
        
        private static string GetTypeFieldAndFucntionCode(Type type)
        {
            var code = new StringBuilder();
            var fields = new List<FieldInfo>();
            // fields
            foreach(var fieldInfo in type.GetFields())
            {
                if (!fieldInfo.IsPublic)
                {
                    continue;
                }
                var inputAttribute = fieldInfo.GetCustomAttribute<NodeInputAttribute>();
                if (inputAttribute == null)
                {
                    continue;
                }
                code.AppendLine($"        public {ValueParserUtil.GetFieldTypeTransform(fieldInfo.FieldType.Name)} {fieldInfo.Name};");
                fields.Add(fieldInfo);
            }

            foreach (var fieldInfo in type.GetFields())
            {
                if (!fieldInfo.IsPublic)
                {
                    continue;
                }
                var outputAttribute = fieldInfo.GetCustomAttribute<NodeOutputAttribute>();
                if (outputAttribute == null)
                {
                    continue;
                }
                code.AppendLine($"        public {ValueParserUtil.GetFieldTypeTransform(fieldInfo.FieldType.Name)} {fieldInfo.Name};");
                fields.Add(fieldInfo);
            }

            if (fields.Count == 0)
            {
                return code.ToString();
            }
            code.AppendLine();

            // functions
            code.AppendLine("        public override object GetValue(string fieldName)");
            code.AppendLine("        {");
            code.AppendLine("            switch (fieldName)");
            code.AppendLine("            {");
            foreach(var field in fields)
            {
                code.AppendLine($"                case \"{field.Name}\": return {field.Name};");
            }
            code.AppendLine("                default: return null;");
            code.AppendLine("            }");
            code.AppendLine("        }");
            code.AppendLine("        public override void SetValue(string fieldName, object value)");
            code.AppendLine("        {");
            code.AppendLine("            switch (fieldName)");
            code.AppendLine("            {");
            foreach (var field in fields)
            {
                code.AppendLine($"                case \"{field.Name}\": {{ {field.Name} = ({ValueParserUtil.GetFieldTypeTransform(field.FieldType.Name)})value; break; }};");
            }
            code.AppendLine("                default: break;");
            code.AppendLine("            }");
            code.AppendLine("        }");

            return code.ToString();
        }

        private static string GetTypeEventIdCode(Type type)
        {
          
            var code = new StringBuilder();
            var eventNode = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("GetEventId");
            int eventId = (int)methodInfo.Invoke(eventNode, null);
            code.AppendLine("        public override int GetEventId()");
            code.AppendLine("        {");
            code.AppendLine($"            return {eventId};");
            code.AppendLine("        }");
            return code.ToString();
        }

        private static string GetTypeExecuteLogicCode(bool isBuiltin, Type type)
        {
            var code = new StringBuilder();
            var filePath = GetFilePath(isBuiltin, type);
            string oldExecLogicCode = "";
            if (File.Exists(filePath))
            {
                string allCode = File.ReadAllText(filePath);
                oldExecLogicCode = CodeGenerateUtil.GetStringByStartAndEnd(allCode, ExecuteLogicStart, ExecuteLogicEnd);
            }
            if (string.IsNullOrEmpty(oldExecLogicCode))
            {
                code.AppendLine(ExecuteLogicStart);
                code.AppendLine("        public override void ExecuteLogic()");
                code.AppendLine("        {");
                code.AppendLine("            ");
                code.AppendLine("        }");
                code.AppendLine(ExecuteLogicEnd);
                return code.ToString();
            }
            else
            {
                return oldExecLogicCode;
            }
        }
    }

}
