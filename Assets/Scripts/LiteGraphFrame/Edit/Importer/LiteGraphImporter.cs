using UnityEditor.AssetImporters;
using UnityEngine;


namespace LiteGraphFrame
{
    [ScriptedImporter(1, LiteGraphCommonUtil.Extension)]
    class LiteGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string text = LiteGraphFileUtil.SafeReadAllText(ctx.assetPath);
            var textAsset = new TextAsset(text);
            ctx.AddObjectToAsset("main obj", textAsset);
            ctx.SetMainObject(textAsset);
        }
    }
}
