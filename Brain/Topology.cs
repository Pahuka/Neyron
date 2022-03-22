using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron.Brain
{
    public class Topology
    {
        public int ImputCount { get; }
        public int OutputCount { get; }
        public List<int> HiddenLayers { get; }
        public int InputCount { get; }

        public Topology(int inputCount, int outputCount, params int[] layers )
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
        }
    }
}
