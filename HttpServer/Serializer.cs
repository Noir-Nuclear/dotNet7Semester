using System;

namespace HttpServer
{
    interface Serializer
    {
        String serialize<T>(T input);
        T deserialize<T>(string serializableStr);
    }
}

