using LitJson;

namespace LiteGraphFrame
{
    abstract class IJsonable
    {
        public abstract JsonData Encoder();

        public abstract void Decoder(JsonData jsonData);

        public string Serialize()
        {
            return Encoder().ToJson();
        }

        public void Deserialize(string strData)
        {
            var jsonData = JsonMapper.ToObject(strData);
            Decoder(jsonData);
        }
    }
}
