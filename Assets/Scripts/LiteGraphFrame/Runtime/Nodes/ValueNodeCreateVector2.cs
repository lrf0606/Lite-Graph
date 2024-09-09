using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ValueNodeCreateVector2 : NodeRuntime
    {
        public float X;
        public float Y;
        public LitJson.Vector2 Vec2;

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
                case "X": { X = (float)value; break; };
                case "Y": { Y = (float)value; break; };
                case "Vec2": { Vec2 = (LitJson.Vector2)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            Vec2 = new LitJson.Vector2(X, Y);
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===