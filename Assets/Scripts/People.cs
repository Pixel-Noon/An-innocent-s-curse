using UnityEngine;
using System.Collections;

public class People : MonoBehaviour{
	private const double EPS = 1e-9;

	private bool canJump = true;
	private bool isGrounded;

	protected bool isDead = false;
	protected Rigidbody2D rb;
	protected bool isLeft = true;
	protected bool isCursed;
	protected SpriteRenderer sr;

	public float speed;
	public float jumpSpeed;
		
	public void Flip(){
		sr.flipX = isLeft;
	}

	public void Jump(){
		if (!isGrounded && !canJump)
			return;
		else {
			if (!isGrounded)
				canJump = false;
			rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
			isGrounded = false;
		}
	}

	public void Move(float horizontal){
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

	protected IEnumerator KillByCurse(){
		speed *= 1.3f;
		yield return new WaitForSeconds (5);
		print (gameObject.name + " is dead");
		isDead = true;
	}

	public bool CheckCurse(){
		return isCursed;
	}
}