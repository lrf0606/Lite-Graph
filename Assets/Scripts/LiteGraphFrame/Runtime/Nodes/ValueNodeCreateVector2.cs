// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ValueNodeCreateVector2 : NodeRuntime
    {
        public System.Single X;
        public System.Single Y;
        public UnityEngine.Vector2 Vec2;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "X": return X;
                case "Y": return Y;
                case "Vec2": return Vec2;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "X": { X = (System.Single)value; break; };
                case "Y": { Y = (System.Single)value; break; };
                case "Vec2": { Vec2 = (UnityEngine.Vector2)value; break; };
                default: break;
            }
        }
        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            Vec2 = new UnityEngine.Vector2(X, Y);
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===
