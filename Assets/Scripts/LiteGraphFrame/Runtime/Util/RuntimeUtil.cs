namespace LiteGraphFrame
{
    public static class LiteGraphRuntimeUtil
    {
        public static void RunLiteGrpah(string assetPath, int eventId)
        {
            var graphObj = new GraphRuntime(assetPath);
            graphObj.RunEvent(eventId);
        }
    }
}
