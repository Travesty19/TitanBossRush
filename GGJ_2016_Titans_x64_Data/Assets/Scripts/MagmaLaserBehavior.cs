using UnityEngine;
using System.Collections;

public class MagmaLaserBehavior : MonoBehaviour
{
    public float m_thickness = 1.0f;

    private Transform m_handle;
    private Transform m_visual;
    private Transform m_burn;

    // Use this for initialization
    void Start()
    {
        m_handle = transform.FindChild("handle");
        m_visual = transform.FindChild("visual");
        m_burn = transform.FindChild("burn");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBeam();
    }

    public void UpdateBeam()
    {
        Vector3 offset = m_handle.position - transform.position;
        Vector3 direction = Vector3.Normalize(offset);
        Vector3 hitpoint = m_handle.position;
        float distance = offset.magnitude;

        RaycastHit[] hit = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(transform.position, direction, hit, distance, Layers.WORLD) > 0)
        {
            m_burn.FindChild("Particle Burn").GetComponent<ParticleSystem>().Play();
            m_burn.position = hit[0].point;
            hitpoint = hit[0].point;
            distance = hit[0].distance;
        }
        else
        {
            m_burn.FindChild("Particle Burn").GetComponent<ParticleSystem>().Stop();
            m_burn.position = m_handle.position;
        }


        Vector3 midpoint = (transform.position + hitpoint) / 2.0f;

        m_visual.position = midpoint;
        m_visual.localScale = new Vector3(m_thickness, m_thickness, distance);
        m_visual.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
    }
}
