using LitJson;
using System.Reflection;


namespace LiteGraphFrame
{
    class FieldPortData : PortDataBase
    {
        public string FieldName { get; set; }
        public string SourceTypeName { get; set; } // 端口原本数据类型
        public string TargetTypeName { get; set; } // 和其他端口连接后需要转化为的数据类型
        public object FieldValue { get; set; }
        public string FieldDescription { get; set; }
        public object RuntimeFieldValue { get; set; } // 不需要序列化

        public FieldPortData() : base()
        {
            PortType = EPortType.Field;
        }

        public void InitFieldIfno(FieldInfo fieldInfo, string fieldDescription)
        {
            FieldName = fieldInfo.Name;
            SourceTypeName = fieldInfo.FieldType.Name;
            TargetTypeName = "";
            FieldValue = fieldInfo.GetValue(OwnerNodeData);
            FieldDescription = fieldDescription;
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
            var otherPortSourceTypeName = ((FieldPortData)otherPortData).SourceTypeName;
            if (SourceTypeName != otherPortSourceTypeName && !FieldPortUtil.CheckFieldCanTransform(SourceTypeName, otherPortSourceTypeName))
            {
                return false;
            }
            return true;
        }

        public override void OnConnectedChange(bool isConnected, PortDataBase otherPortData)
        {
            if (!IsInputPort)
            {
                if (isConnected)
                {
                    TargetTypeName = ((FieldPortData)otherPortData).SourceTypeName;
                }
                else
                {
                    TargetTypeName = "";
                }
            }

        }

        public override JsonData Encoder()
        {
            var jsonData = base.Encoder();
            jsonData["FieldName"] = FieldName;
            jsonData["SourceTypeName"] = SourceTypeName;
            jsonData["TargetTypeName"] = TargetTypeName;
            jsonData["FieldValue"] = ValuePraseUtil.ToString(SourceTypeName, FieldValue);
            jsonData["FieldDescription"] = FieldDescription;
            return jsonData;
        }

        public override void Decoder(JsonData jsonData)
        {
            base.Decoder(jsonData);
            FieldName = (string)jsonData["FieldName"];
            SourceTypeName = (string)jsonData["SourceTypeName"];
            TargetTypeName = (string)jsonData["TargetTypeName"];
            FieldValue = ValuePraseUtil.ToObject(SourceTypeName, (string)jsonData["FieldValue"]);
            FieldDescription = (string)jsonData["FieldDescription"];
        }
    }
}
