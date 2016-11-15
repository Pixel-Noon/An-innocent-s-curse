using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundController : MonoBehaviour {

	public List<Sprite> cloudSprites;
	public GameObject cloud; 
	public float maxClouds;
	public bool noClouds;

	private bool isSpawning = false;
	private List<GameObject> clouds;

	// Use this for initialization
	void Start () {
		clouds = new List<GameObject> ();
		if (maxClouds == 0)
			maxClouds = 1000;
	}

	// Update is called once per frame
	void Update () {
		int numOfClouds = 0;
		foreach(GameObject c in clouds){
			if (c.transform.position.x < -10) {
				clouds.Remove (c);
				Destroy (c);
				break;
			}
			float x = c.transform.position.x;
			float width = Camera.main.pixelWidth;
			Vector3 maxcamerax = Camera.main.ScreenToWorldPoint (new Vector3 (width, 0, 10));
			Vector3 mincamerax = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 10));
			if (x < maxcamerax.x && x > mincamerax.x) {
				numOfClouds += 1;
			}
		}

		if (numOfClouds < maxClouds && !isSpawning && !noClouds) {
			isSpawning = true;
			StartCoroutine (SpawnCloud ());
		}
	}

	IEnumerator SpawnCloud(){
		Sprite s = cloudSprites [Random.Range (0, cloudSprites.Count)];
		float speed = Random.Range (1f, 2f);
		float offset = Random.Range (0f, 80f);
		float scale = Random.Range (0.8f, 1.2f);

		float height = Camera.main.pixelHeight;
		float width = Camera.main.pixelWidth;

		Vector3 p = Camera.main.ScreenToWorldPoint (new Vector3 (width+50, height-offset, 10));
		GameObject go = (GameObject) Instantiate (cloud, p, Quaternion.identity);
		Cloud c = go.GetComponent<Cloud> ();
		c.setSprite (s);
		c.setSpeed (speed/scale);
		c.setScale (scale);
		clouds.Add (go);

		float seconds = Random.Range (1f, 3f);

		yield return new WaitForSeconds (seconds);
		isSpawning = false;
	}
}
