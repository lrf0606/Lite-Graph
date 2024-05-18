using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace LiteGraphFrame
{
    static class LiteGraphEditorUtil
    { 
        public static void CreateNewLiteGraphFile()
        {
            var endAction = ScriptableObject.CreateInstance<NewLiteGraphActoin>();
            string pathName = string.Format("{0}.{1}", LiteGraphCommonUtil.NewFile, LiteGraphCommonUtil.Extension);
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endAction, pathName, null, null);
        }
    }

    class NewLiteGraphActoin : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var graphData = new GraphData();
            graphData.Initlization(pathName);
            LiteGraphFileUtil.WriteToDisk(pathName, graphData.Serialize());
            AssetDatabase.Refresh();
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(obj);
        }

        public override void Cancelled(int instanceId, string pathName, string resourceFile)
        {
            base.Cancelled(instanceId, pathName, resourceFile);
        }
    }


}

