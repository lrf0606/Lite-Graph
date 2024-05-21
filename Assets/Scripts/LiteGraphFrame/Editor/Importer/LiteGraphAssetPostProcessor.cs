using UnityEngine;
using UnityEditor;
using System.IO;

namespace LiteGraphFrame
{
    class LiteGraphAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetPath in deletedAssets)
            {
                if (LiteGraphEditorUtil.IsLiteGraphFile(assetPath))
                {
                    // todo 处理editor window打开时的删除文件操作
                    //Debug.Log($"delete asset: {assetPath}");
                }  
            }
        }
    }
}
