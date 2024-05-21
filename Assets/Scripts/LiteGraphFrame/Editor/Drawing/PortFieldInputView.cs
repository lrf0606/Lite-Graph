using UnityEngine;
using UnityEngine.UIElements;

namespace LiteGraphFrame
{
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
            var fieldType = portData.TypeName;
            var fieldValue = portData.FieldValue;
            if (fieldType == typeof(int).Name)
            {
                var intField = new IntegerField
                {
                    value = (int)fieldValue
                };
                intField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = intField;
            }
            else if (fieldType == typeof(string).Name)
            {
                var textFiled = new TextField
                {
                    value = (string)fieldValue
                };
                textFiled.style.maxWidth = 150;
                textFiled.RegisterValueChangedCallback(evt=> { portData.FieldValue = evt.newValue; });
                m_FieldElement = textFiled;
            }
            else if (fieldType == typeof(float).Name)
            {
                var floatField = new FloatField
                {
                    value = (float)fieldValue
                };
                floatField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = floatField;
            }
            else if (fieldType == typeof(bool).Name)
            {
                var boolField = new Toggle
                {
                    value = (bool)fieldValue
                };
                boolField.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = boolField;
            }
            else if (fieldType == typeof(Vector2).Name)
            {
                var vec2Field = new Vector2Field
                {
                    value = (Vector2)fieldValue
                };
                vec2Field.RegisterValueChangedCallback(evt => { portData.FieldValue = evt.newValue; });
                m_FieldElement = vec2Field;
            }
            else if (fieldType == typeof(Vector3).Name)
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
