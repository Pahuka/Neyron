using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neyron.Brain
{
    public class NeuronNetwork
    {
        public Topology Topology { get; set; }
        public List<Layer> Layers { get; set; }

        public NeuronNetwork(Topology topology)
        {
            Topology = topology;
            Layers = new List<Layer>();

            CreateInputLayer();
            CreateHiddenLayer();
            CreateOutputLayer();
        }

        public Neuron FeedForward(List<double> inputSignals)
        {
            SendInputSignals(inputSignals);
            FeedForwardAllLayers();

            if (Topology.OutputCount == 1)
                return Layers
                    .Last().Neurons
                    .First();
            else
                return Layers
                    .Last().Neurons
                    .OrderByDescending(x => x.Output)
                    .First();
        }

        private void FeedForwardAllLayers()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var prevLayer = Layers[i - 1].GetSignals();
                var layer = Layers[i];

                foreach (var item in layer.Neurons)
                {
                    item.FeedForward(prevLayer);
                }
            }
        }

        private void SendInputSignals(List<double> inputSignals)
        {
            for (int i = 0; i < inputSignals.Count; i++)
            {
                var signal = new List<double>() { inputSignals[i] };
                var neuron = Layers.First().Neurons.First();
                neuron.FeedForward(signal);
            }
        }

        private void CreateOutputLayer()
        {
            var outputNeurons = new List<Neuron>();
            var outputLayer = new Layer(outputNeurons, Brain.NeuronType.Output);
            var lastLayer = Layers.Last();
            for (int i = 0; i < Topology.OutputCount; i++)
            {
                var neuron = new Neuron(lastLayer.Count, Brain.NeuronType.Output);
                outputNeurons.Add(neuron);
            }
            Layers.Add(outputLayer);
        }

        private void CreateHiddenLayer()
        {
            for (int j = 0; j < Topology.HiddenLayers.Count; j++)
            {

                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();
                for (int i = 0; i < Topology.HiddenLayers[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.Count);
                    hiddenNeurons.Add(neuron);
                }
                Layers.Add(lastLayer);
            }
        }

        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();
            var inputLayer = new Layer(inputNeurons, Brain.NeuronType.Input);
            for (int i = 0; i < Topology.ImputCount; i++)
            {
                var neuron = new Neuron(1, Brain.NeuronType.Input);
                inputNeurons.Add(neuron);
            }
            Layers.Add(inputLayer);
        }
    }
}