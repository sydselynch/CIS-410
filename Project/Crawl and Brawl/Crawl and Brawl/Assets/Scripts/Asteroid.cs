using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.transform.tag == "damage") {
			Destroy (gameObject);
		}
	}
}
