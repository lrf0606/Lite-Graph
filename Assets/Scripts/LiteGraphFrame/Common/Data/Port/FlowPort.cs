namespace LiteGraphFrame
{
    class FlowPortData : PortDataBase
    {
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
