using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

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
                        code = CodeGenerateConfig.UsingNamespace + code; // 加个using防止后续编辑器自动不全using到错误位置
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
            code.Append(head);
            code.Append($"namespace {type.Namespace}\r\n");
            code.Append("{\r\n");
            code.Append($"    public partial class {type.Name} : {typeof(NodeRuntime).Name}\r\n");
            code.Append("    {\r\n");
            code.Append(GetTypeFieldAndFucntionCode(type));
            code.Append(GetTypeEventIdCode(type));
            code.Append(GetTypeExecuteLogicCode(type));
            code.Append("    }\r\n");
            code.Append("}\r\n");

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
                var inputAttribute = fieldInfo.GetCustomAttribute<NodeInputAttribute>();
                if (inputAttribute == null)
                {
                    continue;
                }
                code.Append($"        public {fieldInfo.FieldType} {fieldInfo.Name};\r\n");
                fields.Add(fieldInfo);
            }

            foreach (var fieldInfo in type.GetFields())
            {
                var outputAttribute = fieldInfo.GetCustomAttribute<NodeOutputAttribute>();
                if (outputAttribute == null)
                {
                    continue;
                }
                code.Append($"        public {fieldInfo.FieldType} {fieldInfo.Name};\r\n");
                fields.Add(fieldInfo);
            }

            code.Append("\r\n");
            
            if (fields.Count == 0)
            {
                return code.ToString();
            }

            // functions
            code.Append("        public override object GetValue(string fieldName)\r\n");
            code.Append("        {\r\n");
            code.Append("            switch (fieldName)\r\n");
            code.Append("            {\r\n");
            foreach(var field in fields)
            {
                code.Append($"                case \"{field.Name}\": return {field.Name};\r\n");
            }
            code.Append("                default: return null;\r\n");
            code.Append("            }\r\n");
            code.Append("        }\r\n");
            code.Append("        public override void SetValue(string fieldName, object value)\r\n");
            code.Append("        {\r\n");
            code.Append("            switch (fieldName)\r\n");
            code.Append("            {\r\n");
            foreach (var field in fields)
            {
                code.Append($"                case \"{field.Name}\": {{ {field.Name} = ({field.FieldType})value; break; }};\r\n");
            }
            code.Append("                default: break;\r\n");
            code.Append("            }\r\n");
            code.Append("        }\r\n");

            return code.ToString();
        }

        private static string GetTypeEventIdCode(Type type)
        {
            if (!type.IsSubclassOf(typeof(EventNodeData)))
            {
                return "";
            }
            var code = new StringBuilder();
            var eventNode = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("GetEventId");
            int eventId = (int)methodInfo.Invoke(eventNode, null);
            code.Append("        public override int GetEventId()\r\n");
            code.Append("        {\r\n");
            code.Append($"            return {eventId};\r\n");
            code.Append("        }\r\n");
            return code.ToString();
        }

        private static string GetTypeExecuteLogicCode(Type type)
        {
            if (type.IsSubclassOf(typeof(EventNodeData)))
            {
                return "";
            }
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
                code.Append(CodeGenerateConfig.ExecuteLogicStart);
                code.Append("        public override void ExecuteLogic()\r\n");
                code.Append("        {\r\n");
                code.Append("            \r\n");
                code.Append("        }\r\n");
                code.Append(CodeGenerateConfig.ExecuteLogicEnd);
                return code.ToString();
            }
            else
            {
                return oldExecLogicCode;
            }
        }
    }

}
