using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	private Rigidbody2D rb;
	private bool isDead = false;

	private Transform player;
	public float radiusOfSight;
	public float speed;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
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
		forward *= radiusOfSight;
		Debug.DrawRay (gameObject.transform.position, forward, Color.green);

		RaycastHit2D hit = Physics2D.Raycast (gameObject.transform.position, forward, radiusOfSight, 1 << LayerMask.NameToLayer("Player"));
		if (hit.collider != null) {
			rb.velocity = new Vector2 (speed * Mathf.Sign (forward.x), rb.velocity.y);
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
