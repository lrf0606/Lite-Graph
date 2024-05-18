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
            // 流程端口只能和流程端口连接
            if (otherPortData is not FlowPortData)
            {
                return false;
            }
            return true;
        }
    }
}
