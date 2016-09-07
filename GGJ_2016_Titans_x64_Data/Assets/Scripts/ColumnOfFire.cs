using UnityEngine;
using System.Collections;

public class ColumnOfFire : MonoBehaviour {

	public float min, max;
	public float y;
	private float startTimer = 0.0f;
	private float dangerZone;
	private float pillarGrowth;
	public GameObject fireStartCircle;

	// Use this for initialization
	void Start () {
		startTimer = Time.time;
		float r = Random.Range (7, 12);
		transform.localScale = new Vector3(r, transform.localScale.y, r);
		float r1 = Random.Range(-40,40);
		Vector2 direction = Random.insideUnitCircle;
		transform.position = new Vector3 (direction.x * r1, y, direction.y * r1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - startTimer > 6.0f) {
			dangerZone = 1;
		}

		if (dangerZone == 1) {
			pillarGrowth+= 0.02f;
			transform.position = new Vector3 (transform.position.x, pillarGrowth, transform.position.z);
			//scr_death ();
		}

		if (Time.time - startTimer > 16.0f) {
			Destroy(this.gameObject);
		}
	}
}
