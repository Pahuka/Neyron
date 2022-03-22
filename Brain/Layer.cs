using Neyron.Brain.Brain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron.Brain
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; }
        public int Count { get { return (int)Neurons?.Count; } }

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
            Neurons = neurons;
        }

        public List<double> GetSignals()
        {
            var result = new List<double>();
            foreach (var item in Neurons)
            {
                result.Add(item.Output);
            }

            return result;
        }
    }
}
