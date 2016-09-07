using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour
{
    public float m_lifetime = 0;
    private float m_time = 0;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_time += Time.fixedDeltaTime;

        if (m_time >= m_lifetime)
            GameObject.Destroy(this.gameObject);
		    
    }
}
