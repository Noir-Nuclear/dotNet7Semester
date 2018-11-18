using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using Newtonsoft.Json;

namespace HttpClient
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
