using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClient
{
    interface Serializer
    {
        String serialize<T>(T input);
        T deserialize<T>(string serializableStr);
    }
}

