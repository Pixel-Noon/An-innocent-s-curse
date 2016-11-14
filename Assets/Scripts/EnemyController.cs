using UnityEngine;
using System.Collections;

public class EnemyController : People {

	private bool isDead = false;
	private Transform player;

	public float radiusOfSight;
	public EnemyTypes whichEnemy;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		player = FindObjectOfType<PlayerController> ().gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead) {
			Raycasting ();
		}
	}

	void Raycasting(){
		Vector3 forward = new Vector3(player.position.x - gameObject.transform.position.x, 0, 0);
		forward.Normalize ();

		if (isLeft && forward.x > 0 || !isLeft && forward.x < 0)
			return;

		forward *= radiusOfSight;

		Debug.DrawRay (gameObject.transform.position, forward, Color.green);

		RaycastHit2D hit = Physics2D.Raycast (gameObject.transform.position, forward, radiusOfSight, 1 << LayerMask.NameToLayer("Player"));
		if (hit.collider != null) {
			Move (speed * Mathf.Sign(forward.x));
		} else {
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
