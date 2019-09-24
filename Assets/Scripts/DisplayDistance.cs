using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDistance : MonoBehaviour {
	[SerializeField]
	private Text _text;

	// Use this for initialization
	void Awake () {
		int pos = (int)transform.position.x;
			_text.text = pos.ToString();
	}
}
