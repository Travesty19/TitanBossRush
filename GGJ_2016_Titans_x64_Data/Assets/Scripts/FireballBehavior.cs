using UnityEngine;
using System.Collections;

public class FireballBehavior : MonoBehaviour
{
    public float m_gravity = 1.0f;
    public float m_airDrag = 1.0f;

    public Vector3 m_velocity = Vector3.zero;


    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lateralMotion = Vector3.Scale(m_velocity, new Vector3(1.0f, 0.0f, 1.0f));
        Vector3 lateralDirection = Vector3.Normalize(lateralMotion);

        float lateralSpeed = Vector3.Dot(m_velocity, lateralDirection);
        float dampening = Mathf.Min(m_airDrag * Time.deltaTime, lateralSpeed);

        m_velocity -= Vector3.up * m_gravity * Time.deltaTime;
        m_velocity -= lateralDirection * dampening;

        Vector3 lastPosition = transform.position;
        transform.position += m_velocity * Time.deltaTime;

        Vector3 direction = Vector3.Normalize(m_velocity);
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);

        Ray ray = new Ray(lastPosition, Vector3.Normalize(transform.position - lastPosition));
        RaycastHit[] hits = OptimizedCast.SphereCastAll(ray, 0.5f, Vector3.Distance(transform.position, lastPosition), Layers.WORLD);
        if (hits.Length > 0)
        {
            GameObject.Instantiate(ResourceModule.Instance.m_prefabs.m_prefExplosionWave, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
