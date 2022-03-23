using System.Collections;
using System.Collections.Generic;

public class Layer
{
    public int size;
    public double[] neurons;
    public double[,] weights;

    public Layer(int size, int nextSize)
    {
        this.size = size;
        neurons = new double[size];
        weights = new double[size, nextSize];
    }
}