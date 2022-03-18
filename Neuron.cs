using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron
{
    class Neuron
    {
        decimal weight = 0.5m;
        decimal smooth = 0.0001m;
        public decimal LastError { get; private set; }        

        public decimal ProcessInputData(decimal input)
        {
            return input * weight;
        }

        public void Train(decimal input, decimal expectedResult)
        {
            var actualResult = input * weight;
            LastError = expectedResult - actualResult;
            weight += (LastError / actualResult) * smooth;
        }
    }
}
