using UnityEngine;
using System.Collections;

public class VolcanoVisual : MonoBehaviour {

	public Material lavaMaterial;
	public ProceduralMaterial volcanoMaterial;
	public Material eyeMaterial;

	ProceduralMaterial substance;

	public float glowAmount = 0;


	public void Start () {
		SetLavaGlow (.1f);
		SetEyeGlow (.1f);
	}

	// Use this for initialization
	void Update () {

		float shininess = Mathf.PingPong (Time.time, 1.0F);
		SetLavaGlow (shininess);
		float eyeBalls = Mathf.PingPong (Time.time, 2.5f);
		SetEyeGlow (eyeBalls);
	}

	public void SetEyeGlow (float amount) {

		Color c = new Color (amount, amount, amount);

		eyeMaterial.SetColor ("_EmissionColor", c);

	}

	public void SetLavaGlow (float amount) {

		Color c = new Color (amount, amount, amount);

		volcanoMaterial.SetColor ("_EmissionColor", c);
		volcanoMaterial.RebuildTextures ();

		lavaMaterial.SetColor ("_EmissionColor", c);

	}

	//Mountain stuff
	//volcanoMaterial.SetProceduralFloat ("Saturation", glowAmount);
	//substance.SetProceduralFloat ("Hue", .55f);

	//rend = GetComponent<Renderer>();
	//rend.material.shader = Shader.Find("Specular");

	//float shininess = Mathf.PingPong (Time.time, 1.0F);
	//volcanoMaterial.SetFloat ("_Emission", shininess);
	//volcanoMaterial.SetColor ("_EmissionColor", new Color (shininess, shininess, shininess));

	//float lavaGlow = Mathf.PingPong (Time.time, 1.0F);
	//volcanoMaterial.SetFloat ("_Emission", shininess);
	//lavaMaterial.SetColor ("_EmissionColor", new Color (lavaGlow, lavaGlow, lavaGlow));

	//float eyeGlow = Mathf.PingPong (Time.time, 1F);
	//eyeMaterial.SetColor ("_EmissionColor", new Color (eyeGlow, eyeGlow, eyeGlow));


}
