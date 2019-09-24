using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Sim Settings", menuName = "Simulation")]
public class SimulationSettings : ScriptableObject {

	[Range(5, 200), Tooltip("How large each generation is.")]
	public int populationSize;
	[Range(1, 200), Tooltip("How much energy each ball starts out with.")]
	public float ballEnergy;
	[Range(0, 10), Tooltip("How heavy the energy drain is when applying torque.")]
	public float energyDrainMultiplier;
	[Range(0, 10), Tooltip("How much/quickly the ball regains its energy.")]
	public float energyRegainMultiplier;
	[Range(0, 2), Tooltip("How much the energy levels (on death) should matter in testing fitness. The lower, the less it weighs into the formula.")]
	public float fitnessEnergyWeightMultiplier;

	[Range(0, 0.95f), Tooltip("How much the ball bounces. Looks like it's a multiplier as to how much momentum gets carried over.")]
	public float ballBounciness;
	[Range(5, 50), Tooltip("How much the ball should turn")]
	public float ballTurnStrength;

	[Tooltip("Use a predetermined launch position & angle.")]
	public bool usePredeterminedLaunch;

	public Vector3 startPos;
	public Vector2 startVelocity;

}
