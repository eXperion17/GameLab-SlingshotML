using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Reused from the Genetic Algorithm example
/// </summary>
public class Entity
{
	public Chromosome chromosome = null;
	public float fitness = 0.0f;
    private int index = 0;
	
	public Entity(Chromosome chromosome)
	{
		this.chromosome = chromosome;
		fitness = 0.0f;
	}

    public void SetFitness(float fitness) 
	{
        this.fitness = fitness;
    }
		
	public int GetInputValue()
	{
		if (index >= chromosome.data.Length) index = 0;
		int data = chromosome.data[index];
		index++;
		return data;
	}

    public bool HasReachedEnd() 
	{
        return (index >= chromosome.data.Length);
    }

	public int GetValue(int min, int max) 
	{
		return min + GetInputValue() * (max - min) / 255;
	}

	public float GetValue(float min, float max) 
	{
		return min + GetInputValue() * (max - min) / 255f;
	}

	public Chromosome GetChromosome() {
        return chromosome;
    }
	
}

