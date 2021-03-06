using System.Collections;
using System.Collections.Generic;
using System;

public class Genome
{
    private Random random = new Random();
    //public static int skillCount = 5;
    public static int skillCount = 5;

    public float[] weights;
    public int[] skills;

    public Genome(int size)
    {
        weights = new float[size];
        skills = new int[skillCount];
        for (int i = 0; i < size; i++)
        {
            weights[i] = (float)random.NextDouble();
        }
    }

    public Genome(Genome a)
    {
        weights = new float[a.weights.Length];
        Array.Copy(a.weights, 0, weights, 0, a.weights.Length);
        skills = new int[skillCount];
        Array.Copy(a.skills, 0, skills, 0, skillCount);
    }

    public void Mutate(double value)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            if (random.NextDouble() < 0.1) weights[i] += (float)random.NextDouble();
        }
        for (int i = 0; i < skillCount; i++)
        {
            if (random.NextDouble() < 0.05)
            {
                skills[i] = random.Next(0, 4);
            }
        }
    }

}