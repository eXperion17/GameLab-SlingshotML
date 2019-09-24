using UnityEngine;


/// <summary>
/// Reused from the Genetic Algorithm example
/// </summary>
public class Chromosome {
	public int[] data;
	public int length;

	public Chromosome(int length) {
		this.length = length;
		data = new int[length];

		for (int i = 0; i < length; i++) {
			data[i] = Random.Range(0, 255);
		}
	}

	public Chromosome Breed(Chromosome other) {
		Chromosome child = new Chromosome(length);
		Chromosome parent = this;

		int breakPoint1 = Random.Range(0, length);
		int breakPoint2 = Random.Range(0, length);

		for (int i = 0; i < length; i++) {
			child.data[i] = parent.data[i];
			if (i == breakPoint1) parent = (parent == this) ? other : this;
			if (i == breakPoint2) parent = (parent == this) ? other : this;
		}

		for (int j = 0; j < (data[0]/10); j++) {
			child.data[Random.Range(0, child.data.Length)] = Random.Range(0, 255);
		}
		return child;
	}

	public int[] GetData() {
		return data;
	}
	/*
	public override string ToString() {
		return string.Join(",", data);
	}*/
}