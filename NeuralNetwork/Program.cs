using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NeuralNetwork
{
    class Program
    {
        static NeuralNetwork network = null;
        static float threshold = 0.0f;
        static float bias = 0.0f;
        static int inputLayerNodes = 4;
        static int outputLayerNodes = 3;
        static int seed = 3;
        static Random rand;
        static float[][] inputTestData;
        static int testDataCount = 100;
        static void Main(string[] args)
        {
            
            CreateNetwork();

            
            rand = new Random();

            TestData();


            Console.WriteLine("Done testing");
            string exit = "";
            while(exit.ToUpper() != "N")
            {
                float[] input = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("What is your {0} number?:", i + 1);
                    string num = Console.ReadLine();
                    input[i] = Int32.Parse(num);
                    input[i] = input[i] / 250;
                }

                testInput(input);

                Console.Write("\nDo you want to test another 4 numbers from (0-250)? (Y/N) :");
                exit = Console.ReadLine();
            }
            
        }
        public static void CreateNetwork()
        {
            network = new NeuralNetwork(threshold, bias);
            network.AddLayer(inputLayerNodes);
            network.AddLayer(outputLayerNodes);

            network.RandomiseLinks();
        }

        public static void TestData()
        {
            if (network == null)
                return;
            //Generate testData
            inputTestData = new float[testDataCount][];

            for (int i = 0; i < testDataCount; i++)
                inputTestData[i] = new float[inputLayerNodes];

            foreach (float[] inputDataHor in inputTestData)
                for (int i = 0; i < inputLayerNodes; i++)
                {
                    inputDataHor[i] = rand.Next(0, 250);
                    inputDataHor[i] = inputDataHor[i] / 250;
                }


            Console.WriteLine("Testing 100 testData");


            for (int iterations = 0; iterations < 1000; iterations++)
            {

                for (int i = 0; i < testDataCount; i++)
                {
                    network.Start(inputTestData[i]);
                    float[] initialResult = network.Output();
                    float initialError = ErrorFunction(initialResult, inputTestData[i]);

                    for (int check = 0; check < 100; check++)
                    {
                        network.ChangeRandomWeight();
                        network.Start(inputTestData[i]);
                        float[] endResult = network.Output();
                        float endError = ErrorFunction(endResult, inputTestData[i]);

                        if (endError < initialError)
                            check = 1000;
                        else
                            network.RevertRandomWeightChange();
                    }
                }
            }
        }
        
        public static void testInput(float[] input)
        {
            Console.WriteLine("Input was [{0},{1},{2},{3}]", input[0], input[1], input[2], input[3]);
            network.Start(input);
            float[] output = network.Output();
            Console.WriteLine("Output was [{0},{1},{2}]", output[0], output[1], output[2]);
            Console.WriteLine("Error was {0}", ErrorFunction(output, input));
        }

        public static float ErrorFunction(float[] realResult,float[] input)
        {
            int largestIndex = 0;
            for (int i = 0; i < 3; i++)
            {
                if (realResult[i] > realResult[largestIndex])
                    largestIndex = i;
            }


            float wantedOutput = input[0]*250 + input[1]*250 + input[2]*250 + input[3]*250;

            int numb = 0;


            if (wantedOutput < 250)
                numb = 0;
            else if (wantedOutput >= 250 && wantedOutput <= 750)
                numb = 500;
            else
                numb = 1000;

            if (largestIndex == 0)
                return Math.Abs(numb - 0);
            if (largestIndex == 1)
                return Math.Abs(numb - 500);
            else
                return Math.Abs(numb - 1000);
        }

    }
}
