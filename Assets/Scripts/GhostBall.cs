using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostBall : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D _rigidBody;

	[SerializeField]
	private Vector3 stageOffset;
	[SerializeField]
	private GameObject ballSprite;
	[SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private Text energyText;

	public bool launched = false;
	private int identifier = 0;

	private Entity entity;
	[SerializeField, Range(1, 100)]
	private float energy;
	[SerializeField, Range(1, 15)]
	private float drainMultiplier;
	[SerializeField, Range(1, 15)]
	private float gainMultiplier;
	[SerializeField, Range(0, 15)]
	private float energyFitnessWeight;
	[SerializeField, Range(5, 50)]
	private float torqueStrength;

	private float maxEnergy;

	private void Awake() {
		maxEnergy = energy;
		ResetBall();
	}

	public void ApplySimulationSettings(SimulationSettings settings) {
		energy = settings.ballEnergy;
		drainMultiplier = settings.energyDrainMultiplier;
		gainMultiplier = settings.energyRegainMultiplier;
		energyFitnessWeight = settings.fitnessEnergyWeightMultiplier;
		torqueStrength = settings.ballTurnStrength;
	}

	public void Launch(Vector3 originalBall, Vector2 force, int id) {
		identifier = id;
		launched = true;

		transform.position = originalBall - (stageOffset * (identifier+1));
		var colorVariation = UnityEngine.Random.Range(0, 0.1f);
		var color = new Color(entity.GetValue(0f, 1f)+ colorVariation, entity.GetValue(0f, 1f)+ colorVariation, entity.GetValue(0f, 1f)+ colorVariation);
		//baseEnergy = entity.GetValue(50, 150);
		//Debug.Log(color);
		sprite.color = color;

		_rigidBody.bodyType = RigidbodyType2D.Dynamic;
		_rigidBody.velocity = Vector2.zero;
		_rigidBody.AddForce(force, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		if (launched) {
			if (energy.ToString().Length > 4)
				energyText.text = energy.ToString().Substring(0, 3);
			else
				energyText.text = energy.ToString();
			energyText.color = Color.Lerp(Color.red, Color.green, energy / maxEnergy);
			
			var ballPos = new Vector3(transform.position.x, transform.position.y + (stageOffset.y * (identifier+1)), transform.position.z);

			ballSprite.transform.position = ballPos;
			ballSprite.transform.rotation = transform.rotation;


			float appliedTorque = entity.GetValue(0, torqueStrength*2) - torqueStrength;

			if (appliedTorque <= energy) {
				energy -= Mathf.Abs(appliedTorque * drainMultiplier);
				_rigidBody.AddTorque(-appliedTorque);
			}

			energy += gainMultiplier;

			if (energy > maxEnergy)
				energy = maxEnergy;

			if (IsDone()) {
				entity.SetFitness(GetFitness());
			}
		}
	}

	public void ResetBall() {
		_rigidBody.velocity = Vector2.zero;
		transform.position = new Vector3(-4, -3, 0);
		_rigidBody.bodyType = RigidbodyType2D.Kinematic;
		launched = false;
		energy = maxEnergy;
	}

	public bool IsDone() {
		return _rigidBody.velocity.normalized.y == 0;
	}

	public float GetFitness() {
		return transform.position.x + (energy * energyFitnessWeight);
	}

	internal void SetEntity(Entity entity) {
		this.entity = entity;
	}
}
