using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyController : People {

	private Transform player;
	private bool walking = false;
	private bool isFollowing = false;
	private int side;

	public float radiusOfSight;
	public EnemyTypes whichEnemy;
	public float radiusOfMovement = 0;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		player = FindObjectOfType<PlayerController> ().gameObject.transform;

		if (radiusOfMovement == 0) {
			radiusOfMovement = Random.Range (-5f, 5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead) {
			if (!isCursed) {
				TryToFollow ();
			}
			if (!walking && !isFollowing) {
				walking = true;
				StartCoroutine (Walking());
			}
		}

	}

	IEnumerator Walking(){
		float distance = radiusOfMovement;
		side = 1;
		Vector3 currPosition = gameObject.transform.position;

		if (distance < 0) {
			side = -1;
			distance *= -1;
		}

		while (Mathf.Abs(currPosition.x - gameObject.transform.position.x) < distance && !isDead) {				
			Move (speed * side);
			yield return null;
		}

		float seconds = Random.Range (0.5f, 2f);
		yield return new WaitForSeconds (seconds);

		radiusOfMovement = Random.Range (1f, 5f) * Mathf.Sign (radiusOfMovement * -1f);
		walking = false;
	}

	void TryToFollow(){
		Vector3 forward = new Vector3(player.position.x - gameObject.transform.position.x, 0, 0);
		forward.Normalize ();

		if (isLeft && forward.x > 0 || !isLeft && forward.x < 0) {
			return;
		}

		forward *= radiusOfSight;

		Debug.DrawRay (gameObject.transform.position, forward, Color.green);

		RaycastHit2D[] hit = Physics2D.RaycastAll (gameObject.transform.position, forward, radiusOfSight);
		if (!isCursed) {
			side =(int) Mathf.Sign (forward.x);
		}
		//foreach(RaycastHit2D h in hit){
		if (hit.Length > 1) {
			if (hit [1].collider != null && hit [1].collider.tag == "Player") {
				isFollowing = true;
				Move (speed * side);
			} else {
				isFollowing = false;
				rb.velocity = new Vector2 (0, rb.velocity.y);
			}
		} else {
			isFollowing = false;
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}

	void OnCollisionEnter2D (Collision2D coll){
		People people;
		if ((people = coll.gameObject.GetComponent<People>()) != null && people.CheckCurse()) {
			side *= -1;
			if (!isCursed) {
				isCursed = true;
				isFollowing = false;
				side *= -1;
				StartCoroutine (KillByCurse ());
				rb.velocity = new Vector2 (0, rb.velocity.y);
			}	
		}
	}
}