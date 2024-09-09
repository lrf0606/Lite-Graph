using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ValueNodeCreateVector3 : NodeRuntime
    {
        public float X;
        public float Y;
        public float Z;
        public LitJson.Vector3 Vec3;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "X": return X;
                case "Y": return Y;
                case "Z": return Z;
                case "Vec3": return Vec3;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "X": { X = (float)value; break; };
                case "Y": { Y = (float)value; break; };
                case "Z": { Z = (float)value; break; };
                case "Vec3": { Vec3 = (LitJson.Vector3)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            Vec3 = new LitJson.Vector3(X, Y, Z);
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===