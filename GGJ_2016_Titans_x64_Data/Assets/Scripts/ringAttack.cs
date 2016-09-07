using UnityEngine;
using System.Collections;

public class ringAttack : MonoBehaviour {

	private int radius;
	private int flip;
	public Transform volcanoTransform;


	// Use this for initialization
	void Start () {
		flip = 1;

	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3 (radius, transform.localScale.y, radius);
		radius+=20*flip;
		if (transform.localScale.x > 5000) {
			flip = -1;
		} else if (transform.localScale.x < 10) {
			flip = 1;
		}


		transform.position = new Vector3 (volcanoTransform.position.x, volcanoTransform.position.y, volcanoTransform.position.z);

		//On hit Destroy(
	}
}
