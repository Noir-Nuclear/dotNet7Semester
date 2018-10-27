using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Serialization
{
    public class Input
    {
        public int K { get; set; }
        public decimal[] Sums { get; set; }
        public int[] Muls { get; set; }
    }

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

    public class Output
    {
        public decimal SumResult { get; set; }
        public int MulResult { get; set; }
        public decimal[] SortedInputs { get; set; }
    }

    interface Serializer
    {
        String serialize<T>(T input);
        T deserialize<T>(string serializableStr);
    }

    class XMLSerializer : Serializer
    {
        public string serialize<T>(T input)
        {
            //todo: если метод вызывать на ождном и том же типе 1000 000 раз, то 1000 000 раз будет создаваться сериализатор, попробуйте закешировать его в поле класса
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            String result;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, input);
                result = Encoding.UTF8.GetString(stream.ToArray());
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.LoadXml(result);
                doc.ChildNodes.Item(1).Attributes.RemoveAll();
                doc.RemoveChild(doc.ChildNodes.Item(0));
                return doc.InnerXml;
            }
        }

        public T deserialize<T>(string serializableStr)
        {
            //todo: если метод вызывать на ождном и том же типе 1000 000 раз, то 1000 000 раз будет создаваться сериализатор, попробуйте закешировать его в поле класса
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(serializableStr);
            return (T)serializer.Deserialize(stringReader);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Serializer serializer;
            String serializationType = Console.ReadLine(), serializableText = Console.ReadLine();
            //todo: есть такой паттерн - цепочка отвественностей. смысл - инкапсулировать выбор сериализации внутри класса сериализации
            //todo: т.е. вместо свитча в мейне, научить сериализатор отвечать на вопрос, а моржет ли он работать с таким типом сериалзцции
            switch (serializationType)
            {
                case "Json":
                    serializer = new JSONSerializer();
                    break;
                case "Xml":
                    serializer = new XMLSerializer();
                    break;
                default:
                    return;
            }
            Console.WriteLine(doMathWithSrlz(serializer, serializableText));
        }

        static String doMathWithSrlz(Serializer serializer, String serializableInput)
        {
            Input input = serializer.deserialize<Input>(serializableInput);
            Output output = new Output();
            List<decimal> sortedInputs = input.Sums.ToList();
            foreach (decimal sum in input.Sums)
            {
                output.SumResult += sum;
            }
            output.SumResult *= input.K;
            output.MulResult = 1;
            foreach (int mul in input.Muls)
            {
                output.MulResult *= mul;
                sortedInputs.Add(mul);
            }
            sortedInputs.Sort();
            output.SortedInputs = sortedInputs.ToArray();
            return serializer.serialize<Output>(output);
        }
    }
}

