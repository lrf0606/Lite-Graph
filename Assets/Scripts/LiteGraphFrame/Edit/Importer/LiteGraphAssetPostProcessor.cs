using UnityEngine;
using UnityEditor;
using System.IO;

namespace LiteGraphFrame
{
    class LiteGraphAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach(var assetPath in importedAssets)
            {
                if (LiteGraphCommonUtil.IsLiteGraphFile(assetPath))
                {
                    // 这里可以处理bp文件导入、删除、移动时的逻辑
                    Debug.Log($"import asset: {assetPath}");
                }  
            }
        }
    }
}
