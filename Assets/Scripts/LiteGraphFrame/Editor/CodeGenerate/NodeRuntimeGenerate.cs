using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;


namespace LiteGraphFrame
{
    static class NodeRuntimeGenerator
    {
        public static void Generate()
        {
            string directory = CodeGenerateConfig.NodeRuntimeDirectory;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var baseType = typeof(NodeDataBase);

            var noGenerateType = typeof(INoGenerate);
            var assembly = typeof(NodeRuntimeGenerator).Assembly;
            foreach (var type in assembly.GetTypes()) 
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType) && !noGenerateType.IsAssignableFrom(type))
                {
                    var code = GetCodeString(type);
                    string filePath = $"{directory}/{type.Name}.cs";
                    if (File.Exists(filePath)) 
                    {
                        string source = File.ReadAllText(filePath);
                        code = CodeGenerateConfig.ReplaceStringByStartAndEnd(source, code, CodeGenerateConfig.GenerateStart, CodeGenerateConfig.GenerateEnd);
                    }
                    else
                    {
                        code = $"{CodeGenerateConfig.UsingNamespace}{Environment.NewLine}{code}"; // 加个using防止后续编辑器自动不全using到错误位置
                        
                    }
                    File.WriteAllText(filePath, code);
                }
            }
        }

        private static string GetCodeString(Type type)
        {
            var code = new StringBuilder();
            string head = CodeGenerateConfig.GenerateStart;
            string tail = CodeGenerateConfig.GenerateEnd;
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
                code.AppendLine(GetTypeExecuteLogicCode(type));
            }
            code.AppendLine("    }");
            code.AppendLine("}");

            code.AppendLine(tail);

            return code.ToString();
        }
        
        private static string GetTypeFieldAndFucntionCode(Type type)
        {
            var code = new StringBuilder();
            var fields = new List<FieldInfo>();
            // fields
            foreach(var fieldInfo in type.GetFields())
            {
                var inputAttribute = fieldInfo.GetCustomAttribute<NodeInputAttribute>();
                if (inputAttribute == null)
                {
                    continue;
                }
                code.AppendLine($"        public {fieldInfo.FieldType} {fieldInfo.Name};");
                fields.Add(fieldInfo);
            }

            foreach (var fieldInfo in type.GetFields())
            {
                var outputAttribute = fieldInfo.GetCustomAttribute<NodeOutputAttribute>();
                if (outputAttribute == null)
                {
                    continue;
                }
                code.AppendLine($"        public {fieldInfo.FieldType} {fieldInfo.Name};");
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
                code.AppendLine($"                case \"{field.Name}\": {{ {field.Name} = ({field.FieldType})value; break; }};");
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

        private static string GetTypeExecuteLogicCode(Type type)
        {
            var code = new StringBuilder();
            string filePath = $"{CodeGenerateConfig.NodeRuntimeDirectory}/{type.Name}.cs";
            string oldExecLogicCode = "";
            if (File.Exists(filePath))
            {
                string allCode = File.ReadAllText(filePath);
                oldExecLogicCode = CodeGenerateConfig.GetStringByStartAndEnd(allCode, CodeGenerateConfig.ExecuteLogicStart, CodeGenerateConfig.ExecuteLogicEnd);
            }
            if (string.IsNullOrEmpty(oldExecLogicCode))
            {
                code.AppendLine(CodeGenerateConfig.ExecuteLogicStart);
                code.AppendLine("        public override void ExecuteLogic()");
                code.AppendLine("        {");
                code.AppendLine("            ");
                code.AppendLine("        }");
                code.AppendLine(CodeGenerateConfig.ExecuteLogicEnd);
                return code.ToString();
            }
            else
            {
                return oldExecLogicCode;
            }
        }
    }

}
