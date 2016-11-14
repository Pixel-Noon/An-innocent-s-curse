﻿using UnityEngine;
using System.Collections;

public class People : MonoBehaviour{
	private const double EPS = 1e-9;

	protected Rigidbody2D rb;
	private bool isGrounded;
	protected bool isLeft = true;
	private bool canJump = true;
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
}