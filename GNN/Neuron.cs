using System.Collections;

namespace Neyron.GNN
{
    public class Neuron
    {
        public float[] weights;
        public float data = 0f;

        public Neuron(int size)
        {
            weights = new float[size];
        }

        public void fire(float[] inputWeights)
        {

        }
    }
}