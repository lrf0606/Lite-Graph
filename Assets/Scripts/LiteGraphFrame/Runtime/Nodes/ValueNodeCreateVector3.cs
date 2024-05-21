// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ValueNodeCreateVector3 : NodeRuntime
    {
        public System.Single X;
        public System.Single Y;
        public System.Single Z;
        public UnityEngine.Vector3 Vec3;

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
                case "X": { X = (System.Single)value; break; };
                case "Y": { Y = (System.Single)value; break; };
                case "Z": { Z = (System.Single)value; break; };
                case "Vec3": { Vec3 = (UnityEngine.Vector3)value; break; };
                default: break;
            }
        }
        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            Vec3 = new UnityEngine.Vector3(X, Y, Z);
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===
