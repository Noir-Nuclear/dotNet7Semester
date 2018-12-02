using Newtonsoft.Json;

namespace HttpServer
{
    class JSONSerializer : Serializer
    {
        public string serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public T deserialize<T>(string serializableStr)
        {
            return JsonConvert.DeserializeObject<T>(serializableStr);
        }
    }
}
