using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour {
	[SerializeField]
	private Transform ballTransform;
	[SerializeField]
	private Transform attachPoint;


	// Use this for initialization
	void Start () {
		//ballTransform.GetComponent<Ball>().on
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 diff = attachPoint.transform.position - ballTransform.position;


		transform.position = attachPoint.transform.position - (diff/2);

		transform.localScale = new Vector3(diff.x, transform.localScale.y, transform.localScale.z);

		//transform.LookAt(ballTransform);
		//transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
	}
}
