using UnityEngine;
using System.Collections;

public class PlayerController : People {

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis ("Horizontal");

		if (horizontal != 0)
			Move (horizontal);

		if (Input.GetKeyDown (KeyCode.Space)) {
			Jump ();
		}
	}
}
