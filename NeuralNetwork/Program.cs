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
        static int seed = 45;
        static Random rand;
        static List<float[]> inputTestData;
        static List<float[]> outputTestData;
        static int testDataCount = 100;


        static void Main(string[] args)
        {
            rand = new Random();

            CreateNetwork();
            
            GenerateTestInput();

            UserTestInput();

            TrainData();


            Console.WriteLine("\nDone testing");

            string exit = "";
            while(exit.ToUpper() != "N")
            {
                UserTestInput();

                Console.Write("\nDo you want to test another 4 numbers from (0-250)? (Y/N) :");
                exit = Console.ReadLine();
            }
            
        }
        public static void UserTestInput()
        {
            //this section tests the neural network first before anything occurs to better the network
            float[] input1 = new float[4];
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("What is your {0} number?:", i + 1);
                string num = Console.ReadLine();
                input1[i] = float.Parse(num);
            }

            //prints out error of all test data
            testInput(input1);
        }
        public static void CreateNetwork()
        {
            network = new NeuralNetwork(threshold, bias);

            network.AddLayer(inputLayerNodes);
            network.AddLayer(5);
            network.AddLayer(outputLayerNodes);

            network.RandomiseLinks(rand);
        }
        public static void GenerateTestInput()
        {
            inputTestData = new List<float[]>();
            outputTestData = new List<float[]>();


            float[] testInput = new float[inputLayerNodes];
            float[] testOutput = new float[outputLayerNodes];
            //Foreach test data, generate inputs and outputs
            for (int i = 0; i < testDataCount; i++)
            {

                //generates inputs and makes sure its a unique input
                do
                {
                    testInput = new float[inputLayerNodes];
                    for (int j = 0; j < inputLayerNodes; j++)
                    {
                        testInput[j] = rand.Next(0, 5);
                        testInput[j] = testInput[j] / 4;
                    }
                } while (inputTestData.Contains(testInput));
                inputTestData.Add(testInput);

                //generates outputs and makes sure its a unique output
                do
                {
                    testOutput = new float[outputLayerNodes];
                    for (int j = 0; j < outputLayerNodes; j++)
                    {
                        testOutput[j] = rand.Next(0, 5);
                        testOutput[j] = testOutput[j] / 4;
                    }
                } while (outputTestData.Contains(testInput));
                outputTestData.Add(testOutput);
            }

            //Console logs the inputs and outputs
            for (int i = 0; i < testDataCount; i++)
            {
                Console.WriteLine(inputTestData[i][0].ToString() + ',' + inputTestData[i][1].ToString() +
                    ',' + inputTestData[i][2].ToString() + ',' + inputTestData[i][3].ToString() +
                    " => " + outputTestData[i][0].ToString() + ',' + outputTestData[i][1].ToString() + ',' +
                    outputTestData[i][2].ToString());
            }
        }
        public static void TrainData()
        {
            if (network == null)
                return;


            Console.WriteLine("Testing 100 testData");

            //test out the network x many times
            for (int iterations = 0; iterations < 10000; iterations++)
            {
                //Gets the initial error by sending each testinput through the network and then adds all of them together.
                float initialNetworkError = 0;
                for (int i = 0; i < testDataCount; i++)
                {
                    network.Start(inputTestData[i]);
                    float[] initialResult = network.Output();
                    initialNetworkError += ErrorFunction(initialResult, outputTestData[i]);
                }

                //changes random weight
                network.ChangeRandomWeight(rand);

                //gets the error after it changed a random weight.
                float endNetworkError = 0;
                for (int i = 0; i < testDataCount; i++)
                {
                    network.Start(inputTestData[i]);
                    float[] result = network.Output();
                    endNetworkError += ErrorFunction(result, outputTestData[i]);
                }

                //if the end error was better than the initial error, log it then continue to another iteration
                //else revert the random weight change.
                if (endNetworkError < initialNetworkError)
                {
                    Console.WriteLine("EndError = " + endNetworkError.ToString() + "  initialError = " + initialNetworkError.ToString());
                    continue;
                }    
                else
                    network.RevertRandomWeightChange();
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
        public static float ErrorFunction(float[] realResult,float[] wantedResult)
        {
            double addAll = (double)(Math.Pow((realResult[0] - wantedResult[0]), 2.0) +
                        Math.Pow((realResult[1] - wantedResult[1]), 2.0) +
                        Math.Pow((realResult[2] - wantedResult[2]), 2.0));

            return (float)Math.Sqrt(addAll);
        }
    }
}
