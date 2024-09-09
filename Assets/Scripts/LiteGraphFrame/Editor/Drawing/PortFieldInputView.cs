using System.Collections.Generic;
using UnityEngine.UIElements;
using LitJson;

namespace LiteGraphFrame
{
    static class FieldPortUtil
    {
        public const string INT_NAME = "Int32";
        public const string FLOAT_NAME = "Single";
        public const string BOOL_NAME = "Boolean";
        public const string STRING_NAME = "String";
        public const string VECTOR2_NAME = "Vector2";
        public const string VECTOR3_NAME = "Vector3";

        public static Dictionary<string, HashSet<string>> TypeTransDict;

        static FieldPortUtil()
        {
            TypeTransDict = new Dictionary<string, HashSet<string>>
            {
                [INT_NAME] = new HashSet<string>() { STRING_NAME, FLOAT_NAME },
                [FLOAT_NAME] = new HashSet<string>() { INT_NAME, STRING_NAME },
                [BOOL_NAME] = new HashSet<string>() { STRING_NAME },
                [STRING_NAME] = new HashSet<string>() { },
                [VECTOR2_NAME] = new HashSet<string>() { STRING_NAME },
                [VECTOR3_NAME] = new HashSet<string>() { STRING_NAME }
            };
        }

        public static bool CheckFieldCanTransform(string sourceType, string targetType)
        {
            if (TypeTransDict.TryGetValue(sourceType, out var typeSet))
            {
                if (typeSet.Contains(targetType))
                {
                    return true;
                }
            }
            return false;
        }

    }

    class PortFieldInputView : VisualElement
    {
        private VisualElement m_FieldElement;
        private FieldPortData m_FieldPortData;
        public PortFieldInputView(FieldPortData portData)
        {
            // style
            this.style.minWidth = 40;

            m_FieldPortData = portData;
            CreateFieldInput(portData);
            RefreshVisible();
        }

        public void RefreshVisible()
        {
            this.visible = !m_FieldPortData.IsConnected();
        }

        public void CreateFieldInput(FieldPortData portData)
        {
            var fieldType = portData.FieldTypeName;
            var fieldValue = portData.FieldValue;
            if (fieldType == FieldPortUtil.INT_NAME)
            {
                var intField = new IntegerField
                {
                    value = (int)ValueParserUtil.ToObject(fieldType, fieldValue)
                };
                intField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue.ToString(); });
                m_FieldElement = intField;
            }
            else if (fieldType == FieldPortUtil.STRING_NAME)
            {
                var textFiled = new TextField
                {
                    value = (string)ValueParserUtil.ToObject(fieldType, fieldValue)
                };
                textFiled.style.maxWidth = 150;
                textFiled.RegisterValueChangedCallback(evt=> { portData.FieldValue = evt.newValue.ToString(); });
                m_FieldElement = textFiled;
            }
            else if (fieldType == FieldPortUtil.FLOAT_NAME)
            {
                var floatField = new FloatField
                {
                    value = (float)ValueParserUtil.ToObject(fieldType, fieldValue)
                };
                floatField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue.ToString(); });
                m_FieldElement = floatField;
            }
            else if (fieldType == FieldPortUtil.BOOL_NAME)
            {
                var boolField = new Toggle
                {
                    value = (bool)ValueParserUtil.ToObject(fieldType, fieldValue)
                };
                boolField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue.ToString(); });
                m_FieldElement = boolField;
            }
            else if (fieldType == FieldPortUtil.VECTOR2_NAME)
            {
                var vec2Field = new Vector2Field();
                var vec2 = (Vector2)ValueParserUtil.ToObject(fieldType, fieldValue);
                vec2Field.value = new UnityEngine.Vector2(vec2.x, vec2.y);
                vec2Field.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue.ToString(); });
                m_FieldElement = vec2Field;
            }
            else if (fieldType == FieldPortUtil.VECTOR3_NAME)
            {
                var vec3Field = new Vector3Field();
                var vec3 = (Vector3)ValueParserUtil.ToObject(fieldType, fieldValue);
                vec3Field.value = new UnityEngine.Vector3(vec3.x, vec3.y, vec3.z);
                vec3Field.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue.ToString(); });
                m_FieldElement = vec3Field;
            }
            if (m_FieldElement != null)
            {
                Add(m_FieldElement);
            }
        }

    }

}
