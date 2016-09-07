using UnityEngine;
using System.Collections;

public class BlastVisual : MonoBehaviour {

	public float explosionScrollSpeed = .5f;

	private Renderer rend;

	public void Start () {
		rend = GetComponent<Renderer> ();
	}

	// Use this for initialization
	void Update () {
		UpdateBlastMaterial ();
	}

	void UpdateBlastMaterial () {

		float offset = Time.time * explosionScrollSpeed;
		rend.material.SetTextureOffset ("_MainTex", new Vector2 (0, offset));
	}




}
