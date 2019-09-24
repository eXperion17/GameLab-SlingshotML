using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour {

	[SerializeField]
	private GameObject backgroundGroup;
	[SerializeField]
	private GameObject landMarkPrefab;
	[SerializeField, Range(1, 25)]
	private float frequency;
	[SerializeField, Range(-5, 5)]
	private float yDistanceVariation;
	[SerializeField, Range(0, 1)]
	private float sizeVariation;
	[SerializeField, Range(15, 50)]
	private float spawnDistance;
	[SerializeField]
	private Transform referencePoint;

	private float newSpawnPos;

	private void Awake() {
		//CreateLandmark(2 + 15);
	}

	private void FixedUpdate() {
		if (referencePoint.position.x + frequency >= newSpawnPos) {
			CreateLandmark(referencePoint.position.x + spawnDistance);

			newSpawnPos = referencePoint.position.x + (frequency*2);
		}
	}

	public void CreateLandmark(float xCord) {
		Vector3 markPos = new Vector3(xCord, transform.position.y, transform.position.z);

		var go = Instantiate(landMarkPrefab, markPos, Quaternion.identity, backgroundGroup.transform);

		var scaleAddition = Random.Range(-sizeVariation, sizeVariation);
		var newScale = new Vector3(go.transform.localScale.x + scaleAddition, go.transform.localScale.y + scaleAddition, go.transform.localScale.z + scaleAddition);
		go.transform.localScale = newScale;

		go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + Random.Range(-yDistanceVariation, yDistanceVariation), go.transform.position.z);
	}
}