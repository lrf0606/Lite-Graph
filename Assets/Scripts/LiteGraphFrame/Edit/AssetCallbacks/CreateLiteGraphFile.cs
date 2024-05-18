using UnityEditor;

namespace LiteGraphFrame
{
    static class CreateLiteGraph
    {
        [MenuItem("Assets/Create/Lite Graph File")]
        public static void CreateLiteGraphFile()
        {
            LiteGraphEditorUtil.CreateNewLiteGraphFile();
        }
    }
}
