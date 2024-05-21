using LitJson;

namespace LiteGraphFrame
{
    class FlowPortData : PortDataBase
    {
        public FlowPortData() : base() 
        {
            PortType = EPortType.Flow;
        }

        public override bool CanConnectTo(PortDataBase otherPortData)
        {
            if (!base.CanConnectTo(otherPortData))
            {
                return false;
            }
            // ���̶˿�ֻ�ܺ����̶˿�����
            if (otherPortData is not FlowPortData)
            {
                return false;
            }
            return true;
        }
    }
}
