using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBall : MonoBehaviour
{
	[SerializeField]
	private List<Transform> ballPositions;

	[SerializeField]
	private bool reverse;

	private Vector3 ballOffset;
	private Camera _camera;

	// Use this for initialization
	void Start () 
	{
		ballOffset = transform.position - GetAveragePosition();
		_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		//if (GetAveragePosition().x > -2) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(GetAveragePosition().x, transform.position.y, transform.position.z), 5);
			
			var spread = SpreadDistance();
			if (spread > 15) {
				//cam increase
				_camera.orthographicSize = 5 + spread / 5;
			} else {
				_camera.orthographicSize = 5;
			}
		//}
	}

	public float SpreadDistance() {
		ballPositions.Sort((x, y) => {
			return x.transform.position.x.CompareTo(y.transform.position.x);
		});

		return Mathf.Abs(ballPositions[0].transform.position.x - ballPositions[ballPositions.Count - 1].transform.position.x);
	}

	public void AddBallsToFocus(List<GhostBall> balls) 
	{
		balls.ForEach(x => ballPositions.Add(x.transform));
	}


	public Vector3 GetAveragePosition() {
		Vector3 total = Vector3.zero;
		foreach (var ball in ballPositions) {
			total += ball.position;
		}

		return total / ballPositions.Count;
	}

}
