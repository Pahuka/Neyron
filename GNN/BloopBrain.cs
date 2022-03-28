using System;
using System.Collections.Generic;
using System.Text;

namespace Neyron.GNN
{
    public class BloopBrain
    {

        public float[] inputLayers, hiddenLayers, outputLayers;
        public float[,] inputWeights, hiddenWeights;
        private Random random = new Random();

        int numberOfInputs = 3;
        int numberOfHidden = 4;
        int numberOfOutputs = 4;
        public float fitness = 0;
        public BloopBrain()
        {
            inputLayers = new float[numberOfInputs];
            hiddenLayers = new float[numberOfHidden];
            outputLayers = new float[numberOfOutputs];
            inputWeights = new float[numberOfHidden, numberOfInputs];
            hiddenWeights = new float[numberOfOutputs, numberOfHidden];

            inputLayers[numberOfInputs - 1] = 1f;
            hiddenLayers[numberOfHidden - 1] = 1f;
        }

        public void GenerationRandomBrain()
        {
            for (int i = 0; i < numberOfHidden; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    inputWeights[i, j] = random.Next(-10, 11);
                }

            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfHidden; j++)
                {
                    hiddenWeights[i, j] = random.Next(-10, 11);
                }

            }
        }

        public void ping()
        {
            for (int i = 0; i < numberOfHidden; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    hiddenLayers[i] += (inputWeights[i, j] * inputLayers[j]);
                }
                hiddenLayers[i] = tanh(hiddenLayers[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfHidden; j++)
                {
                    outputLayers[i] += (hiddenWeights[i, j] * hiddenLayers[j]);
                }
                outputLayers[i] = tanh(outputLayers[i]);
            }

        }

        public float tanh(float x)
        {
            if (x > 20f)
                return 1;
            else if (x < -20f)
                return -1f;
            else
            {
                float a = (float)Math.Exp(x);
                float b = (float)Math.Exp(-x);
                return (a - b) / (a + b);
            }
        }

        public BloopBrain[] Crossover(BloopBrain brain)
        {
            BloopBrain[] cross = new BloopBrain[2];
            int randomHiddenIndex = random.Next(0, numberOfHidden);
            int randomOutputIndex = random.Next(0, numberOfOutputs);

            cross[0] = new BloopBrain();
            cross[1] = new BloopBrain();


            for (int i = 0; i < randomHiddenIndex; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    cross[0].inputWeights[i, j] = inputWeights[i, j];
                    cross[1].inputWeights[i, j] = brain.inputWeights[i, j];
                }

            }

            for (int i = 0; i < randomOutputIndex; i++)
            {
                for (int j = 0; j < numberOfHidden; j++)
                {
                    cross[0].hiddenWeights[i, j] = hiddenWeights[i, j];
                    cross[1].hiddenWeights[i, j] = brain.hiddenWeights[i, j];
                }

            }

            for (int i = randomHiddenIndex; i < numberOfHidden; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    cross[0].inputWeights[i, j] = brain.inputWeights[i, j];
                    cross[1].inputWeights[i, j] = inputWeights[i, j];
                }

            }

            for (int i = randomOutputIndex; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfHidden; j++)
                {
                    cross[0].hiddenWeights[i, j] = brain.hiddenWeights[i, j];
                    cross[1].hiddenWeights[i, j] = hiddenWeights[i, j];
                }

            }


            cross[0].Mutate();
            cross[1].Mutate();
            return cross;
        }

        public void Mutate()
        {
            if (random.Next(0, 100) < 10)
            {
                int hiddenMutate = random.Next(0, 2);
                if (hiddenMutate == 0)
                {
                    int randomHiddenIndex = random.Next(0, numberOfHidden);
                    int randomInputIndex = random.Next(0, numberOfInputs);
                    inputWeights[randomHiddenIndex, randomInputIndex] = random.Next(-10, 11);

                }
                if (hiddenMutate == 1)
                {
                    int randomOutputIndex = random.Next(0, numberOfOutputs);
                    int randomHiddenIndex = random.Next(0, numberOfHidden);
                    hiddenWeights[randomOutputIndex, randomHiddenIndex] = random.Next(-10, 11);
                }
            }
        }
    }
}
