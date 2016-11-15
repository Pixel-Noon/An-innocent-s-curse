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
		foreach(GameObject c in clouds){
			Vector3 p = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 10));
			if (c.transform.position.x < p.x) {
				clouds.Remove (c);
			}

			if (c.transform.position.x < -10) {
				Destroy (c);
				break;
			}
		}

		if (clouds.Count < maxClouds && !isSpawning && !noClouds) {
			isSpawning = true;
			StartCoroutine (SpawnCloud ());
		}
	}

	IEnumerator SpawnCloud(){
		Sprite s = cloudSprites [Random.Range (0, cloudSprites.Count)];
		float speed = Random.Range (1f, 2f);
		float offset = Random.Range (0f, 100f);
		float scale = Random.Range (0.8f, 1.2f);

		float height = Camera.main.pixelHeight;
		float width = Camera.main.pixelWidth;

		Vector3 p = Camera.main.ScreenToWorldPoint (new Vector3 (width, height-offset, 10));
		GameObject go = (GameObject) Instantiate (cloud, p, Quaternion.identity);
		print (p);
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
