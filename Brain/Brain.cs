using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron.Brain
{
    public class Brain1
    {
        public NeuronNetwork brain { get; private set; }

        public Brain1(NeuronNetwork neuronNetwork)
        {
            brain = neuronNetwork;
        }
    }
}
