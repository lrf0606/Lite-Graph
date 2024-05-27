using LitJson;
using System.Reflection;


namespace LiteGraphFrame
{
    class FieldPortData : PortDataBase
    {
        public string FieldName { get; set; }
        public string TypeName { get; set; }
        public object FieldValue { get; set; }
        public string FieldDescription { get; set; }
        public object RuntimeFieldValue { get; set; } // ����Ҫ���л�

        public FieldPortData() : base()
        {
            PortType = EPortType.Field;
        }

        public void InitFieldIfno(FieldInfo fieldInfo, string fieldDescription)
        {
            FieldName = fieldInfo.Name;
            TypeName = fieldInfo.FieldType.Name;
            FieldValue = fieldInfo.GetValue(OwnerNodeData);
            FieldDescription = fieldDescription;
        }

        public override bool CanConnectTo(PortDataBase otherPortData)
        {
            if (!base.CanConnectTo(otherPortData))
            {
                return false;
            }
            // ���ݶ˿�ֻ�ܺ�����������ͬ�����ݶ˿�����
            if (otherPortData is not FieldPortData)
            {
                return false;
            }
            if (TypeName != ((FieldPortData)otherPortData).TypeName)
            {
                return false;
            }
            return true;
        }

        public override JsonData Encoder()
        {
            var jsonData = base.Encoder();
            jsonData["FieldName"] = FieldName;
            jsonData["TypeName"] = TypeName;
            jsonData["FieldValue"] = ValuePraseUtil.ToString(TypeName, FieldValue);
            jsonData["FieldDescription"] = FieldDescription;
            return jsonData;
        }

        public override void Decoder(JsonData jsonData)
        {
            base.Decoder(jsonData);
            FieldName = (string)jsonData["FieldName"];
            TypeName = (string)jsonData["TypeName"];
            FieldValue = ValuePraseUtil.ToObject(TypeName, (string)jsonData["FieldValue"]);
            FieldDescription = (string)jsonData["FieldDescription"];
        }
    }
}
