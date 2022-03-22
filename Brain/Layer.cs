using Neyron.Brain.Brain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron.Brain
{
    class Layer
    {
        public List<Neuron> Neurons { get; set; }
        public int Count { get { return (int)Neurons?.Count; } }

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
            Neurons = neurons;
        }
    }
}
