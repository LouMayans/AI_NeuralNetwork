using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralLink
    {
        public float weight;
        NeuralNode neuralNode;

        public NeuralLink(NeuralNode node)
        {
            neuralNode = node;
        }
        public void Fire(float input)
        {
            neuralNode.FromLink(input * weight);
        }
    }
}
