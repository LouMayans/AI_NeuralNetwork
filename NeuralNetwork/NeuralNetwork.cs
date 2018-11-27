using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        Random rand;
        public int layers = 0;
        public List<NeuralNode[]> neuralNodeTree;
        public float threshold;
        public float bias;

        public List<NeuralLink> links;

        //public List<Tuple<int, float,float>> changeStack;
        public float linkPrevWeight;
        public NeuralLink lastChangedLink;
        public NeuralNetwork(float inThreshold, float inBias)
        {
            neuralNodeTree = new List<NeuralNode[]>();
            threshold = inThreshold;
            inBias = bias;
            //changeStack = new List<Tuple<int, float,float>>();
            links = new List<NeuralLink>();
        }

        public void AddLayer(int nodeCount, int index = -1)
        {

            if (index == -1)
            {
                neuralNodeTree.Add(new NeuralNode[nodeCount]);
                index = neuralNodeTree.Count - 1;
            }
            else
                neuralNodeTree.Insert(index, new NeuralNode[nodeCount]);

            layers += 1;

            for (int i = 0; i < nodeCount; i++)
                neuralNodeTree[index][i] = new NeuralNode(threshold, bias);


            //if there is a layer after the new layer
            if (index < neuralNodeTree.Count - 1)
            {
                foreach (NeuralNode node in neuralNodeTree[index])
                    links.AddRange( node.addLink(neuralNodeTree[index + 1]));
            }
            //checks if this layer is not the first layer

            if (index != 0)
            {
                //links the layers before this layer.
                foreach (NeuralNode node in neuralNodeTree[index - 1])
                    links.AddRange(node.addLink(neuralNodeTree[index]));
            }
        }
        public void RandomiseLinks( Random rand)
        {
            foreach(NeuralNode[] nodeArray in neuralNodeTree)
            {
                foreach(NeuralNode node in nodeArray)
                {
                    if (node.neuralLinks == null)
                        continue;
                    foreach(NeuralLink link in node.neuralLinks)
                    {
                        link.weight = (float)(rand.NextDouble() * (1 - -1) + -1);
                    }
                }
            }
        }
        public void Start(float[] inputs)
        {
            Reset();
            int length = inputs.GetLength(0);
            if (length == neuralNodeTree[0].GetLength(0))
            {
                for (int i = 0; i < length; i++)
                {
                    neuralNodeTree[0][i].Start(inputs[i]);
                }
            }
        }
        private void Reset()
        {
            foreach (NeuralNode[] nodeArray in neuralNodeTree)
            {
                foreach (NeuralNode node in nodeArray)
                {
                    node.value = 0;
                    node.inputLinkCount = 0;
                }
            }
        }

        public float[] Output()
        {
            int nodesInLastLayer = neuralNodeTree[layers - 1].GetLength(0);
            float[] neuralOutput = new float[nodesInLastLayer];

            for (int i = 0; i < nodesInLastLayer; i++)
            {
                neuralOutput[i] = neuralNodeTree[layers - 1][i].value;
            }
            return neuralOutput;
        }

        public Tuple<int, float, float> change;

        public void ChangeRandomWeight(Random rand)
        {
            int linkIndex = rand.Next(0, links.Count-1);
            float tempCurrentWeight = links[linkIndex].weight;
            links[linkIndex].weight = (float)(rand.NextDouble() * (1 - -1) + -1);
            //changeStack.Add(new Tuple<int, float, float>(linkIndex, tempCurrentWeight, links[linkIndex].weight));
            change = new Tuple<int, float, float>(linkIndex, tempCurrentWeight, links[linkIndex].weight);

        }
        public void RevertRandomWeightChange()
        {
            links[change.Item1].weight = change.Item2;

            //links[changeStack[changeStack.Count - 1].Item1].weight = changeStack[changeStack.Count - 1].Item2;
            //changeStack.Remove(changeStack[changeStack.Count - 1]);
        }
    }
}
