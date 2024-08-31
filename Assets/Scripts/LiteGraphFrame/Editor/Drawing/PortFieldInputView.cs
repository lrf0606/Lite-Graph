using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LiteGraphFrame
{

    static class FieldPortUtil
    {
        public static string INT_NAME = typeof(int).Name;
        public static string FLOAT_NAME = typeof(float).Name;
        public static string BOOL_NAME = typeof(bool).Name;
        public static string STRING_NAME = typeof(string).Name;
        public static string VECTOR2_NAME = typeof(Vector2).Name;
        public static string VECTOR3_NAME = typeof(Vector3).Name;

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
            this.visible = m_FieldPortData.ConnectionInfo.NodeData == null;
        }

        public void CreateFieldInput(FieldPortData portData)
        {
            var fieldType = portData.SourceTypeName;
            var fieldValue = portData.FieldValue;
            if (fieldType == FieldPortUtil.INT_NAME)
            {
                var intField = new IntegerField
                {
                    value = (int)fieldValue
                };
                intField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = intField;
            }
            else if (fieldType == FieldPortUtil.STRING_NAME)
            {
                var textFiled = new TextField
                {
                    value = (string)fieldValue
                };
                textFiled.style.maxWidth = 150;
                textFiled.RegisterValueChangedCallback(evt=> { portData.FieldValue = evt.newValue; });
                m_FieldElement = textFiled;
            }
            else if (fieldType == FieldPortUtil.FLOAT_NAME)
            {
                var floatField = new FloatField
                {
                    value = (float)fieldValue
                };
                floatField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = floatField;
            }
            else if (fieldType == FieldPortUtil.BOOL_NAME)
            {
                var boolField = new Toggle
                {
                    value = (bool)fieldValue
                };
                boolField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = boolField;
            }
            else if (fieldType == FieldPortUtil.VECTOR2_NAME)
            {
                var vec2Field = new Vector2Field
                {
                    value = (Vector2)fieldValue
                };
                vec2Field.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = vec2Field;
            }
            else if (fieldType == FieldPortUtil.VECTOR3_NAME)
            {
                var vec3Field = new Vector3Field
                {
                    value = (Vector3)fieldValue
                };
                vec3Field.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = vec3Field;
            }
            if (m_FieldElement != null)
            {
                Add(m_FieldElement);
            }
        }

    }

}
