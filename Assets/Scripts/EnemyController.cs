using UnityEngine;
using System.Collections;

public class EnemyController : People {

	private bool isDead = false;
	private Transform player;
	private bool walking = false;
	private bool isFollowing = false;

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
			Raycasting ();
			if (!walking && !isFollowing) {
				walking = true;
				StartCoroutine (Walking());
			}
		}

	}

	IEnumerator Walking(){
		float distance = radiusOfMovement;
		float side = 1;
		Vector3 currPosition = gameObject.transform.position;

		if (distance < 0) {
			side = -1f;
			distance *= -1;
		}

		while (Mathf.Abs(currPosition.x - gameObject.transform.position.x) < distance) {
			Move (speed * side);
			yield return null;
		}

		float seconds = Random.Range (0.5f, 2f);
		yield return new WaitForSeconds (seconds);

		radiusOfMovement = Random.Range (1f, 5f) * Mathf.Sign (radiusOfMovement * -1f);
		walking = false;
	}

	void Raycasting(){
		Vector3 forward = new Vector3(player.position.x - gameObject.transform.position.x, 0, 0);
		forward.Normalize ();

		if (isLeft && forward.x > 0 || !isLeft && forward.x < 0) {
			return;
		}

		forward *= radiusOfSight;

		Debug.DrawRay (gameObject.transform.position, forward, Color.green);

		RaycastHit2D hit = Physics2D.Raycast (gameObject.transform.position, forward, radiusOfSight, 1 << LayerMask.NameToLayer("Player"));
		if (hit.collider != null) {
			isFollowing = true;
			Move (speed * Mathf.Sign(forward.x));
		} else {
			isFollowing = false;
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}

	void OnCollisionEnter2D (Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			isDead = true;
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}
}
