using LitJson;
using System.Reflection;


namespace LiteGraphFrame
{
    class FieldPortData : PortDataBase
    {
        private string m_FieldName;
        private string m_FieldTypeName;
        private object m_FieldValue;
        private string m_FieldDescription;

        public string FieldName => m_FieldName;
        public string FieldTypeName => m_FieldTypeName;
        public object FieldValue { get { return m_FieldValue; } set { m_FieldValue = value; } }
        public string FieldDescription => m_FieldDescription;

        public FieldPortData() : base()
        {
            m_PortType = EPortType.Field;
        }

        public void InitFieldIfno(FieldInfo fieldInfo, string fieldDescription)
        {
            m_FieldName = fieldInfo.Name;
            m_FieldTypeName = fieldInfo.FieldType.Name;
            m_FieldValue = fieldInfo.GetValue(NodeData);
            m_FieldDescription = fieldDescription;
        }

        public override bool CanConnectTo(PortDataBase otherPortData)
        {
            if (!base.CanConnectTo(otherPortData))
            {
                return false;
            }
            // 数据端口只能和数据类型相同的数据端口连接
            if (otherPortData is not FieldPortData)
            {
                return false;
            }
            // 数据端口部分数据类型可以转化
            var otherPortSourceTypeName = ((FieldPortData)otherPortData).FieldTypeName;
            if (FieldTypeName != otherPortSourceTypeName && !FieldPortUtil.CheckFieldCanTransform(FieldTypeName, otherPortSourceTypeName))
            {
                return false;
            }
            return true;
        }

        public override JsonData Encoder()
        {
            var jsonData = base.Encoder();
            jsonData["FieldName"] = m_FieldName;
            jsonData["FieldTypeName"] = m_FieldTypeName;
            jsonData["FieldValue"] = ValuePraseUtil.ToString(m_FieldTypeName, m_FieldValue);
            jsonData["FieldDescription"] = m_FieldDescription;
            return jsonData;
        }

        public override void Decoder(JsonData jsonData)
        {
            base.Decoder(jsonData);
            m_FieldName = (string)jsonData["FieldName"];
            m_FieldTypeName = (string)jsonData["FieldTypeName"];
            m_FieldValue = ValuePraseUtil.ToObject(FieldTypeName, (string)jsonData["FieldValue"]);
            m_FieldDescription = (string)jsonData["FieldDescription"];
        }
    }
}
