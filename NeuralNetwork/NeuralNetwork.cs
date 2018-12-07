using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        public int layers = 0;
        public float threshold;
        public float bias;
        public float linkPrevWeight;
        public NeuralLink lastChangedLink;
        public List<NeuralLink> links;
        public List<NeuralNode[]> neuralNodeTree;
        public Tuple<int, float, float> change;

        //public List<Tuple<int, float,float>> changeStack;

        public NeuralNetwork(float inThreshold, float inBias)
        {
            neuralNodeTree = new List<NeuralNode[]>();
            threshold = inThreshold;
            inBias = bias;
            //changeStack = new List<Tuple<int, float,float>>();
            links = new List<NeuralLink>();
        }
        public void AddLayer(int nodeCount)
        {
            int index = -1;
            //if index is not set then add a layer to the end then set index to the recently added layer
            neuralNodeTree.Add(new NeuralNode[nodeCount]);
            index = neuralNodeTree.Count - 1;


            layers += 1;

            //sets each node in the tree
            for (int i = 0; i < nodeCount; i++)
                neuralNodeTree[index][i] = new NeuralNode(threshold, bias);

            //if it is not the first layer added to the network
            //then add links to the previous node
            if (index != 0)
            {
                //first it calls addLinks which creates connections between 2 layers
                //then adds the list of links into the links container.
                foreach (NeuralNode node in neuralNodeTree[index - 1])
                    links.AddRange(node.addLinks(neuralNodeTree[index]));
            }
        }
        public void RandomiseLinks( Random rand)
        {
            foreach(NeuralLink link in links)
            {
                //sets the weight between -1 and 1
                link.weight = (float)(rand.NextDouble() * (1 - -1) + -1);
            }
        }
        public void Start(float[] inputs)
        {
            Reset();
            int length = inputs.GetLength(0);
            //makes sure that the input length and the initial nodes are the same length
            if (length == neuralNodeTree[0].GetLength(0))
            {
                //foreach node in the initial layer give it the corresponding information and make them fire.
                for (int i = 0; i < length; i++)
                {
                    neuralNodeTree[0][i].Start(inputs[i]);
                }
            }
        }
        public float[] Output()
        {
            int nodesInLastLayer = neuralNodeTree[layers - 1].GetLength(0);
            float[] neuralOutput = new float[nodesInLastLayer];

            //retrieves the final value for the last layer and returns it back
            for (int i = 0; i < nodesInLastLayer; i++)
            {
                neuralOutput[i] = neuralNodeTree[layers - 1][i].value;
            }
            return neuralOutput;
        }
        public void ChangeRandomWeight(Random rand)
        {
            //randomly picks a random weight and randomly chooses another number between -1 and 1 and sets it to the weight.
            //then gives it to the change tuple so it knows what was the last change
            int linkIndex = rand.Next(0, links.Count-1);
            float tempCurrentWeight = links[linkIndex].weight;
            links[linkIndex].weight = (float)(rand.NextDouble() * (1 - -1) + -1);
            //changeStack.Add(new Tuple<int, float, float>(linkIndex, tempCurrentWeight, links[linkIndex].weight));
            change = new Tuple<int, float, float>(linkIndex, tempCurrentWeight, links[linkIndex].weight);

        }
        public void RevertRandomWeightChange()
        {
            links[change.Item1].weight = change.Item2;
        }
        private void Reset()
        {
            //goes through each node and resets the value they hold and the number of inputs back to 0 so it can run again.
            foreach (NeuralNode[] nodeArray in neuralNodeTree)
            {
                foreach (NeuralNode node in nodeArray)
                {
                    node.value = 0;
                    node.inputLinkCount = 0;
                }
            }
        }
    }
}
