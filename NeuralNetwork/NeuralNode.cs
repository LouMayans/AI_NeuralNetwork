using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralNode
    {
        public int inputLinkCount = 0;
        private int PreviousLinks;
        public float value = 0;
        public float[] NodeInputs = null;
        private float bias;
        private float threshold;
        public NeuralLink[] neuralLinks;

        public NeuralNode(float inThreshold, float inBias)
        {
            threshold = inThreshold;
            bias = inBias;
        }

        public NeuralLink[] addLinks(NeuralNode[] nodes)
        {
            //sets up the links
            neuralLinks = new NeuralLink[nodes.GetLength(0)];
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                neuralLinks[i] = new NeuralLink(nodes[i]);
                nodes[i].PreviousLinks++;
            }
            return neuralLinks;
            
        }
        //When previous node fires and then link fires to this function
        public void FromLink(float input)
        {
            if (NodeInputs == null)
                NodeInputs = new float[PreviousLinks];

            NodeInputs[inputLinkCount] = input;
            ++inputLinkCount;
            //if all previous weights fired to us then fire this node
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
                //fires to all links with value
                if (neuralLinks != null)
                    foreach (NeuralLink link in neuralLinks)
                        link.Fire(value);
            }
            else
            {
                //fires to links with 0 since threshold did not satisfy.
                if (neuralLinks != null)
                    foreach (NeuralLink link in neuralLinks)
                        link.Fire(0);
            }
        }
        public void Start(float input)
        {
            //this gets called only on the initial nodes
            NodeInputs = new float[1];
            NodeInputs[0] = input;
            Fire();
        }
        public void SumInputsAndBias()
        {
            value = 0;
            foreach (float input in NodeInputs)
            {
                value += input;
            }
            value += -bias;
        }
    }
}
