using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cloud : MonoBehaviour {

	private SpriteRenderer sr;
	private Rigidbody2D rb;

	public void setSprite(Sprite cloudSprite){
		sr = GetComponent<SpriteRenderer> ();
		sr.sprite = cloudSprite;
	}

	public void setSpeed(float speed){
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (-speed, 0);
	}

	public void setScale(float scale){
		transform.localScale = new Vector3 (scale, scale, scale);
	}
}
