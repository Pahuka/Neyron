using Neyron.Brain.Brain;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Neyron
{
    public class Neuron
    {
        public List<double> Weights { get; set; }
        public NeuronType NType { get; set; }
        public List<double> Inputs { get; set; }
        public double Output { get; set; }
        public double Delta { get; set; }
        public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
        {
            NType = type;
            Weights = new List<double>();
            Inputs = new List<double>();
            InitWeights(inputCount);
        }

        private void InitWeights(int inputCount)
        {
            var random = new Random();
            for (int i = 0; i < inputCount; i++)
            {
                if (NType == NeuronType.Input)
                    Weights.Add(1);
                else
                    Weights.Add(random.NextDouble());
                Inputs.Add(0);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            var sum = 0.0;

            for (int i = 0; i < inputs.Count; i++)
            {
                Inputs[i] = inputs[i];
            }

            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }

            if (NType != NeuronType.Input)
                Output = Sigmoid(sum);
            else
                Output = sum;
            return Output;
        }

        public void Learn(double error, double learningRate)
        {
            if (NType == NeuronType.Input)
                return;
            Delta = error * SigmoidDx(Output);

            for (int i = 0; i < Weights.Count; i++)
            {
                var newWeight = Weights[i] - Inputs[i] * Delta * learningRate;
                Weights[i] = newWeight;
            }
        }

        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }

        private double SigmoidDx(double x)
        {
            var sigmoid = Sigmoid(x);
            return sigmoid / (1 - sigmoid);
        }
    }
}