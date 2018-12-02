using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HttpServer
{
    class HttpServer
    {
        HttpListener httpListener;
        Serializer serializer;
        Dictionary<String, Delegate> methods;
        Input input;

        public HttpServer(String ip, String port)
        {
            httpListener = new HttpListener();
            methods = new Dictionary<string, Delegate>();
            serializer = new JSONSerializer();
            registerMethods(ip, port);
        }

        void registerMethods(String ip, String port)
        {
            httpListener.Prefixes.Add($"{ip}:{port}/Ping/");
            httpListener.Prefixes.Add($"{ip}:{port}/PostInputData/");
            httpListener.Prefixes.Add($"{ip}:{port}/GetAnswer/");
            httpListener.Prefixes.Add($"{ip}:{port}/Stop/");
            methods.Add($"{ip}:{port}/Ping/", new Action<HttpListenerContext>(ping));
            methods.Add($"{ip}:{port}/PostInputData/", new Action<HttpListenerContext>(postInputData));
            methods.Add($"{ip}:{port}/GetAnswer/", new Action<HttpListenerContext>(getAnswer));
            methods.Add($"{ip}:{port}/Stop/", new Action<HttpListenerContext>(stop));
        }

        public void run()
        {
            httpListener.Start();
            while(httpListener.IsListening)
            {
                var context = httpListener.GetContext();
                String path = context.Request.Url.ToString();
                methods[path].DynamicInvoke(context);
            }
        }

        void stop(HttpListenerContext context)
        {
            postResponse("", context);
            System.Threading.Thread.Sleep(1000);
            httpListener.Stop();
        }

        void ping(HttpListenerContext context)
        {
            postResponse("", context);
        }

        void postInputData(HttpListenerContext context)
        {
            using (StreamReader streamReader = new StreamReader(context.Request.InputStream))
            {
               String requestBody = streamReader.ReadToEnd();
               input = serializer.deserialize<Input>(requestBody);
            }
            postResponse("", context);
        }

        void postResponse(String response, HttpListenerContext context)
        {
            context.Response.StatusCode = 200;
            using (StreamWriter streamWriter = new StreamWriter(context.Response.OutputStream))
            {
                streamWriter.Write(response);
            }
        }

        void getAnswer(HttpListenerContext context)
        {
            String serializedOutput = serializer.serialize(new Output());
            if (input != null)
            {
                Output output = MathUtils.doMathWithInput(input);
                serializedOutput = serializer.serialize(output);
            }
            postResponse(serializedOutput, context);
        }
    }
}
