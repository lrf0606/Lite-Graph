namespace LiteGraphFrame
{
    public static class LiteGraphRuntimeUtil
    {
        public static void RunLiteGrpah(string assetPath, EBPEventId eventId)
        {
            var runtimeObj = new LiteGraphRuntimeObject(assetPath);
            runtimeObj.RunEvent(eventId);
        }
    }
}
