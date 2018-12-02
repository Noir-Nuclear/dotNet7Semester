using System;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            String port = Console.ReadLine();
            new HttpServer("http://127.0.0.1", port).run();
        }
    }
}
