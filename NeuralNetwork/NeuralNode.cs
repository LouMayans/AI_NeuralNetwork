using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralNode
    {
        
        int PreviousLinks;
        public int inputLinkCount = 0;
        float bias;
        float threshold;
        public float value = 0;
        public float[] NodeInputs = null;
        public NeuralLink[] neuralLinks;


        public NeuralNode(float inThreshold, float inBias)
        {
            threshold = inThreshold;
            bias = inBias;
        }

        public void addLink(NeuralNode[] nodes)
        {
            neuralLinks = new NeuralLink[nodes.GetLength(0)];
            int linkIndex = 0;
            foreach (NeuralNode node in nodes)
            {
                neuralLinks[linkIndex] = new NeuralLink(node);
                ++linkIndex;
                node.PreviousLinks++;
            }
            
        }

        public void FromLink(float input)
        {
            if (NodeInputs == null)
                NodeInputs = new float[PreviousLinks];

            NodeInputs[inputLinkCount] = input;
            ++inputLinkCount;
            if (inputLinkCount == PreviousLinks)
            {
                Fire();
            }
        }

        public void Fire()
        {
            SumInputsAndBias();
            value = (float)(1 / (1 + Math.Exp(-value)));
            if (value > threshold)
            {
                
                if (neuralLinks != null)
                    foreach (NeuralLink link in neuralLinks)
                        link.Fire(value);
            }
            else
            {
                if (neuralLinks != null)
                    foreach (NeuralLink link in neuralLinks)
                        link.Fire(0);
            }
        }

        public void Start(float input)
        {
            NodeInputs = new float[1];
            NodeInputs[0] = input;
            Fire();
        }

        public float SumInputsAndBias()
        {
            value = 0;
            foreach (float input in NodeInputs)
            {
                value += input;
            }
            return value + -bias;
        }
    }
}
