using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    enum MethodType
    {
        GET, POST
    }

    class Client
    { 
        Serializer serializer;
        WebClient webClient;
        String URL;

        public Client(String port)
        {
            serializer = new JSONSerializer();
            webClient = new WebClient();
            URL = $"http://localhost:{port}";
        }

        public bool ping()
        {
            try
            {
                sendRequest("Ping", MethodType.GET, null);
            }
            catch (WebException)
            {
                return false;
            }
            return true;   
        }

        public T getInputData<T>()
        {
            String serializedInput = Encoding.UTF8.GetString(sendRequest("GetInputData", MethodType.GET, null));
            return serializer.deserialize<T>(serializedInput);
        }

        public void postAnswer<T>(T data)
        {
            String serializedAnswer = serializer.serialize(data);
            sendRequest("WriteAnswer", MethodType.POST, serializedAnswer);
        }

        private byte[] sendRequest(String methodName, MethodType methodType, String body)
        { 
            String fullURL = $"{URL}/{methodName}/";
            switch (methodType)
            {
                case MethodType.GET:
                    return webClient.DownloadData(fullURL);
                case MethodType.POST:
                    return webClient.UploadData(fullURL, Encoding.UTF8.GetBytes(body));
            }
            return null;
        }

    }
}
