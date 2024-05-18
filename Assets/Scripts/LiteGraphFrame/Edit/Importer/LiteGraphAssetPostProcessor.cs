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
                    // ������Դ���bp�ļ����롢ɾ�����ƶ�ʱ���߼�
                    Debug.Log($"import asset: {assetPath}");
                }  
            }
        }
    }
}
