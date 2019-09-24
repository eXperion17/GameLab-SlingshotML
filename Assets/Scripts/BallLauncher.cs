using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BallLauncher : MonoBehaviour {

	[Header("Adjust this!")]
	public SimulationSettings settings;

	[Header("References")]
	[SerializeField]
	private Ball mainBall;
	[SerializeField]
	private GameObject levelPrefab;
	[SerializeField]
	private GameObject ghostBallPrefab;
	[SerializeField]
	private PhysicsMaterial2D ballPhysics;
	[SerializeField]
	private Transform baseLevel;
	[SerializeField]
	private Transform levelParent;
	[SerializeField]
	private Transform ghostBallsParent;

	[Header("Settings")]
	[SerializeField, Range(1, 100)]
	private int populationCount; //move to own class
	[SerializeField, Range(0.1f, 5)]
	private float timeToSpawn;
	[SerializeField]
	private Vector3 stageOffset;

	[Header("UI")]
	public Text generationText;
	public Text bestDistText, popCountText, baselineDistText, lastGenDistText;

	[Header("Predetermined Variables")]
	public Vector3 simulationStartPosition;
	public Vector2 simulationLaunchVelocity;

	private Vector3 mainBallStart;
	private List<GameObject> levels;
	private List<GhostBall> ghostBalls;
	private Vector2 launchForce;
	private Population population;
	private float bestGenDistance = 0;
	
	private int generation;

	private bool firstRun = true;

	private void Awake() {
		levels = new List<GameObject>();
		ghostBalls = new List<GhostBall>();

		//saveLaunchInfo.onClick.AddListener(OnSaveLaunchClick());
		ApplySimulationSettings();

		population = new Population(populationCount);
		population.InitializeRandomly();

		UpdateUI();

		//replace aaaaaaa
		mainBall.onBallLaunch.AddListener(OnBallLaunch);
		mainBallStart = mainBall.transform.position;

		var entities = population.GetCurrentEntities();
		for (int i = 0; i < populationCount; i++) {
			var newPos = baseLevel.position - stageOffset * (i+1);
			var floor = Instantiate(levelPrefab, newPos, Quaternion.identity, levelParent);
			levels.Add(floor);

			var ball = Instantiate(ghostBallPrefab, new Vector3(-20, 0, 0), Quaternion.identity, ghostBallsParent);
			var ghostBall = ball.GetComponent<GhostBall>();
			ghostBall.SetEntity(entities[i]);
			ghostBalls.Add(ghostBall);
		}

		Camera.main.GetComponent<FollowBall>().AddBallsToFocus(ghostBalls);

		if (settings.usePredeterminedLaunch) {
			mainBall.transform.position = settings.startPos;
			//mainBall.GetComponent<Rigidbody2D>().velocity = settings.startVelocity;
			OnBallLaunch(settings.startVelocity);
		}
	}

	public void OnSaveLaunchClick() {
		settings.startPos = simulationStartPosition;
		settings.startVelocity = simulationLaunchVelocity;
	}

	public void ApplySimulationSettings() {
		if (firstRun)
			populationCount = settings.populationSize;

		ballPhysics.bounciness = settings.ballBounciness;
		if (firstRun) {
			var ghost = ghostBallPrefab.GetComponent<GhostBall>();
			ghost.ApplySimulationSettings(settings);
		}/* else {
			ghostBalls.ForEach(x => x.ApplySimulationSettings(settings));
		}*/
		
	}


	public void OnBallLaunch(Vector2 force) {
		//ApplySimulationSettings();
		if (firstRun) {
			simulationStartPosition = mainBall.transform.position;
			simulationLaunchVelocity = force;
		}

		launchForce = force;
		StartCoroutine(LaunchBalls(force));
	}

	private IEnumerator LaunchBalls(Vector2 force) {
		if (firstRun) {
			mainBallStart = mainBall.transform.position;
			firstRun = false;
		}
		//var startPos = mainBallStart;
		var delay = timeToSpawn / populationCount;
		for (int i = 0; i < populationCount; i++) {
			yield return new WaitForSeconds(delay);
			ghostBalls[i].Launch(mainBallStart, force, i);
		}
	}

	public void FixedUpdate() {
		if (ghostBalls[0].launched) {
			var allDone = true;
			ghostBalls.ForEach(x => { if (!x.IsDone() && x.launched) allDone = false; });

			if (allDone)
				ResetPopulation(false);
		}
	}

	public void ResetPopulation(bool resetLaunch) {
		generation++;
		UpdateUI();

		population = population.GetOffspring();
		ReassignEntities();
		ghostBalls.ForEach(x => x.ResetBall());
		OnBallLaunch(launchForce);

	}

	private void UpdateUI() {
		baselineDistText.text = mainBall.transform.position.x.ToString();

		generationText.text = generation.ToString();

		lastGenDistText.text = GetFurthestDistance().ToString();

		if (population.GetFurthestDistance() > bestGenDistance) {
			bestDistText.text = GetFurthestDistance().ToString();
			bestGenDistance = population.GetFurthestDistance();
		}
		
		popCountText.text = populationCount.ToString();
	}

	private float GetFurthestDistance() {
		if (ghostBalls.Count == 0)
			return 0;

		ghostBalls.Sort((x, y) =>
		{
			return x.transform.position.x.CompareTo(y.transform.position.x);
		});
		ghostBalls.Reverse();

		return ghostBalls[0].transform.position.x;
	}

	private void ReassignEntities() {
		var entities = population.GetCurrentEntities();
		for (int i = 0; i < entities.Count; i++) {
			ghostBalls[i].ResetBall();
			ghostBalls[i].SetEntity(entities[i]);
		}
	}
}
