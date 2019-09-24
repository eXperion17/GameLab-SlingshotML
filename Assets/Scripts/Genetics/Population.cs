using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Reused from the Genetic Algorithm example
/// </summary>
public class Population
{
    const int GeneCount = 150;

    private int count;
    
    public List<Entity> entities;

    public Population(int count) : base()
    {
        this.count = count;
        entities = new List<Entity>();
    }

    public List<Entity> GetCurrentEntities()
    {
        return entities;
    }

    public float GetTotalFitness() {
        float fitness = 0.0f;
        foreach (Entity entity in entities) {
            fitness += entity.fitness;
        }
        return fitness;
    }
    
    public void InitializeRandomly() 
    {
        for (int i = 0; i < count; i++) 
        {
            Entity entity = new Entity(new Chromosome(GeneCount));
            AddEntity(entity);
        }
    }

    private void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public Population GetOffspring()
    {
        Population offSpring = new Population(count);
        List<Entity> weightedSelection = GetWeightedSelection();
        
        for (int i = 0; i < count; i++)
        {
            Entity child = null;
            if (weightedSelection.Count > 0)
            {
                Entity parent1 = weightedSelection[UnityEngine.Random.Range(0, weightedSelection.Count)];
                Entity parent2 = weightedSelection[UnityEngine.Random.Range(0, weightedSelection.Count)];
                if (parent1 != null && parent2 != null)
                {
                    child = new Entity(parent1.chromosome.Breed(parent2.chromosome));
                }
            }				
            if (child == null) { //randomize if needed
                child = new Entity(new Chromosome(GeneCount));
            }
            offSpring.AddEntity(child);
        }

        return offSpring;
    }

	public float GetFurthestDistance() {
		float fitness = 0;
		entities.ForEach(x => { if (x.fitness > fitness) fitness = x.fitness; });
		return fitness;
	}

	private List<Entity> GetWeightedSelection()
    {
        List<Entity> weightedSelection = new List<Entity>();
        entities.Sort((x, y) =>
        {
            return x.fitness.CompareTo(y.fitness);
        });
        entities.Reverse();

        int count = 0;
        foreach (Entity entity in entities) {
            weightedSelection.Add(entity);
            count ++;
            if (count >= 5) break;
        }
        
        return weightedSelection;
    }
}

