using UnityEngine;
using System.Collections;
using System.Linq;

public class VolcanoBossBehavior : MonoBehaviour
{
    public int m_health = 20;

    public float m_moveSpeed = 1.0f;
    public float m_fireballDelay = 0.5f;
    public float m_invulnerabilityTime = 3.0f;
    public float m_stunTime = 0.5f;

    public bool m_damageMe = false;
    
    private int m_state = 0;
    private int m_substate = 0;
    private float m_time = 0.0f;

    private Vector3 m_direction = Vector3.forward;

    private float m_fireballTime = 0.0f;
    private float m_delayTime = 0.0f;
    private float m_directionTime = 0.0f;
    private float m_invulnerabilityTimer = 0.0f;

    private bool m_fireballsOn = true;

    // Use this for initialization
    void Start()
    {
        m_direction = Vector3.Normalize(Vector3.Scale(Random.onUnitSphere, new Vector3(1.0f, 0.0f, 1.0f)));
        m_directionTime = Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_damageMe)
        {
            InflictDamage(1, Vector3.Normalize(Vector3.Scale(transform.position - PlayerController.Instance.transform.position, new Vector3(1.0f, 0.0f, 1.0f))));
            m_damageMe = false;
        }

        m_invulnerabilityTimer = Mathf.Max(m_invulnerabilityTimer - Time.deltaTime, 0.0f);

        StateMain();
        FireballMain();
    }

    public bool InflictDamage(int amount, Vector3 direction)
    {
        m_health -= amount;

        if (m_invulnerabilityTimer <= 0.0f)
        {
            m_invulnerabilityTimer = m_invulnerabilityTime;
            m_direction = direction;
            ChangeState(2);
        }

        return true;
    }

    private void FireballMain()
    {
        if (!m_fireballsOn)
            return;

        m_fireballTime -= Time.deltaTime;

        if (m_fireballTime <= 0.0f)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 2;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up);
            GameObject obj = GameObject.Instantiate(ResourceModule.Instance.m_prefabs.m_prefFireball, spawnPosition, spawnRotation) as GameObject;
            FireballBehavior fireball = obj.GetComponent<FireballBehavior>();
            
            Vector3 random = Random.onUnitSphere * 30;
            random.y = 45;
            fireball.m_velocity = random;

            m_fireballTime = m_fireballDelay;
        }


    }

    private void StateMain()
    {
        if (m_state == 0) // Wait
        {
            if (m_substate == 0)
            {
                m_fireballsOn = true;
                m_delayTime = Random.Range(0.4f, 1.2f);
                m_substate = 1;
            }

            if (m_substate == 1)
            {
                if (m_time > m_delayTime)
                {
                    ChangeState(1);
                }
            }

            m_time += Time.deltaTime;
        }
        else if (m_state == 1) // Move
        {
            if (m_substate == 0)
            {
                m_direction = Vector3.Normalize(Vector3.Scale(Random.onUnitSphere, new Vector3(1.0f, 0.0f, 1.0f)));
                m_directionTime = Random.Range(1.4f, 2.4f);
                m_fireballsOn = true;
                m_substate = 1;
            }

            if (m_substate == 1)
            {
                float step = m_moveSpeed * Time.deltaTime;

                const float k_radius = 3.2f;
                Vector3 castFrom = transform.position + Vector3.up * k_radius;
                Ray ray = new Ray(castFrom, m_direction);
                RaycastHit[] hits = OptimizedCast.SphereCastAll(ray, k_radius, step, Layers.WORLD);

                if (hits.Length == 0)
                {
                    transform.position += m_direction * step;

                    if (m_time > m_directionTime)
                    {
                        ChangeState(1);
                    }
                }
                else
                {
                    RaycastHit nearest = hits.OrderBy(x => x.distance).First();
                    transform.position += m_direction * nearest.distance;

                    ChangeState(1);
                }
            }

            m_time += Time.deltaTime;
        }
        else if (m_state == 2) // Damage
        {
            if (m_substate == 0)
            {
                m_fireballsOn = false;
                const float k_knockbackSpeed = 20.0f;
                const float k_knockbackTime = 0.2f;

                m_time += Time.deltaTime;

                float step = k_knockbackSpeed * Time.deltaTime;

                const float k_radius = 3.2f;
                Vector3 castFrom = transform.position + Vector3.up * k_radius;
                Ray ray = new Ray(castFrom, m_direction);
                RaycastHit[] hits = OptimizedCast.SphereCastAll(ray, k_radius, step, Layers.WORLD);

                if (hits.Length == 0)
                {
                    transform.position += m_direction * step;


                }

                if (m_time > k_knockbackTime)
                {
                    m_time = 0.0f;
                    m_substate = 1;
                }
            }
            else
            {
                m_time += Time.deltaTime;

                if (m_time > m_stunTime)
                {
                    if (m_health > 0)
                        ChangeState(1);
                    else
                        ChangeState(3);
                }
            }
        }
        else if (m_state == 3) // Dead
        {
            m_invulnerabilityTimer = 1.0f;
            m_fireballsOn = false;
            m_time += Time.deltaTime;

            Transform collider = transform.FindChild("collider");
            collider.GetComponent<CapsuleCollider>().enabled = false;

            if (m_substate == 0)
            {
                GetComponent<LookAt>().enabled = false;

                FlamePillarSpawnerBehavior spawner1 = GameObject.FindObjectOfType<FlamePillarSpawnerBehavior>();
                if (spawner1 != null)
                    GameObject.Destroy(spawner1.gameObject);

                ringAttack ring = GameObject.FindObjectOfType<ringAttack>();
                if (ring != null)
                    GameObject.Destroy(ring.gameObject);

                MagmaBeamerBehavior[] beamers = GameObject.FindObjectsOfType<MagmaBeamerBehavior>();
                foreach (MagmaBeamerBehavior beamer in beamers)
                    GameObject.Destroy(beamer.gameObject);

                Vector3 spawnPoint = transform.position + Vector3.up * 2.0f;
                GameObject.Instantiate(ResourceModule.Instance.m_effects.m_prefBossExplosion, spawnPoint, Quaternion.identity);
                m_substate = 1;
            }
            else if (m_substate == 1)
            {
                if (m_time > 11.0f)
                {
                    GameObject.FindObjectOfType<CameraController>().m_targets.Remove(this.transform);
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }

    private void ChangeState(int value)
    {
        m_state = value;
        m_substate = 0;
        m_time = 0.0f;
    }
}
