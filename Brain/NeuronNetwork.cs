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

        //public double Learn(double[] expected, double[,] inputs, int epoch)
        //{
        //    var error = 0.0;

        //    for (int i = 0; i < expected.Length; i++)
        //    {
        //        var output = expected[i];
        //        var input = inputs.Get
        //        for (int j = 0; j < epoch; j++)
        //        {

        //        }
        //    }

        //}

        private double[,] Scalling(double[,] inputs)
        {
            var result = new double[inputs.GetLength(0), inputs.GetLength(1)];
            for (int column = 0; column < inputs.GetLength(1); column++)
            {
                var min = inputs[0, column];
                var max = inputs[0, column];
                for (int row = 1; row < inputs.GetLength(0); row++)
                {
                    var item = inputs[row, column];

                    if (item < min)
                        min = item;
                    if (item > max)
                        max = item;
                }
                var divider = max - min;
                for (int row = 1; row < inputs.GetLength(0); row++)
                {
                    result[row, column] = (inputs[row, column] - min) / divider;
                }
            }

            return result;
        }

        private double[,] Normalization(double[,] inputs)
        {
            var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

            for (int column = 0; column < inputs.GetLength(1); column++)
            {
                var sum = 0.0;
                var average = sum / inputs.GetLength(0);
                var error = 0.0;
                var stdError = Math.Sqrt(error / inputs.GetLength(0));

                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    sum += inputs[row, column];
                }

                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    error += Math.Pow((inputs[row, column] - average), 2);
                }


                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    result[row, column] = (inputs[row, column] - average) / stdError;
                }
            }

            return result;
        }

        public List<Neuron> FeedForward(params double[] inputSignals)
        {
            SendInputSignals(inputSignals);
            FeedForwardAllLayers();

            if (Topology.OutputCount == 1)
                //return Layers
                //    .Last().Neurons
                //    .First();
                return Layers.Last().Neurons;
            else
                //return Layers
                //    .Last().Neurons
                //    .OrderByDescending(x => x.Output)
                //    .First();
                return Layers.Last().Neurons;
        }

        public double Learn(List<Tuple<double, double[]>> dataset, int epoch)
        {
            var error = 0.0;

            for (int i = 0; i < epoch; i++)
            {
                foreach (var item in dataset)
                {
                    error += Backpropagation(item.Item1, item.Item2);
                }
            }

            return error / epoch;
        }

        private double Backpropagation(double expected, params double[] inputs)
        {
            var actual = FeedForward(inputs);
            var difference = actual.Last().Output - expected;

            actual.Last().Learn(difference, Topology.LearningRate);

            for (int i = Layers.Count - 2; i >= 0; i--)
            {
                var layer = Layers[i];
                var prevLayer = Layers[i + 1];
                for (int j = 0; j < layer.Count; j++)
                {
                    var neuron = layer.Neurons[j];

                    for (int k = 0; k < prevLayer.Count; k++)
                    {
                        var prevNeuron = prevLayer.Neurons[k];
                        var error = prevNeuron.Weights[j] * prevNeuron.Delta;
                        neuron.Learn(error, Topology.LearningRate);
                    }
                }
            }
            return difference * difference;
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

        private void SendInputSignals(params double[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
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
                Layers.Add(new Layer(hiddenNeurons));
            }
        }

        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();
            var inputLayer = new Layer(inputNeurons, Brain.NeuronType.Input);
            for (int i = 0; i < Topology.InputCount; i++)
            {
                var neuron = new Neuron(1, Brain.NeuronType.Input);
                inputNeurons.Add(neuron);
            }
            Layers.Add(inputLayer);
        }
    }
}