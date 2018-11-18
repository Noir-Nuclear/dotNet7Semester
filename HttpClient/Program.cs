using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            String port = Console.ReadLine();
            Client client = new Client(port);
            while (!client.ping()) ;
            Input input = client.getInputData<Input>();
            Output output = doMathWithInput(input);
            client.postAnswer(output);
        }

        static Output doMathWithInput(Input input)
        {
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
            return output;
        }
    }
}
