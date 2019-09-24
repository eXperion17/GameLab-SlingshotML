using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class VectorTwoEvent : UnityEvent<Vector2> {}

public class Ball : MonoBehaviour {
	private Vector3 mouseOffset;
	private bool isDragging;
	private Rigidbody2D _rigidBody;

	public Transform centerLaunch;

	[HideInInspector]
	public UnityEvent onMouseDown;
	[HideInInspector]
	public UnityEvent onMouseDrag;
	[HideInInspector]
	public UnityEvent onMouseUp;

	public VectorTwoEvent onBallLaunch;

	private void Awake() {
		//onBallLaunch = new VectorTwoEvent();
		_rigidBody = GetComponent<Rigidbody2D>();
		_rigidBody.bodyType = RigidbodyType2D.Kinematic;
	}

	private void Update() {
		/*if (Input.GetKey(KeyCode.D)) {
			_rigidBody.AddTorque(-5);
		}*/
	}

	private void OnMouseDrag() {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;

		onMouseDrag.Invoke();
	}

	private void OnMouseUp() {
		onMouseUp.Invoke();
		isDragging = false;

		_rigidBody.bodyType = RigidbodyType2D.Dynamic;
		_rigidBody.velocity = Vector2.zero;
		var launchForce = (centerLaunch.transform.position - transform.position) * 5 * _rigidBody.mass;
		_rigidBody.AddForce(launchForce, ForceMode2D.Impulse);

		onBallLaunch.Invoke(launchForce);
	}

	private void OnMouseDown() {
		mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

		onMouseDown.Invoke();
		isDragging = true;
	}

	private void OnDrawGizmos() {
		Vector3 launch = centerLaunch.transform.position - transform.position;

		Vector3 direction = transform.position + (launch);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, direction);
	}
}
