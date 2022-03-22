using Neyron.Brain.Brain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron
{
    public class Neuron
    {
        public List<double> Weights { get; set; }
        public NeuronType NType { get; set; }
        public double Output { get; set; }
        public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
        {
            NType = type;
            Weights = new List<double>();

            for (int i = 0; i < inputCount; i++)
            {
                Weights.Add(1.0);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            var sum = 0.0;

            for (int i = 0; i < inputs.Count; i++)
                sum += inputs[i] * Weights[i];

            if (NType != NeuronType.Input)
                Output = Sigmoid(sum);
            else
                Output = sum;
            return Output;
        }

        public void SetWeights(params double[] weights)
        {
            //TODO удалить после обучения сети

            for (int i = 0; i < weights.Length; i++)
            {
                Weights[i] = weights[i];
            }
        }

        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }
    }
}