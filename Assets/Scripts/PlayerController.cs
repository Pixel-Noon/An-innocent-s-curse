using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private const double EPS = 1e-9;

	private Rigidbody2D rb;
	private bool isGrounded;
	private bool isLeft = false;
	private bool canJump = true;
	private SpriteRenderer sr;

	public float speed;
	public float jumpSpeed;

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

	void Flip(){
		sr.flipX = isLeft;
	}

	void Jump(){
		if (!isGrounded && !canJump)
			return;
		else {
			if (!isGrounded)
				canJump = false;
			rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
			isGrounded = false;
		}
	}

	void Move(float horizontal){
		if (horizontal < -EPS)
			isLeft = true;
		else if (horizontal > EPS)
			isLeft = false;
		Flip ();

		rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
	}


	// Migué
	void OnCollisionEnter2D (Collision2D col){
		Check (col);
	}

	void OnCollisionStay2D (Collision2D col){
		Check (col);
	}

	void OnCollisionExit2D (Collision2D col){
		Check (col, false);
	}

	void Check(Collision2D col, bool check = true){
		if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Enemy") {
			isGrounded = check;
			canJump = true;
		}
	}
}
