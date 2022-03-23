using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron.Brain
{
    public class Topology
    {
        public int OutputCount { get; private set; }
        public List<int> HiddenLayers { get; private set; }
        public int InputCount { get; private set; }
        public double LearningRate { get; set; }

        public Topology(int inputCount, int outputCount, double learningRate, params int[] layers)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            LearningRate = learningRate;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
        }
    }
}
