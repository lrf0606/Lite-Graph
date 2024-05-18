using System.IO;


namespace LiteGraphFrame
{
    public static class LiteGraphCommonUtil
    {
        public const string Extension = "bp";
        public const string ExtensionWithDot = ".bp";
        public const string NewFile = "New Lite Graph";

        public static bool IsLiteGraphFile(string assetPath)
        {
            var extension = Path.GetExtension(assetPath);
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }
            if (extension.ToLowerInvariant() != ExtensionWithDot)
            {
                return false;
            }
            return true;
        }
    }   
}
